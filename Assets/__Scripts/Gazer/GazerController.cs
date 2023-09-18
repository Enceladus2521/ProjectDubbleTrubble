using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazerController : MonoBehaviour
{
    
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _growthRate = 1f;
    [SerializeField] private float _maxSize = 1f;
    [SerializeField] private float _damage = 1f;


    private Rigidbody _rigidbody;
    private Transform _transform;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }


    private void Start() {
        //get random direction
        Vector3 direction = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        direction.Normalize();
        _rigidbody.velocity = direction * _speed;
    }


    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Bounds")){
            if (_transform.localScale.x < _maxSize) {
                _transform.localScale *= _growthRate;
            }
        }
        else if (other.gameObject.CompareTag("Player")) {
            //TODO: damage player
            //other.gameObject.GetComponent<PlayerController>().TakeDamage(_damage);
        }
    }
}