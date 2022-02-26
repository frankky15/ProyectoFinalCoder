using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // * ShotgunSword variables * //
    [SerializeField] private float sSwordPrimaryDamage = 50f;
    [SerializeField] private float sSwordPrimaryRate = 1f;
    [SerializeField] private float sSwordSecondaryDamage = 20f;
    [SerializeField] private float sSwordSecondaryRate = 1f;
    [SerializeField] private float sSwordSecondarySpread = 1f;
    private bool isUsingSSword;

    // * Staff variables * //
    [SerializeField] private GameObject staffGameobject;
    [SerializeField] private GameObject staffPrimaryProjectile;
    [SerializeField] private GameObject staffGunpoint;
    [SerializeField] private Transform staffShootPoint;
    [SerializeField] private float staffPrimaryDamage = 40f;
    [SerializeField] private float staffPrimaryRate = 2f;
    private float staffPrimaryCooldown;
    [SerializeField] private float staffPrimaryDamMult = 4f;
    [SerializeField] private float staffPrimaryChargeTime = 1f;
    [SerializeField] private float staffPrimaryMaxSize = 5f;
    private float staffPrimarySize = 1f;
    private float _staffPrimaryDamage;
    [SerializeField] private float staffSecondaryDamage = 5f;
    [SerializeField] private float staffSecondaryRate = 0.2f;
    private bool isUsingStaff;

    private void Start()
    {
        isUsingStaff = true;
        isUsingSSword = false;

        _staffPrimaryDamage = staffPrimaryDamage;

    }
    private void Update()
    {
        WeaponSwitch();
        ShotgunSword();
        Staff();
    }

    private void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isUsingStaff = true;
            isUsingSSword = false;
            Debug.Log("Using Staff");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isUsingSSword = true;
            isUsingStaff = false;
            Debug.Log("Using Sword");
        }
    }
    // * Weapons * //
    private void ShotgunSword()
    {
        if (!isUsingSSword)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Sword Mele");
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Shooting ShotgunSword");
        }
    }
    private void Staff()
    {
        staffGunpoint.transform.localScale = new Vector3(staffPrimarySize, staffPrimarySize, staffPrimarySize);

        if (!isUsingStaff)
        {
            return;
        }

        // if (Input.GetKeyDown(KeyCode.Mouse0) && staffPrimaryCooldown < Time.time)
        // {
        // }
        if (Input.GetKey(KeyCode.Mouse0) && staffPrimaryCooldown < Time.time)
        {
            // Debug.Log("Charging Staff " + _staffPrimaryDamage);
            
            staffGunpoint.SetActive(true);
            _staffPrimaryDamage += staffPrimaryDamage * staffPrimaryDamMult * Time.deltaTime * 10f;
            staffPrimarySize = Mathf.Clamp((staffPrimarySize += staffPrimaryChargeTime * Time.deltaTime), 1f, staffPrimaryMaxSize);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && staffPrimaryCooldown < Time.time)
        {
            // Debug.Log("Shooting StaffPrimary");

            GameObject projectile = Instantiate(staffPrimaryProjectile, staffShootPoint.position, staffGunpoint.transform.rotation);
            projectile.transform.localScale = staffGunpoint.transform.localScale;
            staffPrimarySize = 1f;
            _staffPrimaryDamage = staffPrimaryDamage;
            staffGunpoint.SetActive(false);
            staffPrimaryCooldown = Time.time + staffPrimaryRate;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Shooting StaffSecondary");
        }
    }
}