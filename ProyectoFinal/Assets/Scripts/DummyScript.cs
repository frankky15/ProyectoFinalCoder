using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private WeaponScriptableObject weaponSO;
    [SerializeField] private float deadResetTime = 4f;
    [SerializeField] private float pushedResetTime = 0.1f;
    [SerializeField] private float health = 300f;
    private float healthLeft;
    private bool isDead;
    private float deadCooldown;
    private bool wasPushed;
    private float pushedCooldown;

    private float staffPrimaryDamageMult;
    private void Awake()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize;
        healthLeft = health;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        IsDead();
        WasPushed();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enemys health is now at: {healthLeft}");

        if (other.name == "Staff Primary Projectile(Clone)")
        {
            healthLeft -= staffPrimaryDamageMult * other.transform.localScale.y;
            wasPushed = true;
            pushedCooldown = pushedResetTime + Time.time;
        }
        
        if (other.name == "StaffSecondary BulletHole(Clone)")
        {
            healthLeft -= weaponSO.staffSecondaryDamage;
            wasPushed = true;
            pushedCooldown = pushedResetTime + Time.time;
        }

        if (other.name == "ShotgunSwordSecondary BulletHole(Clone)")
        {
            healthLeft -= weaponSO.sSwordSecondaryDamage;
            wasPushed = true;
            pushedCooldown = pushedResetTime + Time.time;
        }
    }

    private void WasPushed()
    {
        animator.SetBool("wasPushed", wasPushed);
        if (pushedCooldown < Time.time)
            wasPushed = false;
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
