using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 5f; 
    [SerializeField] private float deceleration = 5f; 

    [SerializeField] private float groundCheckDistance = 0.1f;

    public bool isGroundedBool = false;

    

    private Vector2 inputVector;
    private Vector3 movementVelocity;


   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   
    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        
        //push player in input direction
        if (inputVector.magnitude > 0)
        {
            movementVelocity = new Vector3(inputVector.x, 0, inputVector.y) * acceleration;
        }
        else
        {
            movementVelocity = Vector3.zero;
        }

        //decelerate player if no input
        if (inputVector.magnitude == 0)
        {
            movementVelocity = Vector3.Lerp(movementVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        //clamp velocity to max speed
        movementVelocity = Vector3.ClampMagnitude(movementVelocity, maxSpeed);

        //apply velocity to rigidbody
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);
        

        

        
        
    }

    private void Rotate()
    {
        //rotate in movement direction
        if (inputVector.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y));
        }
    }

   

    
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ground"))
        {
            isGroundedBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            isGroundedBool = false;
        }
    }


    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);

        //look direction gizmo show velocity direction
        Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, rb.velocity.normalized);

        //input direction gizmo show input direction
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(inputVector.x, 0, inputVector.y));

        //movement direction gizmo show movement direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, movementVelocity.normalized);


    }

    

   




   

    #region Inputse

    public void OnMove(InputValue value)
    {
       inputVector = value.Get<Vector2>();

       
    }

    public void OnAction()
    {
        Debug.Log("Action");
    }

    public void OnESC()
    {
        Debug.Log("ESC");
        Application.Quit();
    }

    #endregion
}
