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
    [SerializeField] private float followRange = 100f;//Rango hasta cuanto va a seguir al jugador

    [SerializeField] private List<Transform> wayPoints;//Lista de waypoints (Le quitaria el serializado, pero daria errores, ya que solo se necesita el container hijo para poder poner los waypoints)


    private int nextWayPoint = 0;//Waypoint al que el enemigo se esta moviendo

    private float timerAttack;//Timer para el waitAttack
    private bool moveWithWayPoints;//Variable para saber si se mueve con wayPoints
    private bool isFollowing;//Variable para saber si se esta siguiendo al jugador
    private bool canAttack = false;//Variable para saber cuando se puede atacar
    
    private GameObject player;//El jugador

    //private Animator anim;//Variable que contiene el componente para animar

    // Start is called before the first frame update
    void Start()
    {
        wayPointsSettings();
        player = GameObject.Find("Player");//Asigno el jugador a su variable
        //anim = GetComponent<Animator>(); //Obtengo el componente para animar
        if(hearRange > hearRunRange){//Si hearRange es mayor a hearRunRange
            Debug.LogError("hearRange no debe ser menor a hearRunRange");//Mostrar cartel de error
        }
        nextWayPoint = 0;//Inicializo el primer waypoint
    }

    // Update is called once per frame
    void Update()
    {
        if(moveWithWayPoints){//Si la lista de waypoints no esta vacia:
            wayPointsMove();//me muevo por waypoints
        }
        Ears();//Funcion para escuchar cuando el jugador este cerca
        Follow();//Funcion para seguir al jugador
        
        //Attack();//Funcion para atacar
    }

    void wayPointsSettings(){//Funcion para facilitar la creacion de los waypoints
        GameObject wayPointsContainer = transform.GetChild(0).gameObject;//Container de los waypoints
        int maxWayPoints = wayPointsContainer.transform.childCount;//Cantidad maxima de waypoints en el container
        for (var i = 0; i < maxWayPoints; i++)//I = 0, mientras i sea menor a los maximos waypoints, lo que este dentro del for se va a repetir, cada vez que termine las sentencias dentro del ciclo, i aumenta
        {
            wayPoints.Add(wayPointsContainer.transform.GetChild(i));//Añado un waypoint a la lista
        }
        //El for siguiente es a parte para evitar bugs o eliminar waypoints extra
        for(var i = 0;i < wayPoints.Count; i++){//I = 0, mientras i sea menor al tamaño de la lista de waypoints, lo que este dentro del for se va a repetir, cada vez que termine las sentencias dentro del ciclo, i aumenta
            /*if(wayPoints[i] == null){
                wayPoints.Remove(wayPoints[i]);
            }*/
            for(var j= i+1;j <= wayPoints.Count -1; j++){//j = i +1, mientras j sea menor a la lista de waypoints -1....
                if(wayPoints[i] == wayPoints[j]){//Si el waypoint en la lista es igual al que apunta la j:
                    wayPoints.Remove(wayPoints[j]);//Elimino el waypoint q apunta a J
                }
            }
            
        }
        wayPointsContainer.transform.parent = GameObject.Find("WayPointsContainers").transform;//Llevo el container de waypoints al container de containers
        if(wayPoints[0] != null){//Si la lista de waypoints no es nula:
            moveWithWayPoints = true;//Me muevo con wayPoints
        }
        else{//Sino
            moveWithWayPoints = false;//No me muevo con wayPoints
        }
    }

    void wayPointsMove(){
        if(transform.position != wayPoints[nextWayPoint].position){//Si mi posicion actual es distinta a la posicion del waypoint al que me muevo:
            transform.position = Vector3.MoveTowards(transform.position,wayPoints[nextWayPoint].position, speed * Time.deltaTime);//Me muevo hacia el siguiente waypoint
        }
     
    }

    void Ears(){
        if(Input.GetKey(KeyCode.LeftShift)){//Si el jugador esta corriendo:
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))//Si se pulsa alguna tecla para mover, saltar o disparar:
            {
                if(Vector3.Distance(transform.position,player.transform.position) <= hearRunRange){//Si la distancia entre el jugador es menor o igual a hearRange:
                    if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){//Si la distancia entre el jugador es mayor keepDistance:
                        isFollowing = true;//El enemigo se va a mover
                        moveWithWayPoints = false;
                    }
                    else{
                        isFollowing = false;//Dejo de moverme
                        moveWithWayPoints = true;
                    }
                }
            }
        }
        if(Vector3.Distance(transform.position,player.transform.position) <= hearRange){//Si la distancia entre el jugador es menor o igual a hearRange:
            if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){//Si la distancia entre el jugador es mayor keepDistance:
                isFollowing = true;//El enemigo se va a mover
                moveWithWayPoints = false;
            }
            else{
                isFollowing = false;//Dejo de moverme
            }
        }
    }

    void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        //Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isFollowing){//Si me estoy moviendo:
            moveWithWayPoints = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); //Sigo al objetivo
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador

            transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
        
        }
        if(isFollowing && Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador es menor o igual a keepDistance y me estoy moviendo:
            isFollowing = false;//Dejo de moverme
        }
        if(Vector3.Distance(transform.position,player.transform.position) > followRange){
            isFollowing = false;
            canAttack = false;
            if(wayPoints[0] != null){
                moveWithWayPoints = true;
            }
        }

    }
      
    /*void Attack(){
        if(Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador y el enemigo es menor o igual a keepDistance:
            canAttack = true;//Puedo Atacar
            isFollowing = false;//Dejo de moverme
        }
        if(canAttack && isFollowing){//Si puedo atacar y no me estoy moviendo:
            //anim.SetBool("canAttack",true);//Ataco
            timerAttack += Time.deltaTime;//El timer va a ser igual al tiempo
            if(timerAttack >= waitAttack){//si el timer llega a su wait:
                canAttack = false;//ya no se puede atacar
                //anim.SetBool("canAttack",false);//ya no estoy atacando
                timerAttack = 0;//Reinicio el timer
            }
        }
    }*/
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("WayPoint")){//Si entro a un waypoint:
            nextWayPoint++;//Cambio al siguiente waypoint
            if(nextWayPoint == wayPoints.Count){//Si el siguiente waypoint apunta mas alla del maximo de la lista de waypoints:
                nextWayPoint = 0;//Vuelvo al primer waypoint
            }
        }
    }
}
