using UnityEngine;


[CreateAssetMenu(fileName = "SprayAttack", menuName = "Attacks/SprayAttack")]
public class SprayAttack : IAttack
{    
    [SerializeField] public int _projectileAmount = 1;
    [SerializeField] public float _projectileScaleSpeed = 1f;
    [SerializeField] public float _projectileAddedForce = 50f;
    [SerializeField] public float _spawnDistance = 1f;
    [SerializeField] public float _fireRate = 1f;
    [SerializeField] public float _projectileSpeed = 1f;
    [SerializeField] public int _projectileLife = 1;
    [SerializeField] public float _projectileSize = 1f;
    [SerializeField] public float _projectileSpread = 0f;
}