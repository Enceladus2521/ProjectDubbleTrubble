using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocoNut : PlayerColectable
{
    [Header("Coconut")]
    [SerializeField] private float stunTime = 3f;
    
    [Range(1, 2)]
    [SerializeField] private float scaleSize = 2f;
    public override void Colect()
    {
        Debug.Log("Coconut collected");
        //stun player
        //StopCoroutine(player.GetComponent<PlayerMovement>().DisableInput(stunTime));
        //StartCoroutine(player.GetComponent<PlayerMovement>().DisableInput(stunTime));

        //scale player
        StopCoroutine(player.GetComponent<PlayerMovement>().ScalePlayerSeltsam(DelayedMunsch, scaleSize));
        StartCoroutine(player.GetComponent<PlayerMovement>().ScalePlayerSeltsam(DelayedMunsch, scaleSize));
        
    }

    
}
