using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpukBareBäre : BaseProjectile
{
    [Header("SpukBareBäre")]
    [Tooltip("Time in seconds the player can't move after being hit")]
    [SerializeField] private float TimeInputIsDisabled = 0.5f;
    public override void OnPlayerHit()
    {
        //StopCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(TimeInputIsDisabled));
        StartCoroutine(Player.GetComponent<PlayerMovement>().DisableInput(TimeInputIsDisabled));      
    }

    
}
