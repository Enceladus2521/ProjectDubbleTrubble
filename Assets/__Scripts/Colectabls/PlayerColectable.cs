using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(SphereCollider))]
public abstract class PlayerColectable : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float pickupDistance = 2.5f;
    [SerializeField] private bool Muschable = false;
    [SerializeField] private GameObject MunschPrefab;
    protected GameObject player;
    

   
    private void OnValidate()
    {
        if (pickupDistance < 1) pickupDistance = 1;

        GetComponent<SphereCollider>().radius = pickupDistance;
        GetComponent<SphereCollider>().isTrigger = true;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player hit by collectable");
            player = other.gameObject;
            StartCoroutine(moveToPlayer());            
        }
    }

    public abstract void Colect();

    private void Munscher()
    {
        if (Muschable)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.Munscher.GetComponent<Munsch>().EatableMunsch(playerMovement.playerIndex, MunschPrefab);

        }
    }
 
        
    

    IEnumerator moveToPlayer()
    {
        while (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Colect();
        Munscher();
        
        Destroy(gameObject);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
