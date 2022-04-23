using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyVar", menuName = "ScriptableObjects/EnemyVar")]
public class EnemyVar : ScriptableObject
{
    public float health = 60;
    [SerializeField] float multiplierHealth = 1f;
    [SerializeField] float maxHealth = 900f;
    public float damage = 50f; //Daño de la araña
    [SerializeField] float multiplierDamage =1f;
    [SerializeField] float maxDamage = 100f;
    public float speed = 5f; //Variable de velocidad
    public float runSpeed = 7f; //Variable de velocidad al correr
    public float keepDistance = 2f; //Variable para no pegarse al jugador
    public float waitAttack = 1f;//Cada cuanto van a ser los ataques del enemigo
    public float waitTimeDead = 2f; //Variable para saber en cuanto se termina la animacion de muerte
    public float timeToSetAttack = 0.3f; //Variable para cuanto tiempo de empezada la animacion de ataque, activar el ataque
    public float timeToQuitAttack = 0.5f; //Variable para pasado de un tiempo sacar el ataque

    public float visionRange = 50f;
    [SerializeField] float multiplierVisionRange = 1f;
    public float hearRange = 5f;
    public float visionOnWallRange = 0.5f;//Variable para saber a que distancia de una pared cambia su direccion
    public float followRange = 30f;//Rango hasta cuanto va a seguir al jugador
    [SerializeField] float multiplierFollowRange = 1f;

    public float waitNewDirection = 0.9f;

    //El daño del revolver y de la escopeta son las ultimas añadidas asi que aun no le hemos puesto daño en sus scriptableObjects, asi que por ahora les usamos el daño asi
    public float revolverDamage = 60f;
    public float shotgunDamage = 200f;

    public float timeEffect = 0.0f;
    [SerializeField] private float timeEffectMultiplier = 0.0f;
    [SerializeField] float maxEffectTime = 10f;
    public float effectInstensity = 0.0f;
    [SerializeField] private float effectIntensityMultiplier = 0.0f;
    [SerializeField] float maxEffectIntensity = 5f;

    int score = 0;
    protected virtual void Start() {
        score = ScoreManager.Instance.score;
        Debug.Log(score);
        if(health < maxHealth) health += score*multiplierHealth; else health = maxHealth;
        if(damage < maxDamage)    damage += score*multiplierDamage; else damage = maxDamage;
        followRange += score*multiplierFollowRange;
        visionRange += score*multiplierFollowRange;
        if(effectInstensity < maxEffectIntensity) effectInstensity += score * effectIntensityMultiplier; else effectInstensity = maxEffectIntensity;
        if(timeEffect < maxEffectTime) timeEffect += score * timeEffectMultiplier; else timeEffect = maxEffectTime;
    }
}
