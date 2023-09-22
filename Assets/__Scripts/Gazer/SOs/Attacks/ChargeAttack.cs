using UnityEngine;

[CreateAssetMenu(fileName = "ChargeAttack", menuName = "Attacks/ChargeAttack")]
public class ChargeAttack : IAttack
{
    [SerializeField] public float _chargeTime = 1f;
    [SerializeField] public float _chargeSpeed = 1f;
    [SerializeField] public int _chargesAmount = 1;
}
