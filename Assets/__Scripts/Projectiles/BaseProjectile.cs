using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] float StartVelocity = 15f;
    [SerializeField] float TimeToLive = 15f;
    private Rigidbody rb;
    
    protected GameObject Player;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        rb.velocity = transform.forward * StartVelocity;
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {        
        yield return new WaitForSeconds(TimeToLive);
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() => Destroy(gameObject));        
    }

    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.gameObject;
            OnPlayerHit();
        }
    }

    public abstract void OnPlayerHit();


}
