using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance;

    [SerializeField] private bool canDie = true;
    [HideInInspector] public float health = 200f;
    [SerializeField] public float maxHealth = 200f;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight = 2f;
    private float camAxisX = 0f;
    private float camAxisY = 0f;
    private Quaternion camRotation;
    [SerializeField] private bool invertYAxis = true;
    public float sensMultiplier = 3f;
    [SerializeField] private float runningMultiplier = 1f;
    private bool running;
    [SerializeField] private float footstepInterval = 0.4f;
    [SerializeField] private GameObject camPivot;
    [SerializeField] private CharacterController ccPlayer;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform headCheck; // para hacer un physics check para cuando golpea con la cabeza algo anular fuerza vertical a 0...
    [SerializeField] private float headHitDistance = 0.25f;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float stepOffset = 0.5f;
    [SerializeField] private bool enablePlayerHud = true;

    private PlayAudio playAudio;

    private float timerOnPoisoned = 0;
    private float timerOnSlowed = 0;
    private float timeWhileIsPoisoned = 0;
    private float timeWhileIsSlowed = 0;
    private float poisonDamageToTake = 0;
    private float speedReduction = 0;

    private Vector3 lastPosition;
    private bool moving;
    private bool isGrounded;

    private bool isPoisoned = false;
    private bool isSlowed = false;

    private Vector3 velocity;

    private void Awake()
    {
        Instance = this;

        lastPosition = gameObject.transform.position;
    }
    private void Start()
    {
        playAudio = GetComponent<PlayAudio>();
        StartCoroutine(Footsteps());

        PlayerHUD_Manager.Instance.HealthBarInit(maxHealth);
    }
    private void Update()
    {
        PlayerHUD_Manager.Instance.UpdateHealthBar(health);

        if (Input.GetKeyDown(KeyCode.M)) health -= 20; // test..

        Movement(movementSpeed);


        if (!GameManager.Instance.gameIsPaused)
        {
            MRotation(); // esto no se ejecuta mientras el juego esta en pausa.

            if (isPoisoned)
            {
                timerOnPoisoned += Time.deltaTime;
                if (timerOnPoisoned <= timeWhileIsPoisoned)
                {
                    health -= poisonDamageToTake;
                }
                else
                {
                    timerOnPoisoned = 0;
                    isPoisoned = false;
                }
            }
            if (isSlowed)
            {
                timerOnSlowed += Time.deltaTime;
                if (timerOnSlowed > timeWhileIsSlowed)
                {
                    speedReduction = 0;
                    timerOnSlowed = 0;
                    isSlowed = false;
                }
            }
        }
        Gravity();

        if (canDie) PlayerDeath();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spider" || other.tag == "Enemy")
        {
            EnemyHandler enemy;
            enemy = other.GetComponentInParent<EnemyHandler>();
            health -= enemy.damage;
            // Debug.Log($"An enemy hited you!! health is now at: {health}");
        }
    }

    private void Movement(float Speed = 1f, float rMult = 1f, float rSMult = 1f)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rMult = runningMultiplier;
            rSMult = runningMultiplier * 1.1f;
            running = true;
        }
        else running = false;
        if (Input.GetKey(KeyCode.W))
            ccPlayer.Move((Speed - speedReduction) * Time.deltaTime * transform.forward * rMult);

        if (Input.GetKey(KeyCode.S))
            ccPlayer.Move((Speed - speedReduction) * Time.deltaTime * (transform.forward * -1));

        if (Input.GetKey(KeyCode.A))
            ccPlayer.Move((Speed - speedReduction) * Time.deltaTime * (transform.right * -1) * rSMult);

        if (Input.GetKey(KeyCode.D))
            ccPlayer.Move((Speed - speedReduction) * Time.deltaTime * transform.right * rSMult);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (lastPosition.x != gameObject.transform.position.x | lastPosition.z != gameObject.transform.position.z)
        {
            lastPosition = gameObject.transform.position;
            moving = true;
        }
        else moving = false;
    }
    private void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            ccPlayer.stepOffset = stepOffset;
        }
        else
        {
            ccPlayer.stepOffset = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        ccPlayer.Move(velocity * Time.deltaTime);

        if (Physics.CheckSphere(headCheck.position, headHitDistance, groundMask) && velocity.y > 0) // para cuando se golpea la cabeza con el techo la velocidad tiene que ser = 0..
        {
            // Debug.Log("te golpeaste la cabeza"); 
            velocity.y = 0f;
        }
    }
    private void MRotation()
    {
        camAxisY += Input.GetAxis("Mouse X") * sensMultiplier;
        camRotation = Quaternion.Euler(0f, camAxisY, 0f);
        transform.rotation = camRotation;

        if (invertYAxis)
            camAxisX -= Input.GetAxis("Mouse Y") * sensMultiplier;
        else
            camAxisX += Input.GetAxis("Mouse Y") * sensMultiplier;

        camAxisX = Mathf.Clamp(camAxisX, -90, 90);
        Quaternion rotX = Quaternion.Euler(camAxisX, 0f, 0f);
        camPivot.transform.localRotation = rotX;
    }
    private void PlayerDeath()
    {
        if (health <= 0)
        {
            GameOver.Instance.PlayerDied();
            DestroyImmediate(gameObject);
        }
    }
    public void OnPoisoned(float timeOnEffect, float poisonDamage)
    {
        isPoisoned = true;
        timeWhileIsPoisoned = timeOnEffect;
        poisonDamageToTake = poisonDamage;
    }
    public void OnSlowed(float timeOnEffect, float intensityEffect)
    {
        isSlowed = true;
        timeWhileIsSlowed = timeOnEffect;
        speedReduction = intensityEffect;
    }
    private IEnumerator Footsteps()
    {
        while (true)
        {
            if (moving && isGrounded)
            {
                if (!running)
                {
                    playAudio.PlayAudioOneShot(Random.Range(0, 3));
                    yield return new WaitForSeconds(footstepInterval);
                    Debug.Log("Walking");
                }
                else if (running)
                {
                    playAudio.PlayAudioOneShot(Random.Range(0, 3));
                    yield return new WaitForSeconds(footstepInterval * (1 / runningMultiplier));
                    Debug.Log("Running");
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}