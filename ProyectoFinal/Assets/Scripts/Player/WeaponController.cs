using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponsSO;

    // * Staff variables * //
    [SerializeField] private GameObject staffGameobject;
    [SerializeField] private GameObject staffPrimaryProjectile;
    [SerializeField] private GameObject staffGunpoint;
    private float staffPrimaryCooldown;
    private float staffPrimarySize = 1f;
    private float _staffPrimaryDamage;
    private bool isUsingStaff;
    private bool isUsingSSword;

    private void Start()
    {
        isUsingStaff = true;
        isUsingSSword = false;

        _staffPrimaryDamage =  weaponsSO.staffPrimaryDamage;

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
            _staffPrimaryDamage += weaponsSO.staffPrimaryDamage * weaponsSO.staffPrimaryDamMult * Time.deltaTime * 10f;
            staffPrimarySize = Mathf.Clamp((staffPrimarySize += weaponsSO.staffPrimaryChargeTime * Time.deltaTime), 1f, weaponsSO.staffPrimaryMaxSize);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && staffPrimaryCooldown < Time.time)
        {
            // Debug.Log("Shooting StaffPrimary");

            GameObject projectile = Instantiate(staffPrimaryProjectile, staffGunpoint.transform.position, staffGunpoint.transform.rotation);
            projectile.transform.localScale = staffGunpoint.transform.localScale;
            staffPrimarySize = 1f;
            _staffPrimaryDamage = weaponsSO.staffPrimaryDamage;
            staffGunpoint.SetActive(false);
            staffPrimaryCooldown = Time.time + weaponsSO.staffPrimaryRate;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Shooting StaffSecondary");
        }
    }
}