using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyVar", menuName = "ScriptableObjects/EnemyVar")]
public class EnemyVar : ScriptableObject
{
    public float damage = 50f; //Daño de la araña
    public float speed = 5f; //Variable de velocidad
    public float runSpeed = 7f; //Variable de velocidad al correr
    public float keepDistance = 2f; //Variable para no pegarse al jugador
    public float waitAttack = 1f;//Cada cuanto van a ser los ataques del enemigo
    public float waitTimeDead = 2f; //Variable para saber en cuanto se termina la animacion de muerte
    public float timeToSetAttack = 0.3f; //Variable para cuanto tiempo de empezada la animacion de ataque, activar el ataque
    public float timeToQuitAttack = 0.5f; //Variable para pasado de un tiempo sacar el ataque

    public float hearRange = 20f; //Rango para escuchar al jugador cuando este cerca
    public float hearRunRange = 30f; //Rango mas grande para escuchar al jugador correr
    public float followRange = 100f;//Rango hasta cuanto va a seguir al jugador
}
