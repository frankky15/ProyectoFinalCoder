using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapons")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public GameObject prefab;

     // * Primary * //
    public GameObject p_projectile;
    public GameObject p_flashEffect;
    public float p_castTime;
    public float p_fireRate;
    public float p_magAmount;
    public float p_pelletAmount;
    public float p_spread;
    public float p_reloadTime;
    public float p_projectileMinSize;
    public float p_projectileMaxSize;
    public float p_lifeTime;

    // * Secondary * //
    public GameObject s_projectile;
    public GameObject s_flashEffect;
    public float s_castTime;
    public float s_fireRate;
    public float s_magAmount;
    public float s_pelletAmount;
    public float s_spread;
    public float s_reloadTime;
    public float s_projectileMinSize;
    public float s_projectileMaxSize;
    public float s_lifeTime;

    // * Misc * //
    public float flashTime = 0.1f;
    public float pulloutTime = 0.5f;
}