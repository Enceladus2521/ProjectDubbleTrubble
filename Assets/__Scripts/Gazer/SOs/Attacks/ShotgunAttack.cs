using UnityEngine;




[CreateAssetMenu(fileName = "ShotgunAttack", menuName = "Attacks/ShotgunAttack")]
public class ShotgunAttack : IAttack
{
    [SerializeField] public int _bulletLeftAndRight = 1;
    [SerializeField] public float _bulletScaleSpeed = 1f;
    [SerializeField] public float _bulletAddedForce = 50f;
    [SerializeField] public float _spawnDistance = 1f;
    [SerializeField] public float _bulletSpread = 10f;
    [SerializeField] public float _bulletSpeed = 1f;
    [SerializeField] public int _bulletLife = 1;
    [SerializeField] public float _bulletSize = 1f;
    [SerializeField] public float _fireRate = 1f;
}