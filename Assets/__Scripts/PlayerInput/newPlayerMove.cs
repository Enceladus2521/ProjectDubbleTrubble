using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class newPlayerMove : MonoBehaviour
{
    [SerializeField] private GameObject otherPlayer;
    [SerializeField] private float maxDistance = 4f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;

    public Vector3 externalForce;


    private Rigidbody rb;
    private Vector2 inputVector;

    private Vector3 movementVelocity;
    private Vector3 movementDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Rotate();
        Move();              
    }

    private void Rotate()
    {
        //rotate player in movement direction
        if (inputVector.magnitude > 0)
        {
            movementDirection = new Vector3(inputVector.x, 0, inputVector.y);
            transform.rotation = Quaternion.LookRotation(movementDirection);
        }
    }

    private void Move()
    {
        //push forward in movement direction
        if (inputVector.magnitude > 0)
        {
            movementVelocity = new Vector3(inputVector.x, 0, inputVector.y) * acceleration;
        }
        else
        {
            movementVelocity = Vector3.zero;
        }

        //decelerate if no input
        if (inputVector.magnitude == 0)
        {
            movementVelocity = Vector3.Lerp(movementVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        //clamp velocity to max speed
        movementVelocity = Vector3.ClampMagnitude(movementVelocity, maxSpeed);

        //add external force to velocity
        movementVelocity += externalForce;

        //if player at max distance from other player, apply force to move towards other player
        if (Vector3.Distance(transform.position, otherPlayer.transform.position) > maxDistance && inputVector.magnitude > 0)
        {
            Vector3 directionToOtherPlayer = (otherPlayer.transform.position - transform.position).normalized;
            movementDirection /= 2;

            //apply external force to other player to move towards this player
            otherPlayer.GetComponent<newPlayerMove>().externalForce = -directionToOtherPlayer * acceleration;
            
        }
        else
        {
            //remove external force from other player
            otherPlayer.GetComponent<newPlayerMove>().externalForce = Vector3.zero;
        }
        
        


        //apply velocity to rigidbody
        rb.velocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);        
    }



    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(otherPlayer.transform.position, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, otherPlayer.transform.position);

        Gizmos.color = Color.green;
        //max distance
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }



    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }



}
