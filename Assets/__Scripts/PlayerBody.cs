using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player1.transform.position, 0.5f);
        Gizmos.DrawWireSphere(player2.transform.position, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(player1.transform.position, player2.transform.position);

    }
}
