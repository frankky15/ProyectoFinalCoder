using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private OldWeaponScriptableObject weaponSO; //-
    [SerializeField] private float deadResetTime = 4f;
    [SerializeField] private float health = 300f;
    private float healthLeft;
    private bool isDead;
    private float deadCooldown;
    private float staffPrimaryDamageMult;//-
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.3f;
    private bool isGrounded;
    private Rigidbody rb;
    private bool isGroundedOneShotBool = true;

    private void Awake()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize; //-
        healthLeft = health;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {

    }

    private void Update()
    {
        IsGrounded();
    }
    private void FixedUpdate()
    {
        IsDead();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector3.down);
    }
    private void OnTriggerEnter(Collider other) //-
    {
        Debug.Log($"Enemys health is now at: {healthLeft}");

        if (other.name == "Staff Primary Projectile(Clone)") // staff carga
        {
            healthLeft -= staffPrimaryDamageMult * other.transform.localScale.y;
            if (isGrounded) rb.AddForce(((other.transform.position - gameObject.transform.position).normalized) * -6 + Vector3.up * 10, ForceMode.Impulse);
        }

        if (other.name == "StaffSecondary BulletHole(Clone)") // staff subfusil
        {
            healthLeft -= weaponSO.staffSecondaryDamage;
            if (isGrounded) rb.AddForce(((other.transform.position - gameObject.transform.position).normalized) * -3 + Vector3.up * 5, ForceMode.Impulse);
        }

        if (other.name == "ShotgunSwordSecondary BulletHole(Clone)") // escopeta
        {
            healthLeft -= weaponSO.sSwordSecondaryDamage;
        }
    }

    private void IsDead()
    {
        animator.SetBool("isDead", isDead);

        if (healthLeft <= 0)
        {
            isDead = true;
            healthLeft = health;
            deadCooldown = deadResetTime + Time.time;
        }
        if (deadCooldown < Time.time)
            isDead = false;
    }
    private void IsGrounded()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundMask);

        // playOneShot
        if (!isGrounded && isGroundedOneShotBool)
        {
            GameManager.Instance.TimeManager("slowmo");
            isGroundedOneShotBool = false;
        }
        else if (isGrounded && !isGroundedOneShotBool)
        {
            GameManager.Instance.TimeManager("default");
            isGroundedOneShotBool = true;
        }
    }
}
