using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutProjectile : BaseProjectile
{
    [Header("Coconut")]
    [SerializeField] private float ScaleTime = 3f;
    [Range(1, 2)]
    [SerializeField] private float scaleSize = 2f;


    
    public override void OnPlayerHit()
    {
        
    }


    public override void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //find closest player
        float closestDistance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                Player = player;
            }
        }

        transform.SetParent(Player.transform);

        //Scale player
        StopCoroutine(Player.GetComponent<PlayerMovement>().ScalePlayerSeltsam(ScaleTime, scaleSize));
        StartCoroutine(Player.GetComponent<PlayerMovement>().ScalePlayerSeltsam(ScaleTime, scaleSize));

        StartCoroutine(SpawnSubProjectiles()); 

        StartCoroutine(DestroyAfterTime());    

        
    }

    IEnumerator SpawnSubProjectiles()
    {
        if(subProjectiles > 0 && subProjectilePrefab != null) //if subprojectiles are set, shoot them instead
        {
            
            float startAngle = -spreadAngle;
            float angleStep = spreadAngle * 2 / subProjectiles;
            for (int i = 0; i < subProjectiles; i++)
            {
                GameObject subProjectile = Instantiate(subProjectilePrefab, transform.position, Quaternion.identity);
                subProjectile.transform.forward = Quaternion.Euler(0, startAngle + angleStep * i, 0) * transform.forward;
                subProjectile.GetComponent<SubProjectile>().SetVars(StartVelocity, TimeToLive);
                yield return null;
            }                
        }     
        Destroy(gameObject);   
        
    }



    


}