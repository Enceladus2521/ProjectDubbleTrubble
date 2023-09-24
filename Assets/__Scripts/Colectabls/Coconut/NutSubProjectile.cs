using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutSubProjectile : SubProjectile
{

    [Header("NutSubProjectile")]
    [SerializeField] private float stunTime = 3f;
    public override void OnPlayerHit()
    {
        //stun player
        StopCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(stunTime));
        StartCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(stunTime));

        StopAllCoroutines();
        Destroy(gameObject);
    }
}
