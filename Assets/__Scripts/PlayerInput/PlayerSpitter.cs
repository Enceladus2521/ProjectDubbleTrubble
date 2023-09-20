using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpitter : MonoBehaviour
{
    [SerializeField] Transform projectileSpawnPoint;

    public void SpawnProjectile(GameObject projectilePrefab)
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }
}
