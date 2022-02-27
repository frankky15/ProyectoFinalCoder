using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight = 2f;
    private float camAxisX = 0f;
    private float camAxisY = 0f;
    private Quaternion camRotation;
    [SerializeField] private bool invertYAxis = true;
    [SerializeField] private float sensMultiplier = 3f;
    [SerializeField] private float runningMultiplier = 1f;
    [SerializeField] private GameObject camPivot;
    [SerializeField] private CharacterController ccPlayer;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float stepOffset = 0.5f;
    private bool isGrounded;

    private Vector3 velocity;

    private void Start()
    {

    }
    private void Update()
    {
        Movement(movementSpeed);

        MRotation();

        Gravity();
    }

    private void Movement(float Speed = 1f, float rMult = 1f, float rSMult = 1f)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rMult = runningMultiplier;
            rSMult = runningMultiplier * 1.1f;
        }
        // if (!isGrounded) // * queria que cuando salte mantenga momentum, pero queda medio mal y roba un poco el control...
        // {
        //     rMult = runningMultiplier;
        //     rSMult = runningMultiplier * 0.3f;
        // }
        if (Input.GetKey(KeyCode.W))
            ccPlayer.Move(Speed * Time.deltaTime * transform.forward * rMult);

        if (Input.GetKey(KeyCode.S))
            ccPlayer.Move(Speed * Time.deltaTime * (transform.forward * -1));

        if (Input.GetKey(KeyCode.A))
            ccPlayer.Move(Speed * Time.deltaTime * (transform.right * -1) * rSMult);

        if (Input.GetKey(KeyCode.D))
            ccPlayer.Move(Speed * Time.deltaTime * transform.right * rSMult);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
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
}