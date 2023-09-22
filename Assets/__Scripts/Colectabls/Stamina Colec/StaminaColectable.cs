using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StaminaColectable : PlayerColectable
{

    [Header("Stamina Colectable")]
    [SerializeField] public bool addMaxStamina = false;
    [SerializeField] public int staminaAmount = 10;
    
    
    public override void Colect()
    {
        if(addMaxStamina) player.GetComponent<PlayerMovement>().FillStamina();        
        else player.GetComponent<PlayerMovement>().AddStamina(staminaAmount);
    }
}


 