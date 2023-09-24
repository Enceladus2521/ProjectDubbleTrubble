using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    
    [SerializeField] public float _angle;
    [SerializeField] public Color _color;






    private void OnDrawGizmos() {
        Gizmos.color = _color;
        // Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angle, 0) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angle, 0) * transform.forward * 5);
    }
}
