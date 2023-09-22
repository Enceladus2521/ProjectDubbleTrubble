using UnityEngine;

public abstract class IAttack : ScriptableObject
{
    [SerializeField] public EAttackTypes _attackType;
    [SerializeField] public float _cooldown = 1f;
    [SerializeField] public int _damage = 1;
}