using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaColectable : PlayerColectable
{

    [Header("Stamina Colectable")]
    [SerializeField] private int staminaAmount = 10;
    
    public override void Colect()
    {
        player.GetComponent<PlayerMovement>().AddStamina(staminaAmount);
    }
}
