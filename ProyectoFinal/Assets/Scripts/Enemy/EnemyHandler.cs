using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]//Cualquier gameObject que se le ponga este script va a tener el componente Rigidbody
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private float health = 300f;
    public float damage = 50f;
    [SerializeField] private float speed = 5f; //Variable de velocidad
    [SerializeField] private float runSpeed = 7f; //Variable de velocidad al correr
    [SerializeField] private float keepDistance = 2f; //Variable para no pegarse al jugador
    [SerializeField] private float waitAttack = 1f;//Cada cuanto van a ser los ataques del enemigo
    [SerializeField] private float waitTimeDead = 2f; //Variable para saber en cuanto se termina la animacion de muerte


    [SerializeField] private float hearRange = 20f; //Rango para escuchar al jugador cuando este cerca
    [SerializeField] private float hearRunRange = 30f; //Rango mas grande para escuchar al jugador correr
    [SerializeField] private float followRange = 100f;//Rango hasta cuanto va a seguir al jugador

    
    [SerializeField] private bool moveWithWayPoints = false;//Variable para saber si se mueve con wayPoints

    [SerializeField] private WeaponScriptableObject weaponSO;//Script para guardar el daño de las balas del arma del jugador



    private int nextWayPoint = 0;//Waypoint al que el enemigo se esta moviendo
    
    private float timerDead = 0; //Timer para saber cuanto dura la animacion de muerte
    private float timerAttack;//Timer para el waitAttack
    
    private float staffPrimaryDamageMult;

    private bool isFollowing;//Variable para saber si se esta siguiendo al jugador
    private bool canAttack = false;//Variable para saber cuando se puede atacar

    
    private GameObject player;//El jugador
    private GameObject hitBoxAttack; //Collider que simula el ataque
    
    private AnimationsController animator; //Script que activa las animaciones

    [SerializeField] private List<Transform> wayPoints; //Lista de waypoints solo se necesita el container hijo para poder poner los waypoints


    // Start is called before the first frame update
    void Start()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize; //Variable para calcular el daño del disparo principal segun cuanto se cargue 

        hitBoxAttack = transform.GetChild(0).gameObject; //Obtengo la hitbox de ataque
        hitBoxAttack.SetActive(false); //Desactivo la hitbox de ataque

        animator = GetComponent<AnimationsController>(); //Obtengo las animaciones
        if(moveWithWayPoints){//Si se marco la opcion de mover con wayPoints:
            wayPointsSettings();//configuro los waypoints
            nextWayPoint = 0;//Inicializo el primer waypoint
        }
        isFollowing = false;
        canAttack = false;
        
        player = GameObject.Find("Player");//Asigno el jugador a su variable
        if(hearRange > hearRunRange){//Si hearRange es mayor a hearRunRange
            Debug.LogError("hearRange no debe ser menor a hearRunRange");//Mostrar cartel de error
        }
        if(runSpeed < speed){  //Si la velocidad al correr es menor a la velocidad:
            Debug.LogError("la velocidad al correr debe ser mayor o igual a la velocidad normal"); //Mostrar Error
        }
        if(followRange < hearRange || followRange < hearRunRange){ //Si el rango a perseguir, es menor al de escuchar, o menor al de escuchar correr:
            Debug.LogError("El rango para perseguir debe ser mayor a los rangos para escuchar");//Mostrar Error
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveWithWayPoints){//Si la lista de waypoints no esta vacia:
            wayPointsMove();//me muevo por waypoints
        }
        Ears();//Funcion para escuchar cuando el jugador este cerca
        Follow();//Funcion para seguir al jugador
        
        Attack(); //Funcion para atacar
        Death();//Funcion para animar la muerte, y eliminar el gameObject
    }

    void wayPointsSettings(){//Funcion para facilitar la creacion de los waypoints
        GameObject wayPointsContainer = transform.GetChild(1).gameObject;//Container de los waypoints
        int maxWayPoints = wayPointsContainer.transform.childCount;//Cantidad maxima de waypoints en el container
        if(maxWayPoints > 0){//Si existen wayPoints en el container:
            for (var i = 0; i < maxWayPoints; i++) //I = 0, mientras i sea menor a los maximos waypoints, lo que este dentro del for se va a repetir, cada vez que termine las sentencias dentro del ciclo, i aumenta
            {
                wayPoints.Add(wayPointsContainer.transform.GetChild(i));//Añado un waypoint a la lista
            }
            //El for siguiente es a parte para evitar bugs o eliminar waypoints extra
            for(var i = 0;i < wayPoints.Count; i++){ //I = 0, mientras i sea menor al tamaño de la lista de waypoints, lo que este dentro del for se va a repetir, cada vez que termine las sentencias dentro del ciclo, i aumenta
                /*if(wayPoints[i] == null){
                    wayPoints.Remove(wayPoints[i]);
                }*/
                for(var j= i+1;j <= wayPoints.Count -1; j++){ //j = i +1, mientras j sea menor a la lista de waypoints -1....
                    if(wayPoints[i] == wayPoints[j]){ //Si el waypoint en la lista es igual al que apunta la j:
                        wayPoints.Remove(wayPoints[j]); //Elimino el waypoint q apunta a J
                    }
                }
                
            }
            wayPointsContainer.transform.parent = GameObject.Find("WayPointsContainers").transform; //Llevo el container de waypoints al container de containers
            if(wayPoints.Count > 0){ //Si la lista de waypoints no esta vacia:
                moveWithWayPoints = true; //Me muevo con wayPoints
            }
            else{//Sino
                moveWithWayPoints = false; //No me muevo con wayPoints
            }
        }
        else{
            if(wayPoints.Count <= 0){ //Si la lista de waypoints esta vacia:
                moveWithWayPoints = false; //No se mueve por waypoints
            }
        }
        
    }

    void wayPointsMove(){
        if(transform.position != wayPoints[nextWayPoint].position){ //Si mi posicion actual es distinta a la posicion del waypoint al que me muevo:
            transform.position = Vector3.MoveTowards(transform.position,wayPoints[nextWayPoint].position, speed * Time.deltaTime); //Me muevo hacia el siguiente waypoint
        }
     
    }

    void Ears(){
        if(Input.GetKey(KeyCode.LeftShift)){ //Si el jugador esta corriendo:
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))//Si se pulsa alguna tecla para mover, saltar o disparar:
            {
                if(Vector3.Distance(transform.position,player.transform.position) <= hearRunRange){ //Si la distancia entre el jugador es menor o igual a hearRange:
                    if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){ //Si la distancia entre el jugador es mayor keepDistance:
                        isFollowing = true; //El enemigo se va a mover
                        moveWithWayPoints = false; //Dejo de moverme con waypoints si lo estaba haciendo
                    }
                    else{
                        isFollowing = false; //Dejo de moverme
                        if(wayPoints.Count > 0){ //Si la lista de waypoints no esta vacia:
                            moveWithWayPoints = true; //me  muevo con waypoints
                        }
                    }
                }
            }
        }
        if(Vector3.Distance(transform.position,player.transform.position) <= hearRange){ //Si la distancia entre el jugador es menor o igual a hearRange:
            if(Vector3.Distance(transform.position,player.transform.position) > keepDistance){ //Si la distancia entre el jugador es mayor a keepDistance:
                isFollowing = true; //El enemigo va a perseguir al jugador
                moveWithWayPoints = false; //Dejo de moverme con wayPoints
            }
            else{//Sino:
                    isFollowing = false;//Dejo de perseguir
                    if(wayPoints.Count > 0){ //Si la lista de waypoints no esta vacia:
                        moveWithWayPoints = true; //me  muevo con waypoints
                    }
            }
        }
    }

    void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        //Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isFollowing){//Si me estoy moviendo:
            moveWithWayPoints = false;//Dejo de moverme con wayPoints
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime); //Sigo al objetivo
            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador

            transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
        
        }
        if(isFollowing && Vector3.Distance(transform.position,player.transform.position) <= keepDistance){//Si la distancia entre el jugador es menor o igual a keepDistance y me estoy moviendo:
            isFollowing = false;//Dejo de moverme
            canAttack = true;
        }
        if(Vector3.Distance(transform.position,player.transform.position) > followRange){//Si la distancia con el jugador es mayor al rango para perseguir:
            isFollowing = false;//dejo de perseguir
            canAttack = false;//No puedo atacar
            if(wayPoints.Count > 0){//Si la lista de wayPoints no esta vacia:
                moveWithWayPoints = true;//Me muevo con wayPoints
            }
        }

    }
      
    void Attack(){
        timerAttack += Time.deltaTime;//El timer va a ser igual al tiempo
        if(canAttack){
            animator._animateWhenRun = false; //Cancelo la animacion de correr de la araña
            animator.Attack(); //Llamo a la funcion de animacion de ataque en el script de animacion
            hitBoxAttack.SetActive(true);
            if(timerAttack >= waitAttack){//si el timer llega a su wait:
                hitBoxAttack.SetActive(false);
                canAttack = false;//ya no se puede atacar
                isFollowing = true; //Sigo al jugador
                animator._animateWhenRun = true; //Reactivo la animacion al correr
                timerAttack = 0;//Reinicio el timer
            }
        }
    }
    void Death(){
        if(health <= 0){ //Si la vida es menor o igual a 0:
            moveWithWayPoints = false; //No me muevo
            isFollowing = false; //No persigo
            canAttack = false; //No puedo atacar
            speed = 0; //No me muevo
            animator.SetDead(); //Activo la animacion de tiempo
            timerDead += Time.deltaTime; //Activo el timer de muerte
            if(timerDead >= waitTimeDead){ //Si el timer llega a waitTimeDead:
                Destroy(transform.gameObject); //Me elimino de la escena
            }
        }
    }

    private void OnTriggerEnter(Collider other)
        {
        if(other.CompareTag("WayPoint")) //Si entro a un waypoint:
        {
            nextWayPoint++;//Cambio al siguiente waypoint
            if(nextWayPoint == wayPoints.Count)//Si el siguiente waypoint apunta mas alla del maximo de la lista de waypoints:
            {
                nextWayPoint = 0;//Vuelvo al primer waypoint
            }
        }
        switch (other.name)
        {
            case "Staff Primary Projectile(Clone)": //Si recibe la bala principal del arma principal:
                health -= staffPrimaryDamageMult * other.transform.localScale.y; //Pierde salud en base al daño de la bala y tamaño de la bala
                animator.Hit(); //Hace una animacion para recibir el daño
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false;  //No se mueve por way points
                Debug.Log($"Enemys health is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "StaffSecondary BulletHole(Clone)": //Si recibe la bala secundaria del arma principal:
                health -= weaponSO.staffSecondaryDamage; //Pierde salud en base al daño de la bala
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false; //No se mueve por way points
                animator.Hit(); //Hace una animacion de hit
                Debug.Log($"Enemys health is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "ShotgunSwordSecondary BulletHole(Clone)": //Si recibe la bala principal del arma principal:
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false; //No se mueve por way points
                health -= weaponSO.sSwordSecondaryDamage; //Pierde salud en base al daño de la bala
                animator.Hit(); //Hace la animacion de hit
                Debug.Log($"Enemys health is now at: {health}"); //Mensaje con la vida del enemigo
                break;
        }
        
        }
}
