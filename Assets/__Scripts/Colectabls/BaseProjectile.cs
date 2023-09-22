using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] float StartVelocity = 15f;
    [Tooltip("Dont use this!!!!!!")]
    [SerializeField] float SpawnDistance = 0f;
    [SerializeField] protected float TimeToLive = 15f;
    [SerializeField] float spreadAngle = 0f;

    [Header("Subprojectiles")]
    [SerializeField] int subProjectiles = 0;
    [SerializeField] GameObject subProjectilePrefab;

    
    private Rigidbody rb;
    
    protected GameObject Player;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
        
       
    }
    
    private void Start()
    {
        if(SpawnDistance > 0)transform.position += transform.forward * SpawnDistance;
        
        if(subProjectiles > 0 && subProjectilePrefab != null) //if subprojectiles are set, shoot them instead
        {
            
            float startAngle = -spreadAngle;
            float angleStep = spreadAngle * 2 / subProjectiles;
            for (int i = 0; i < subProjectiles; i++)
            {
                GameObject subProjectile = Instantiate(subProjectilePrefab, transform.position, Quaternion.identity);
                subProjectile.transform.forward = Quaternion.Euler(0, startAngle + angleStep * i, 0) * transform.forward;
                subProjectile.GetComponent<SubProjectile>().SetVars(StartVelocity, TimeToLive);
            }           


            Destroy(gameObject);
        }        
        else //if no subprojectiles are set, just shoot forward with a random angle
        {            
            Vector3 randomforwardangle = Quaternion.Euler(0, Random.Range(-spreadAngle, spreadAngle), 0) * transform.forward;
            rb.velocity = randomforwardangle * StartVelocity;
        }
        
        StartCoroutine(DestroyAfterTime());
    }

    public virtual IEnumerator DestroyAfterTime()
    {        
        yield return new WaitForSeconds(TimeToLive);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => Destroy(gameObject));        
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
