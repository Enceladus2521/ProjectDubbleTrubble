using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    
    [SerializeField] private float _angle;
    [SerializeField] private Color _color;






    private void OnDrawGizmos() {
        Gizmos.color = _color;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angle, 0) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _angle, 0) * transform.forward * 5);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_angle, 0) * transform.forward * 5);
    }



}
