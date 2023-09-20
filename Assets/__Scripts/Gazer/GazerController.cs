using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GazerController : MonoBehaviour
{
    
    [Header("Gazer Settings")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _growthRate = 1f;
    [SerializeField] private float _minSize = 1f;
    [SerializeField] private float _maxSize = 1f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private bool _canCharge = true;
    [SerializeField] private float _chargeSpeed = 1f;
    [SerializeField] private float _chargeTime = 1f;
    [SerializeField] private float _chargeCooldown = 1f;


    [Header("Face")]
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _eye1;
    [SerializeField] private GameObject _eye2;
    [SerializeField] private LookDirection _lookDirection = LookDirection.Right;
    [SerializeField] private float _moveRadius = 1f;
    [SerializeField] private float _moveSpeed = 1f;
    



    private Rigidbody _rigidbody;
    private Transform _transform;
    private float _startTime;
    private bool _charging = false;
    private List<GameObject> _players;
    
    private GameObject _cameraMain;
    private Vector3 _cameraMainRight;
    private float _time;
    private Vector3 _faceMoveDirection;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _cameraMain = Camera.main.gameObject;
        _cameraMainRight = _cameraMain.transform.right;
        _startTime = Time.time;
        _time = Time.time;
    }


    private void Start() {
        //get random direction
        Vector3 direction = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        direction.Normalize();
        _rigidbody.velocity = direction * _speed;

        //get players
        _players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
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
        if (Time.time - _startTime > _chargeCooldown && !_charging && _canCharge) {
            _charging = true;
            StartCoroutine(charge());
        }

        //rotate eye1 in direction of the player
        if (_players.Count > 0) {
            //eye1
            Vector3 direction1 = _players[0].transform.position - _eye1.transform.position;
            direction1.y = 0;
            direction1.Normalize();
            _eye1.transform.rotation = Quaternion.LookRotation(direction1);

            //eye2
            Vector3 direction2 = _players[0].transform.position - _eye2.transform.position;
            direction2.y = 0;
            direction2.Normalize();
            _eye2.transform.rotation = Quaternion.LookRotation(direction2);



            Vector3 faceMoveDirection = (direction1 + direction2) / 2;

            //make face follow the player by the average of the two eyes
            //make sure the face is at max distance of _moveRadius
            if (faceMoveDirection.magnitude > _moveRadius) {
                faceMoveDirection.y = _face.transform.localPosition.y;
                faceMoveDirection.Normalize();
                faceMoveDirection.x *= _moveRadius;
                faceMoveDirection.z *= _moveRadius;
            }

            //lerp to the new position
            _faceMoveDirection = faceMoveDirection;
            _face.transform.localPosition = Vector3.Lerp(_face.transform.localPosition, faceMoveDirection, Time.deltaTime * _moveSpeed);

            //flip the face around z axis if the _face is on the left side of the gazer
            if (_face.transform.localPosition.x < 0) {
                _face.transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
            else {
                _face.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
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



    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _moveRadius);

        //draw blue sphere of capsule collider
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + _cameraMainRight * 0.5f, 0.5f);

        //draw ray from gazer to current _face position, but dont use _faceMoveDirection because the face lerps to it, so it wont be the current position
        //make the ray a little longer, so it is visible
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, _face.transform.localPosition * 30f);
    }

}

[System.Serializable]
public enum LookDirection {
    Left = 180,
    Right = 0
}