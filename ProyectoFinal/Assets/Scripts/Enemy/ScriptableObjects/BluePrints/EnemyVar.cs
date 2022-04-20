using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyVar", menuName = "ScriptableObjects/EnemyVar")]
public class EnemyVar : ScriptableObject
{
    public int points = 10;
    [SerializeField] private int multiplierPoints = 1;
    public float health = 60;
    [SerializeField] float multiplierHealth = 1f;
    public float damage = 50f; //Daño de la araña
    [SerializeField] float multiplierDamage =1f;
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
    public float visionOnWallRange = 3f;//Variable para saber a que distancia de una pared cambia su direccion
    public float followRange = 30f;//Rango hasta cuanto va a seguir al jugador
    [SerializeField] float multiplierFollowRange = 1f;

    public float waitNewDirection = 0.9f;

    private int score = 0;
    private void Start() {
        score = PlayerPrefs.GetInt("score", 0);
        points += score*multiplierPoints;
        health += score*multiplierHealth;
        damage += score*multiplierDamage;
        followRange += score*multiplierFollowRange;
        visionRange += score*multiplierFollowRange;
    }
}
