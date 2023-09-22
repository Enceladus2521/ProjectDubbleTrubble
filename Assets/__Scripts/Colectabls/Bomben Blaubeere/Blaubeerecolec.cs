using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaubeerecolec : PlayerColectable
{
    [Header("Blaubeere Settings")]
    [Range(1,3)]
    [SerializeField] private float ProcentualSpeedBoost = 1f;
    [SerializeField] private float SpeedTime = 1f;
    public override void Colect()
    {
        player.GetComponent<PlayerMovement>().AddBoost(ProcentualSpeedBoost,SpeedTime);
    }
}
