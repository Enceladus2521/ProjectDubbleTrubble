using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PlayerMoveStats", menuName = "")]
//cange script icon

public class PlayerMoveStats : ScriptableObject
{
    
    [Header("Movement")]
    [Tooltip("The maximum speed the player can move at.")]
    public float maxMoveSpeed = 10f;

    [Tooltip("The Acceleration curve for the player speed increase.")]
    public AnimationCurve accelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Tooltip("The time it takes for the player to reach max speed.")]
    public float accelerationTime = 1f;
    
    [Tooltip("The Deceleration curve for the player speed decrease.")]
    public AnimationCurve decelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Tooltip("The time it takes for the player to reach 0 speed.")]
    public float decelerationTime = 1f;
    [Tooltip("The maximum speed the player can turn at.")]
    public float maxTurnSpeed = 10f;






    [Header("Health")]
    [Tooltip("The maximum health the player can have.")]
    public int MaxHealth = 3;

    [Tooltip("The time the player is invincible after taking damage.")]
    public float invincibilityTime = 1f;




    [Header("Stamina")]
    
    [Tooltip("The maximum stamina the player can have.")]
    public int MaxStamina = 100;

    [Tooltip("The rate at which stamina regenerates.")]
    public float StaminaRegenSpeed = 1f;    

    [Tooltip("The delay before stamina starts regenerating.")]
    public float StaminaRegenDelay = 1f;



    [Tooltip("The rate at which stamina drains.")]
    public float StaminaDrainSpeed = 1f;

    [Tooltip("The speed at which the stamina returns to normal maxAmount.")]
    public float StaminaDrainReturnSpeed = 5f;

    







   

    




    

   
}

