using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiShait : MonoBehaviour
{
   //Intstance this
    public static UiShait Instance;

    public GameObject Player1HealthBar;
    public GameObject Player2HealthBar;

    public GameObject Player1StaminaBar;
    public GameObject Player2StaminaBar;
    
    private void Awake()
    {
        Instance = this;
    }

    public void updateHealth(byte index, int currentHealth){
        if(index == 0)
        {
            updatePlayer1HealthBar(currentHealth);
        }
        else if(index == 1)
        {
            updatePlayer2HealthBar(currentHealth);
        }

    }

    public void updateStamina(byte index, float currentStamina){
        if(index == 0)
        {
            updatePlayer1StaminaBar(currentStamina);
        }
        else if(index == 1)
        {
            updatePlayer2StaminaBar(currentStamina);
        }
    }


    public void updatePlayer1HealthBar(int currentHealth)
    {
        Player1HealthBar.GetComponent<HealthBarController>().setHealth(currentHealth);
    }

    public void updatePlayer2HealthBar(int currentHealth)
    {
        Player2HealthBar.GetComponent<HealthBarController>().setHealth(currentHealth);
    }

    public void updatePlayer1StaminaBar(float currentStaminapercentage)
    {
        Player1StaminaBar.GetComponent<BoostMeter>().SetBoost(currentStaminapercentage);
    }

    public void updatePlayer2StaminaBar(float currentStaminapercentage)
    {
        Player2StaminaBar.GetComponent<BoostMeter>().SetBoost(currentStaminapercentage);
    }

}
