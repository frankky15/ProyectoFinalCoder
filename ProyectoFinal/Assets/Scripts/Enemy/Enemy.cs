using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f; //Variable de velocidad
    [SerializeField] private float keepDistance = 2f; //Variable para no pegarse al jugador
    [SerializeField] private float waitAttack = 1f;//Cada cuanto van a ser los ataques del enemigo

    private float timerAttack;//Timer para el waitAttack
    private bool isMoving;
    private bool canAttack = false;//Variable para saber cuando se puede atacar
    private GameObject player;//El jugador
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");//Asigno el jugador a su variable
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Attack();
    }

    void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        if(Vector3.Distance(transform.position,player.transform.position) > keepDistance && !canAttack){//Si la distancia entre el jugador y ele enemigo es mayor a keepDistance, y no puedo atacar:
            isMoving = true;//Me estoy moviendo
        }
        else//Sino
        {
            isMoving = false;//No me estoy moviendo
        }
        if(isMoving){//Si me estoy moviendo:
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); //Sigo al objetivo
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);

            transform.rotation = newRotation;
        
        }

    }
      
    void Attack(){
        if(Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador y el enemigo es menor o igual a keepDistance:
            canAttack = true;//Puedo Atacar
            isMoving = false;//Dejo de moverme
        }
        if(canAttack && !isMoving){//Si puedo atacar y no me estoy moviendo:
            anim.SetBool("canAttack",true);//Ataco
            timerAttack += Time.deltaTime;//El timer va a ser igual al tiempo
            if(timerAttack >= waitAttack){//si el timer llega a su wait:
                canAttack = false;//ya no se puede atacar
                anim.SetBool("canAttack",false);//ya no estoy atacando
                timerAttack = 0;
            }
        }
    }
}
