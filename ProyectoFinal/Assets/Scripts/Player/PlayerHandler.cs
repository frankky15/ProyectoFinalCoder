using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHandler : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    private float camAxisX = 0f;
    private float camAxisY = 0f;
    private Quaternion camRotation;
    [SerializeField] bool invertYAxis = true;
    [SerializeField] float sensMultiplier = 3f;
    [SerializeField] float runningMultiplier = 1f;
    [SerializeField] private GameObject camPivot;

    private void Update()
    {
        FixedMovement(movementSpeed);

        MRotation();
    }

    private void Movement () // *1 Este es el metodo de movimiento usando fuerzas
    {
        
    }
    private void FixedMovement(float Speed = 1f, float rMult = 1f, float rSMult = 1f) // *1 Estes es el metodo de movimiento sin usar fuerzas
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            rMult = runningMultiplier;
            rSMult = runningMultiplier * 0.7f;
        }

        if (Input.GetKey(KeyCode.W))
            transform.Translate(Speed * Time.deltaTime * Vector3.forward * rMult);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(Speed * Time.deltaTime * Vector3.back);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(Speed * Time.deltaTime * Vector3.left * rSMult);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(Speed * Time.deltaTime * Vector3.right * rSMult);
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