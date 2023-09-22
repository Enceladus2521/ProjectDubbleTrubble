using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [Header("Gazer")]
    [SerializeField] private float _defaultSpeed = 1f;
    [SerializeField] public int _life;

    [Header("Face")]
    [SerializeField] private GameObject _face;
    [SerializeField] private GameObject _eye1;
    [SerializeField] private GameObject _eye2;
    [SerializeField] private LookDirection _lookDirection = LookDirection.Right;
    [SerializeField] private float _moveRadius = 1f;
    [SerializeField] private float _moveSpeed = 1f;

    [Header("TrailVFX")]
    [SerializeField] private UnityEngine.VFX.VisualEffect _trailEffect;
    [SerializeField] private bool _enabled = true;
    [SerializeField] private float _trailSpawnRate = 10f;
    [SerializeField] private float _trailSpeed = 1f;
    [SerializeField] private Vector2 _trailLifetime = new Vector2(1f, 1f);
    [SerializeField] private float _trailParticleSize = 1f;
    [SerializeField] private float _trailWidth = 1f;
    



    private Rigidbody _rigidbody;
    private Transform _transform;
    private List<GameObject> _players;
    
    private GameObject _cameraMain;
    private Vector3 _cameraMainRight;
    private float _time;
    private Vector3 _faceMoveDirection;
    private List<IAttack> _currentAttackPool = new List<IAttack>();
    private IAttack _currentAttack;
    private int _currentDamage;
    public float _currentSpeed;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _cameraMain = Camera.main.gameObject;
        _cameraMainRight = _cameraMain.transform.right;
        _time = Time.time;

        //set trail effect values
        _trailEffect.SetFloat("SpawnRate", _trailSpawnRate);
        _trailEffect.SetFloat("TrailSpeed", _trailSpeed);
        _trailEffect.SetVector2("TrailLifetime", _trailLifetime);
        _trailEffect.SetFloat("TrailScale", _trailParticleSize);
        _trailEffect.SetBool("isSpawning", _enabled);

        _currentSpeed = _defaultSpeed;
    }


    private void Start() {
        _players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }


    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Bounds")){

            //add +-10 degrees to current direction
            Vector3 direction = _rigidbody.velocity;
            direction = Quaternion.Euler(0, UnityEngine.Random.Range(-10f,10f), 0) * direction;
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
            _rigidbody.velocity = direction * _currentSpeed;

            _life--;
            if (_life <= 0) {
                Destroy(gameObject);
            }

        }
        else if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage((int)_currentDamage);
            Debug.Log("Player hit by gazer");
            LeanTween.cancel(gameObject);
            Destroy(gameObject);
        }
    }


    private void Update() {

        Vector3 gazerMoveDirection = _rigidbody.velocity;
        _trailEffect.SetVector3("TrailDirection", gazerMoveDirection * -1f);
        //trails trailwidth is a Vector3, which is placed at a 90 degree angle to the trail direction, with the float trailwidth as the width
        _trailEffect.SetVector3("TrailWidth", Vector3.Cross(gazerMoveDirection, Vector3.up).normalized * _trailWidth);

        //set trail spawnrate according to the speed of the gazer
        _trailEffect.SetFloat("SpawnRate", gazerMoveDirection.magnitude * _trailSpawnRate);


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

}
