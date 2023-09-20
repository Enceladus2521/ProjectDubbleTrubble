using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartParams : MonoBehaviour
{
    [SerializeField] public float _height;
    [SerializeField] public float _width;



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(_width, _height, 0));
    }
}
