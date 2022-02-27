using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]//Cualquier gameObject que se le ponga este script va a tener el componente Rigidbody
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f; //Variable de velocidad
    [SerializeField] private float keepDistance = 2f; //Variable para no pegarse al jugador
    [SerializeField] private float waitAttack = 1f;//Cada cuanto van a ser los ataques del enemigo

    [SerializeField] private float hearRange = 20f; //Rango para escuchar al jugador cuando este cerca
    [SerializeField] private float hearRunRange = 30f; //Rango mas grande para escuchar al jugador correr

    private float timerAttack;//Timer para el waitAttack
    private bool isMoving;//Variable para saber si me estoy moviendo
    private bool canAttack = false;//Variable para saber cuando se puede atacar
    private GameObject player;//El jugador

    //private Animator anim;//Variable que contiene el componente para animar

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");//Asigno el jugador a su variable
        //anim = GetComponent<Animator>(); //Obtengo el componente para animar
    }

    // Update is called once per frame
    void Update()
    {
        Ears();//Funcion para escuchar cuando el jugador este cerca
        Follow();//Funcion para seguir al jugador
        //Attack();//Funcion para atacar
    }
    void Ears(){
        if(Input.GetKey(KeyCode.LeftShift)){//Si el jugador esta corriendo:
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))//Si se pulsa alguna tecla para mover, saltar o disparar:
            {
                if(Vector3.Distance(transform.position,player.transform.position) <= hearRunRange){//Si la distancia entre el jugador es menor o igual a hearRange:
                    if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){//Si la distancia entre el jugador es mayor keepDistance:
                        isMoving = true;//El enemigo se va a mover
                    }
                    else{
                        isMoving = false;//Dejo de moverme

                    }
                }
            } 
        }
        if(Vector3.Distance(transform.position,player.transform.position) <= hearRange){//Si la distancia entre el jugador es menor o igual a hearRange:
            if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){//Si la distancia entre el jugador es mayor keepDistance:
                isMoving = true;//El enemigo se va a mover
            }
            else{
                isMoving = false;//Dejo de moverme

            }
        }
    }

    void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isMoving){//Si me estoy moviendo:
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); //Sigo al objetivo
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador

            transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
        
        }
        if(isMoving && Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador es menor o igual a keepDistance:
            isMoving = false;//Dejo de moverme
        }

    }
      
    /*void Attack(){
        if(Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador y el enemigo es menor o igual a keepDistance:
            canAttack = true;//Puedo Atacar
            isMoving = false;//Dejo de moverme
        }
        if(canAttack && !isMoving){//Si puedo atacar y no me estoy moviendo:
            //anim.SetBool("canAttack",true);//Ataco
            timerAttack += Time.deltaTime;//El timer va a ser igual al tiempo
            if(timerAttack >= waitAttack){//si el timer llega a su wait:
                canAttack = false;//ya no se puede atacar
                //anim.SetBool("canAttack",false);//ya no estoy atacando
                timerAttack = 0;//Reinicio el timer
            }
        }
    }*/
}
