using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class SubProjectile : MonoBehaviour
{
    private float StartVelocity = 15f;
    private float TimeToLive = 15f;

    private Rigidbody rb;
    protected GameObject Player;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player = other.gameObject;
            OnPlayerHit();
        }
    }

    public void SetVars(float startVelocity, float timeToLive)
    {
        StartVelocity = startVelocity;
        TimeToLive = timeToLive;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * StartVelocity;
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(TimeToLive);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => Destroy(gameObject));
    }

    public abstract void OnPlayerHit();

}
