using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazerController : MonoBehaviour
{
    
    [Header("Gazer")]
    [SerializeField] private float _defaultSpeed = 1f;
    [SerializeField] private float _defaultSize = 1f;
    [SerializeField] private GameObject _gazerPrefab;
    [SerializeField] private float _knockback = 500f;
    [SerializeField] private float _knockbackTime = 1f;
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _canAttack = true;


    [Header("Face")]
    [SerializeField] private GameObject _face;
    [SerializeField] private LookDirection _lookDirection = LookDirection.Right;
    [SerializeField] private float _moveRadius = 1f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] public FaceAnimator _faceAnimator;
    [SerializeField] private FaceAnim _angryFace;
    [SerializeField] private FaceAnim _obsessedFace;
    [SerializeField] private FaceAnim _evilGrinFace;
    [SerializeField] public FaceAnim _sadgeFace;

    [Header("TrailVFX")]
    [SerializeField] private UnityEngine.VFX.VisualEffect _trailEffect;
    [SerializeField] private bool _enabled = true;
    [SerializeField] private float _trailSpawnRate = 10f;
    [SerializeField] private float _trailSpeed = 1f;
    [SerializeField] private Vector2 _trailLifetime = new Vector2(1f, 1f);
    [SerializeField] private float _trailParticleSize = 1f;
    [SerializeField] private float _trailWidth = 1f;
    


    public static event Action<GameObject> OnChangeFocusedPlayer;

    public bool _faceOverwrite = false;


    private Rigidbody _rigidbody;
    private Transform _transform;
    private GazerSoundEffects _gazerSoundEffects;
    private float _startTime;
    private bool _isAttacking = false;
    private List<GameObject> _players;
    
    private GameObject _cameraMain;
    private Vector3 _cameraMainRight;
    private float _time;
    private Vector3 _faceMoveDirection;
    private List<IAttack> _currentAttackPool = new List<IAttack>();
    private IAttack _currentAttack;
    private int _currentDamage;
    private float _currentSpeed;
    private int _chargesLeft = 0;
    private bool _attackOver = true;
    private bool _needNewAttack = true;
    private Coroutine _moveProjectileCoroutine;
    private GameObject _focusedPlayer;


    private void OnValidate() {
        _trailEffect.SetBool("isSpawning", true);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _gazerSoundEffects = GetComponent<GazerSoundEffects>();
        _cameraMain = Camera.main.gameObject;
        _cameraMainRight = _cameraMain.transform.right;
        _startTime = Time.time;
        _time = Time.time;

        //set trail effect values
        _trailEffect.SetFloat("SpawnRate", _trailSpawnRate);
        _trailEffect.SetFloat("TrailSpeed", _trailSpeed);
        _trailEffect.SetVector2("TrailLifetime", _trailLifetime);
        _trailEffect.SetFloat("TrailScale", _trailParticleSize);
        _trailEffect.SetBool("isSpawning", _enabled);

        _currentSpeed = _defaultSpeed;
        _transform.localScale = new Vector3(_defaultSize, _defaultSize, _defaultSize);

        _faceAnimator = _face.GetComponentInChildren<FaceAnimator>();

        GameManager.OnPhaseChange += changeCurrentAttackPool;
    }

    private void OnDestroy() {
        GameManager.OnPhaseChange -= changeCurrentAttackPool;
    }


    private void Start() {
        //get random direction
        Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, UnityEngine.Random.Range(-1f,1f));
        direction.Normalize();

        if (!_canMove) {
            direction = Vector3.zero;
        }

        _rigidbody.velocity = direction * _currentSpeed;

        //get players
        _players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        _focusedPlayer = _players[0];

        StartCoroutine(getClosestPlayer());
    }


    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Bounds")){

            _gazerSoundEffects.gazerEnviHit();

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
        }
        else if (other.gameObject.CompareTag("Player")) {
            _gazerSoundEffects.gazerPlayerHit();
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage((int)_currentDamage);
            other.gameObject.GetComponent<PlayerMovement>().Bounce(transform, _knockback, _knockbackTime);
            //Debug.Log("Player hit by gazer");
        }

        if (_chargesLeft <= 0) {
            _isAttacking = false;
        }
        else {
            StartCoroutine(charge());
        }
    }


    private void Update() {
        if (_currentAttackPool.Count == 0 || _canAttack == false) {
        }
        else {
            if (_currentAttack == null && _isAttacking == false) {
                _currentAttack = _currentAttackPool[UnityEngine.Random.Range(1, _currentAttackPool.Count)];
                _currentDamage = _currentAttack._damage;
                _needNewAttack = false;
            }
            else if (_isAttacking == false && _needNewAttack == true) {
                _currentAttack = _currentAttackPool[UnityEngine.Random.Range(1, _currentAttackPool.Count)];
                _currentDamage = _currentAttack._damage;
                _needNewAttack = false;
            }
            else {
                //if the cooldown of the current attack is over, start the attack
                if (Time.time - _startTime > _currentAttack._cooldown) {
                    if (!_isAttacking && _attackOver) startAttack();
                }
            }
        }


        if (_isAttacking == true) {
            _faceAnimator._currentFaceAnim = _evilGrinFace;
        }
        else if (_faceOverwrite == false) {
            _faceAnimator._currentFaceAnim = _obsessedFace;
        }

        Vector3 gazerMoveDirection = _rigidbody.velocity;
        _trailEffect.SetVector3("TrailDirection", gazerMoveDirection * -1f);
        //trails trailwidth is a Vector3, which is placed at a 90 degree angle to the trail direction, with the float trailwidth as the width
        _trailEffect.SetVector3("TrailWidth", Vector3.Cross(gazerMoveDirection, Vector3.up).normalized * _trailWidth);

        //set trail spawnrate according to the speed of the gazer
        _trailEffect.SetFloat("SpawnRate", gazerMoveDirection.magnitude * _trailSpawnRate);


        if (_players.Count > 0) {

            //faceMoveDirection should be the direction from the gazer to the focused player
            Vector3 faceMoveDirection = _focusedPlayer.transform.position - transform.position;


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


    private void changeCurrentAttackPool(Phase newPhase) {
        _currentAttackPool = newPhase._attackPool;
        _currentSpeed = newPhase._speed;
        _transform.localScale = new Vector3(newPhase._size, newPhase._size, newPhase._size);
    }


    private void startAttack() {
        _isAttacking = true;
        _attackOver = false;

        switch (_currentAttack._attackType) {
            case EAttackTypes.charge:
                _chargesLeft = (_currentAttack as ChargeAttack)._chargesAmount;
                StartCoroutine(charge());
                break;
            case EAttackTypes.spray:
                StartCoroutine(spray());
                break;
            case EAttackTypes.shotgun:
                StartCoroutine(shotgun());
                break;
            default:
                Debug.LogError("Attack type not found");
                break;
        }
    }


    private IEnumerator charge() {
        ChargeAttack chargeAttack = _currentAttack as ChargeAttack;
        _chargesLeft--;
        _rigidbody.velocity = Vector3.zero;
        _gazerSoundEffects.gazerCharge();
        yield return new WaitForSeconds(chargeAttack._chargeTime);
        
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject player = players[UnityEngine.Random.Range(0, players.Count)];

        Vector3 direction = player.transform.position - _transform.position;
        direction.y = 0;
        direction.Normalize();
        _rigidbody.velocity = direction * chargeAttack._chargeSpeed;
        _startTime = Time.time;

        _attackOver = true;
        _needNewAttack = true;
    }


    private IEnumerator spray() {
        SprayAttack sprayAttack = _currentAttack as SprayAttack;
        for (int i = 0; i < sprayAttack._projectileAmount; i++) {

            GameObject projectile = Instantiate(_gazerPrefab, _transform.position + _faceMoveDirection * sprayAttack._spawnDistance, Quaternion.identity);
            CapsuleCollider boxCollider = projectile.GetComponent<CapsuleCollider>();
            boxCollider.enabled = false;
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            projectile.GetComponent<Rigidbody>().isKinematic = true;
            projectile.transform.SetParent(transform.parent);
            projectileController._life = sprayAttack._projectileLife;
            projectileController._currentSpeed = sprayAttack._projectileSpeed;
            projectileController._currentDamage = sprayAttack._damage;
            projectile.transform.localScale = Vector3.zero;

            if (_moveProjectileCoroutine != null) {
                StopCoroutine(_moveProjectileCoroutine);
                _moveProjectileCoroutine = null;
            }
            else {
                _moveProjectileCoroutine = StartCoroutine(moveProjectile(projectile, sprayAttack._spawnDistance));
            }

            //LeanTween the projectile, so it will grow from 0 to the given projectileSize
            _gazerSoundEffects.gazerSpawn();
            LeanTween.scale(projectile, Vector3.zero, 0.0000001f).setOnComplete(() => {
                LeanTween.scale(projectile, new Vector3(sprayAttack._projectileSize, sprayAttack._projectileSize, sprayAttack._projectileSize), sprayAttack._projectileScaleSpeed).setOnComplete(() => {
                    StopCoroutine(_moveProjectileCoroutine);
                    _moveProjectileCoroutine = null;
                    projectile.GetComponent<Rigidbody>().isKinematic = false;
                    projectile.GetComponent<Rigidbody>().AddForce(_faceMoveDirection * sprayAttack._projectileAddedForce, ForceMode.Impulse);
                    boxCollider.enabled = true;
                });
            });
            
            // Debug.Log("FireRate: " + sprayAttack._fireRate);
            yield return new WaitForSeconds(sprayAttack._fireRate);
        }
        _startTime = Time.time;
        _attackOver = true;
        _isAttacking = false;
        _needNewAttack = true;
    }


    private IEnumerator shotgun() {
        ShotgunAttack shotgunAttack = _currentAttack as ShotgunAttack;
        int spawnedBullets = 0;
        List<GameObject> bullets = new List<GameObject>();

        GameObject emptyForPerformance = new GameObject();
        emptyForPerformance.transform.SetParent(transform);
        emptyForPerformance.transform.localPosition = _transform.position + _faceMoveDirection * shotgunAttack._spawnDistance;

        if (_moveProjectileCoroutine != null) {
                StopCoroutine(_moveProjectileCoroutine);
                _moveProjectileCoroutine = null;
        }
        else {
            _moveProjectileCoroutine = StartCoroutine(moveProjectile(emptyForPerformance, shotgunAttack._spawnDistance));
        }


        for (int i = 0; i < shotgunAttack._bulletLeftAndRight; i++) {
            // Vector3 direction2 = Quaternion.Euler(0, shotgunAttack._bulletSpread, 0) * _faceMoveDirection;
            Vector3 direction2 = Quaternion.Euler(0, shotgunAttack._bulletSpread * (i + 1), 0) * _faceMoveDirection;
            bullets.Add(Instantiate(_gazerPrefab, _transform.position + direction2 * shotgunAttack._spawnDistance, Quaternion.identity));
            bullets[spawnedBullets].GetComponent<CapsuleCollider>().enabled = false;
            ProjectileController projectileController1 = bullets[spawnedBullets].GetComponent<ProjectileController>();
            bullets[spawnedBullets].transform.SetParent(emptyForPerformance.transform);
            bullets[spawnedBullets].transform.localScale = new Vector3(shotgunAttack._bulletSize, shotgunAttack._bulletSize, shotgunAttack._bulletSize);
            bullets[spawnedBullets].GetComponent<Rigidbody>().isKinematic = true;
            projectileController1._life = shotgunAttack._bulletLife;
            projectileController1._currentSpeed = shotgunAttack._bulletSpeed;
            projectileController1._currentDamage = shotgunAttack._damage;
            bullets[spawnedBullets].transform.localScale = Vector3.zero;
            spawnedBullets++;
        }

        //same for the middle one
        bullets.Add(Instantiate(_gazerPrefab, _transform.position + _faceMoveDirection * shotgunAttack._spawnDistance, Quaternion.identity));
        bullets[spawnedBullets].GetComponent<CapsuleCollider>().enabled = false;
        ProjectileController projectileController3 = bullets[spawnedBullets].GetComponent<ProjectileController>();
        bullets[spawnedBullets].transform.SetParent(emptyForPerformance.transform);
        bullets[spawnedBullets].transform.localScale = new Vector3(shotgunAttack._bulletSize, shotgunAttack._bulletSize, shotgunAttack._bulletSize);
        bullets[spawnedBullets].GetComponent<Rigidbody>().isKinematic = true;
        projectileController3._life = shotgunAttack._bulletLife;
        projectileController3._currentSpeed = shotgunAttack._bulletSpeed;
        projectileController3._currentDamage = shotgunAttack._damage;
        bullets[spawnedBullets].transform.localScale = Vector3.zero;
        spawnedBullets++;


        //same for the other side
        for (int i = 0; i < shotgunAttack._bulletLeftAndRight; i++) {
            // Vector3 direction3 = Quaternion.Euler(0, -shotgunAttack._bulletSpread, 0) * _faceMoveDirection;
            Vector3 direction3 = Quaternion.Euler(0, -shotgunAttack._bulletSpread * (i + 1), 0) * _faceMoveDirection;
            bullets.Add(Instantiate(_gazerPrefab, _transform.position + direction3 * shotgunAttack._spawnDistance, Quaternion.identity));
            bullets[spawnedBullets].GetComponent<CapsuleCollider>().enabled = false;
            ProjectileController projectileController2 = bullets[spawnedBullets].GetComponent<ProjectileController>();
            bullets[spawnedBullets].transform.SetParent(emptyForPerformance.transform);
            bullets[spawnedBullets].transform.localScale = new Vector3(shotgunAttack._bulletSize, shotgunAttack._bulletSize, shotgunAttack._bulletSize);
            bullets[spawnedBullets].GetComponent<Rigidbody>().isKinematic = true;
            projectileController2._life = shotgunAttack._bulletLife;
            projectileController2._currentSpeed = shotgunAttack._bulletSpeed;
            projectileController2._currentDamage = shotgunAttack._damage;
            bullets[spawnedBullets].transform.localScale = Vector3.zero;
            spawnedBullets++;
        }


        //Loop through all the projectiles, ad LeanTween to them, so they will grow from 0 to the given projectileSize
        //add no force to them
        //the next projectiles should start it's lean tween only if the one is finished
        foreach (GameObject bullet in bullets)
        {
            _gazerSoundEffects.gazerSpawn();
            LeanTween.scale(bullet, Vector3.zero, 0.0000001f).setOnComplete(() => {
                LeanTween.scale(bullet, new Vector3(shotgunAttack._bulletSize, shotgunAttack._bulletSize, shotgunAttack._bulletSize), shotgunAttack._bulletScaleSpeed);
            });

            while (bullet.transform.localScale.x < shotgunAttack._bulletSize) {
                if(bullet == null) {
                    break;
                }
                yield return null;
            }
        }


        foreach (GameObject bullet in bullets)
        {
            bullet.transform.SetParent(transform.parent);
            bullet.GetComponent<Rigidbody>().isKinematic = false;
            bullet.GetComponent<Rigidbody>().AddForce(_faceMoveDirection * shotgunAttack._bulletAddedForce, ForceMode.Impulse);
            bullet.GetComponent<CapsuleCollider>().enabled = true;
            yield return new WaitForSeconds(shotgunAttack._fireRate);
        }
        StopCoroutine(_moveProjectileCoroutine);
        _moveProjectileCoroutine = null;


        _startTime = Time.time;
        _attackOver = true;
        _isAttacking = false;
        _needNewAttack = true;
    }



    private IEnumerator moveProjectile(GameObject obj, float distance) {

        while (true) {
            obj.transform.position = _transform.position + _faceMoveDirection * distance;
            obj.transform.rotation = Quaternion.LookRotation(_faceMoveDirection);
            yield return null;
        }
    }

    private IEnumerator getClosestPlayer() {
        GameObject playerTemp;
        playerTemp = _players[0];
        while (true) {
            //get closest player
            float closestDistance = Mathf.Infinity;
            foreach (GameObject player in _players) {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    playerTemp = player;
                }
            }
            if (closestDistance < 5f) {
                // _gazerSoundEffects.gazerCloseCall();
            }
            _focusedPlayer = playerTemp;
            OnChangeFocusedPlayer?.Invoke(_focusedPlayer);

            yield return null;
        }
    }
}


[System.Serializable]
public enum LookDirection {
    Left = 180,
    Right = 0
}