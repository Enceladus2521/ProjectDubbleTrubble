using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazerController : MonoBehaviour
{
    
    
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _growthRate = 1f;
    [SerializeField] private float _minSize = 1f;
    [SerializeField] private float _maxSize = 1f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _chargeSpeed = 1f;
    [SerializeField] private float _chargeTime = 1f;
    [SerializeField] private float _chargeCooldown = 1f;

    



    private Rigidbody _rigidbody;
    private Transform _transform;
    private float _startTime;
    private bool _charging = false;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _startTime = Time.time;
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

            //add +-10 degrees to current direction
            Vector3 direction = _rigidbody.velocity;
            direction = Quaternion.Euler(0, Random.Range(-10f,10f), 0) * direction;
            direction.Normalize();
            


            if (direction.z < 0.1f && direction.z > 0f) {
                direction.z = 0.1f;
            }
            else if (direction.z < 0f && direction.z > -0.1f) {
                direction.z = -0.1f;
            }

            if (direction.x < 0.1f && direction.x > 0f) {
                direction.x = 0.1f;
            }
            else if (direction.x < 0f && direction.x > -0.1f) {
                direction.x = -0.1f;
            }
            _rigidbody.velocity = direction * _speed;
        }
        else if (other.gameObject.CompareTag("Player")) {
            //TODO: damage player
            //other.gameObject.GetComponent<PlayerController>().TakeDamage(_damage);
        }
    }

    private void Update() {
        if (Time.time - _startTime > _chargeCooldown && !_charging) {
            _charging = true;
            StartCoroutine(charge());
        }
    }


    private IEnumerator charge() {
        _rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(_chargeTime);
        
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject player = players[Random.Range(0, players.Count)];

        Vector3 direction = player.transform.position - _transform.position;
        direction.y = 0;
        direction.Normalize();
        _rigidbody.velocity = direction * _chargeSpeed;
        _startTime = Time.time;
        _charging = false;
    }


    
}