using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamDetect : MonoBehaviour
{
    [SerializeField] WeaponScriptableObject weaponSO;
    [SerializeField] float health = 300f;
    private float staffPrimaryDamageMult;
    private void Awake()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize;
    }
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enemys health is now at: {health}");

        if (other.name == "Staff Primary Projectile(Clone)")
        {
            health -= staffPrimaryDamageMult * other.transform.localScale.y;
        }
        
        if (other.name == "StaffSecondary BulletHole(Clone)")
        {
            health -= weaponSO.staffSecondaryDamage;
        }

        if (other.name == "ShotgunSwordSecondary BulletHole(Clone)")
        {
            health -= weaponSO.sSwordSecondaryDamage;
        }
    }
}
