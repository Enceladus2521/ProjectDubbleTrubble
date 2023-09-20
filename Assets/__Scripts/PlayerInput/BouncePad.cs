using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BouncePad : MonoBehaviour
{


    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private float moveDisableTime = 0.5f;


    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player hit by bounce pad");
            other.gameObject.GetComponent<PlayerMovement>().Bounce(transform, bounceForce, moveDisableTime);
        }
    }
}
