using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private WeaponScriptableObject weaponSO; //-
    [SerializeField] private float deadResetTime = 4f;
    [SerializeField] private float health = 300f;
    private float healthLeft;
    private bool isDead;
    private float deadCooldown;
    private float staffPrimaryDamageMult;//-
    
    private void Awake()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize; //-
        healthLeft = health;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        IsDead();
    }
    private void OnTriggerEnter(Collider other) //-
    {
        Debug.Log($"Enemys health is now at: {healthLeft}");

        if (other.name == "Staff Primary Projectile(Clone)") // staff carga
        {
            healthLeft -= staffPrimaryDamageMult * other.transform.localScale.y;
        }
        
        if (other.name == "StaffSecondary BulletHole(Clone)") // staff subfusil
        {
            healthLeft -= weaponSO.staffSecondaryDamage;
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
}
