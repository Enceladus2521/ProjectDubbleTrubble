using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErdberSubProjectile : SubProjectile
{
    [Header("Erdber Sub Projectile")]
    [SerializeField] float stunTime = 1;
    
    public override void OnPlayerHit()
    {
        //stun player
        StopCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(stunTime));
        StartCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(stunTime));
        
    }
}

