using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponsSO;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform hitscanTransform;

    // * Staff variables * //
    [SerializeField] private GameObject staffGameobject;
    [SerializeField] private GameObject staffPrimaryProjectile;
    [SerializeField] private GameObject staffGunpoint;
    [SerializeField] private GameObject staffSecondaryMuzzleFlash;

    // * Shotgun Sword variables * //
    [SerializeField] private Transform sSwordGunpoint;
    [SerializeField] private GameObject bulletHole;
    private float staffPrimaryCooldown;
    private float staffPrimarySize = 1f;
    private float _staffPrimaryDamage;
    private float staffSecondaryCooldown;
    private float staffSecondaryMuzzleflashCooldown;
    private bool staffSecondaryMuzzleFlashBool;
    private bool isUsingStaff;
    private bool isUsingSSword;


    private void Start()
    {
        isUsingStaff = true;
        isUsingSSword = false;
        _staffPrimaryDamage = weaponsSO.staffPrimaryDamage;
    }
    private void Update()
    {
        WeaponSwitch();
        ShotgunSword();
        Staff();
        HitscanRaycast();
    }
    private void LateUpdate()
    {

    }

    private void HitscanRaycast()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hitscanHit, 999f, aimColliderMask))
        {
            hitscanTransform.position = hitscanHit.point;
        }
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

    // ? Shotgun Sword //
    private void ShotgunSword()
    {
        if (!isUsingSSword)
        {
            return;
        }

        ShotgunSwordPrimary();
        ShotgunSwordSecondary();
    }
    private void ShotgunRaycast(Vector3 spread)
    {
        // Debug.Log(spread);

        if (Physics.Raycast(sSwordGunpoint.transform.position, spread, out RaycastHit pelletHit, 999f, aimColliderMask))
        {
            GameObject pelletHole = Instantiate(bulletHole, pelletHit.point, Quaternion.identity) as GameObject;
            Destroy(pelletHole, 2f);
            pelletHole.transform.LookAt(pelletHit.point + pelletHit.normal); // esto es para cuando le ponga particulas..

            // Debug.Log("Shotgun Raycast se ejecuto completamente");
        }
    }
    private void ShotgunSwordPrimary()
    {

    }
    private void ShotgunSwordSecondary()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            for (int i = 0; i < weaponsSO.sSwordSecondaryPelletAmm; i++)
            {
                // * dispersion
                Vector3 sSwordSpread = sSwordGunpoint.transform.position + staffGunpoint.transform.forward * 1000f;
                sSwordSpread += Random.Range(-weaponsSO.sSwordSecondarySpread, weaponsSO.sSwordSecondarySpread) * transform.right;
                sSwordSpread += Random.Range(-weaponsSO.sSwordSecondarySpread, weaponsSO.sSwordSecondarySpread) * transform.up;
                sSwordSpread -= sSwordGunpoint.transform.position;
                sSwordSpread.Normalize();
                // * aplico un raycast con la info de dispersion
                ShotgunRaycast(sSwordSpread);
                //Debug.Log(hitscanTransform.position);
            }
        }
    }
    // ? staff //
    private void Staff()
    {
        staffGunpoint.transform.localScale = new Vector3(staffPrimarySize, staffPrimarySize, staffPrimarySize);

        if (!isUsingStaff)
        {
            return;
        }

        StaffPrimary();
        StaffSecondary();
    }
    private void StaffPrimary()
    {
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

            Vector3 aimDir = (hitscanTransform.position - staffGunpoint.transform.position).normalized;
            GameObject projectile = Instantiate(staffPrimaryProjectile, staffGunpoint.transform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            projectile.transform.localScale = staffGunpoint.transform.localScale;
            staffPrimarySize = 1f;
            _staffPrimaryDamage = weaponsSO.staffPrimaryDamage;
            staffGunpoint.SetActive(false);
            staffPrimaryCooldown = Time.time + weaponsSO.staffPrimaryRate;
        }
    }
    private void StaffSecondary()
    {
        staffSecondaryMuzzleFlash.SetActive(staffSecondaryMuzzleFlashBool);

        if (Input.GetKey(KeyCode.Mouse1) && staffSecondaryCooldown < Time.time)
        {
            Debug.Log("Shooting StaffSecondary");
            staffSecondaryMuzzleflashCooldown = weaponsSO.staffSecondaryFlashTime + Time.time;
            staffSecondaryMuzzleFlashBool = true;
            if (staffSecondaryCooldown < Time.time)
            {
                GameObject bulletHole = Instantiate(this.bulletHole, hitscanTransform.position, Quaternion.identity) as GameObject;
                Destroy(bulletHole, 2f);
            }
            staffSecondaryCooldown = weaponsSO.staffSecondaryRate + Time.time;
        }
        if (staffSecondaryMuzzleFlashBool && staffSecondaryMuzzleflashCooldown < Time.time)
        {
            staffSecondaryMuzzleFlashBool = false;
        }
    }
}