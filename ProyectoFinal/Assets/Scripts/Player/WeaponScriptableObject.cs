
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapons")]
public class WeaponScriptableObject : ScriptableObject
{
    // * ShotgunSword variables * //
    public float sSwordPrimaryDamage = 50f;
    public float sSwordPrimaryRate = 1f;
    public float sSwordSecondaryDamage = 20f;
    public float sSwordSecondaryRate = 1f;
    public float sSwordSecondarySpread = 1f;

    // * Staff variables * //
    public float staffPrimaryDamage = 40f;
    public float staffPrimaryRate = 2f;
    public float staffPrimaryDamMult = 4f;
    public float staffPrimaryChargeTime = 1f;
    public float staffPrimaryMaxSize = 5f;
    public float staffSecondaryDamage = 5f;
    public float staffSecondaryRate = 0.2f;
    }