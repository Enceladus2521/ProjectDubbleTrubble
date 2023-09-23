using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBäre : PlayerColectable
{
    [Header("Heal Bäre")]
    [SerializeField] public bool addMaxHealth = false;
    [SerializeField] public int healAmount = 1;


    public override void Colect()
    {
        if (addMaxHealth) player.GetComponent<PlayerMovement>().FillHealth();
        
        else player.GetComponent<PlayerMovement>().Heal(healAmount);
    }
}
