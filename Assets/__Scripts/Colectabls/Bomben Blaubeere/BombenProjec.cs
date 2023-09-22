using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class BombenProjec : BaseProjectile
{
    [Header("Bomben Settings")]
    [SerializeField] private float ExplosionRadius = 5f;
    [SerializeField] private float ExplosionForce = 1f;
    [SerializeField] private float TimePlayerCantMove = 1f;
    [SerializeField] private int ExplosionDamage = 1;
    [SerializeField] private float GazerExplosionForce = 500f;

    private int segments = 50;

    public override void OnPlayerHit()
    {
        
    }

   
    private void OnEnable()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
        CreatePoints(lineRenderer);

    }

    void CreatePoints(LineRenderer lineRenderer)
    {
        float x;
        float y = 0f;
        float z;

        
        float angle = 20f;
        for (int i = 0; i < segments + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * ExplosionRadius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * ExplosionRadius;

            lineRenderer.SetPosition(i,new Vector3(x,y,z));

            angle += (360f / segments);
        }
    }

    public override IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(TimeToLive);
        Explode();
        
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerMovement>().TakeDamage(ExplosionDamage);
                collider.GetComponent<PlayerMovement>().Bounce(transform, ExplosionForce,TimePlayerCantMove);
            }
            else if (collider.CompareTag("Gazer"))
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(GazerExplosionForce, transform.position, ExplosionRadius);
                
            }
        }
        Destroy(gameObject);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
