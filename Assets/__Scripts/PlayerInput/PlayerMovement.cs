using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public byte playerIndex = 0;

    #region Variables

    #region References
    [SerializeField] private PlayerMoveStats moveStats;
    private Rigidbody rb;

    [SerializeField] public GameObject Munscher;
    #endregion

    #region dynamic variables
    private Vector2 moveInput;
    private Vector3 moveVelocity;


    private float timeSinceLastMoveInput;
    public float timeDifference;
    #endregion

    #region Stats
    [Header("DebugStats")]
    public bool inputEnabled = true;
    public int currentHealth;
    public float currentStamina;
    public float currentMaxStamina;

    public float currentMaxSpeed;

    

    #endregion

    #endregion


    private void Start()
    {
        if (moveStats == null) Debug.LogError("PlayerMoveStats is null on PlayerMovement");
        rb = GetComponent<Rigidbody>();
        currentHealth = moveStats.MaxHealth;
        currentStamina = moveStats.MaxStamina;
        currentMaxStamina = moveStats.MaxStamina;
        currentMaxSpeed = moveStats.maxMoveSpeed;

        UiShait.Instance.updateHealth(playerIndex, currentHealth);


    }



    private void Update()
    {
        if(UiShait.Instance == null) Debug.LogWarning("UiShait is null on PlayerMovement");
        else UiShait.Instance.updateStamina(playerIndex, currentStamina / currentMaxStamina);        

        if (moveInput == Vector2.zero)
        {
            timeDifference = Time.time - timeSinceLastMoveInput;
            //set time since last move input to this time
            if (timeSinceLastMoveInput == 0) timeSinceLastMoveInput = Time.time;

            //decrease move velocity
            decreaseMoveVelocity();

            if (timeDifference > moveStats.StaminaRegenDelay && currentStamina < currentMaxStamina)
            {
                //regenerate stamina
                currentStamina += moveStats.StaminaRegenSpeed * Time.deltaTime;
                if (currentStamina > currentMaxStamina) currentStamina = currentMaxStamina;
            }

        }
        else
        {
            //set time since last move input to 0
            timeSinceLastMoveInput = 0;
            timeDifference = 0;

            //drain stamina
            currentStamina -= moveStats.StaminaDrainSpeed * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;

            Vector3 newMoveInput = new Vector3(moveInput.x, 0, moveInput.y);
            if (currentStamina > 0)
            {
                //accelerate to max speed using acceleration curve and acceleration time and time.deltaTime
                moveVelocity = Vector3.Lerp(moveVelocity, newMoveInput * currentMaxSpeed, moveStats.accelerationCurve.Evaluate(Time.deltaTime / moveStats.accelerationTime));
                //ToDo: Add custom max speed for each player
            }
            else
            {
                decreaseMoveVelocity();
            }


            //rotate in velocity direction using max turn speed and time.deltaTime
            if (moveVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVelocity), moveStats.maxTurnSpeed * Time.deltaTime);
            }
        }

    }


    private void decreaseMoveVelocity()
    {
        //decelerate to 0 using deceleration curve and deceleration time and time.deltaTime
        moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, moveStats.decelerationCurve.Evaluate(Time.deltaTime / moveStats.decelerationTime));
    }




    private void FixedUpdate()
    {
        if (!inputEnabled) return;
        rb.velocity = moveVelocity;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //draw a ray from the player in the direction of moveVelocity with a length of stamina
        Gizmos.DrawRay(transform.position, moveVelocity * (currentStamina / moveStats.MaxStamina));


        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, new Vector3(moveInput.x, 0, moveInput.y) * 2);
    }







    #region public functions

    private bool invincible = false;
    public void TakeDamage(int damage)
    {
        if (invincible) return;
        currentHealth -= damage;
        //update healthbar
        UiShait.Instance.updateHealth(playerIndex, currentHealth);
        if (currentHealth <= 0) Debug.Log("Player died");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            StartCoroutine(player.GetComponent<PlayerMovement>().Invincibility());
        }
    }

    public IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(moveStats.invincibilityTime);
        invincible = false;        
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > moveStats.MaxHealth) currentHealth = moveStats.MaxHealth;
        //update healthbar
        UiShait.Instance.updateHealth(playerIndex, currentHealth);
    }



    public void FillHealth()
    {
        currentHealth = moveStats.MaxHealth;
        //update healthbar
        UiShait.Instance.updateHealth(playerIndex, currentHealth);
    }

    public void EmptyStamina()
    {
        currentStamina = 0;

    }

    public void FillStamina()
    {
        currentStamina = moveStats.MaxStamina;

    }
    public void AddStamina(int amount)
    {

        currentStamina += amount;
        if (currentStamina > moveStats.MaxStamina) currentMaxStamina = currentStamina + amount;

        StopCoroutine(ResetMaxStaminaToNormal());
        StartCoroutine(ResetMaxStaminaToNormal());
    }

    IEnumerator ResetMaxStaminaToNormal()
    {
        while (currentMaxStamina > moveStats.MaxStamina)
        {
            currentMaxStamina -= moveStats.StaminaDrainReturnSpeed * Time.deltaTime;
            if (currentStamina > currentMaxStamina) currentStamina = currentMaxStamina;
            yield return null;
        }
        currentMaxStamina = moveStats.MaxStamina;
    }

    public void Bounce(Transform origen, float bounceForce, float moveDisableTime)
    {
        StartCoroutine(DisableInput(moveDisableTime));
        rb.AddForce((transform.position - origen.position).normalized * bounceForce, ForceMode.Impulse);
    }

    public IEnumerator DisableInput(float time)
    {
        inputEnabled = false;
        moveVelocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        inputEnabled = true;
    }

    public void AddBoost(float ProcentualSpeedBoost, float boostTime)
    {
        StopCoroutine(Boost());
        StartCoroutine(Boost(ProcentualSpeedBoost, boostTime));
    }

    IEnumerator Boost(float boostAmount = 1f, float boostTime = 1f)
    {
        currentMaxSpeed = moveStats.maxMoveSpeed * boostAmount;
        yield return new WaitForSeconds(boostTime);
        currentMaxSpeed = moveStats.maxMoveSpeed;
    }











    #endregion










    #region  input

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    #endregion

}


#if UNITY_EDITOR
[CustomEditor(typeof(PlayerMovement))]

public class PlayerMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {


        PlayerMovement playerMovement = (PlayerMovement)target;

        if (GUILayout.Button("Empty Stamina"))
        {
            playerMovement.EmptyStamina();
        }

        if (GUILayout.Button("Fill Stamina"))
        {
            playerMovement.FillStamina();
        }

        if (GUILayout.Button("Take Damage"))
        {
            playerMovement.TakeDamage(1);
        }

        if (GUILayout.Button("Heal"))
        {
            playerMovement.Heal(1);
        }

        base.OnInspectorGUI();
    }
}

#endif



