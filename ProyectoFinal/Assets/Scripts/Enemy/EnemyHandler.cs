using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public float health = 300f; //Salud de la araña

    [SerializeField] protected bool moveWithWayPoints = false;//Variable para saber si se mueve con wayPoints

    [SerializeField] protected EnemyVar vars;

    [SerializeField] private OldWeaponScriptableObject weaponSO;//Script para guardar el daño de las balas del arma del jugador

    [SerializeField] protected LayerMask layersDetect = new LayerMask();


    private int nextWayPoint = 0;//Waypoint al que el enemigo se esta moviendo
    
    private float timerDead = 0; //Timer para saber cuanto dura la animacion de muerte
    private float timerAttack;//Timer para el vars.waitAttack
    
    private float staffPrimaryDamageMult; //Daño de la bala del disparo cargado

    protected bool isFollowing;//Variable para saber si se esta siguiendo al jugador
    protected bool canAttack = false;//Variable para saber cuando se puede atacar
    protected bool isHurt = false;
    protected bool isDeath = false;

    protected GameObject player;//El jugador
    private GameObject hitBoxAttack; //Collider que simula el ataque
    

    

    [SerializeField] private List<Transform> wayPoints; //Lista de waypoints solo se necesita el container hijo para poder poner los waypoints


    private void Awake() { //Al instanciarse:
        isFollowing = false; //No va a empezar siguiendo al jugador
        canAttack = false; //No va a poder atacar
        isHurt = false;
        if(moveWithWayPoints){//Si se marco la opcion de mover con wayPoints:
            wayPointsSettings();//configuro los waypoints
            nextWayPoint = 0;//Inicializo el primer waypoint
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        staffPrimaryDamageMult = weaponSO.staffPrimaryMaxDamage / weaponSO.staffPrimaryMaxSize; //Variable para calcular el daño del disparo principal segun cuanto se cargue 

        hitBoxAttack = transform.GetChild(0).gameObject; //Obtengo la hitbox de ataque
        hitBoxAttack.SetActive(false); //Desactivo la hitbox de ataque


        player = GameObject.Find("Player");//Asigno el jugador a su variable

        if(vars.hearRange > vars.hearRunRange){//Si vars.hearRange es mayor a vars.hearRunRange
            Debug.LogError("vars.hearRange no debe ser menor a vars.hearRunRange");//Mostrar cartel de error
        }
        if(vars.runSpeed < vars.speed){  //Si la velocidad al correr es menor a la velocidad:
            Debug.LogError("la velocidad al correr debe ser mayor o igual a la velocidad normal"); //Mostrar Error
        }
        if(vars.followRange < vars.hearRange || vars.followRange < vars.hearRunRange){ //Si el rango a perseguir, es menor al de escuchar, o menor al de escuchar correr:
            Debug.LogError("El rango para perseguir debe ser mayor a los rangos para escuchar");//Mostrar Error
        }
        if(vars.timeToSetAttack >= vars.timeToQuitAttack){ //Si el tiempo para que aparezca el ataque es mayor o igual al tiempo para quitarlo
            Debug.LogError("El tiempo para poner el ataque no debe ser ni mayor ni igual al tiempo para quitarlo"); //Mostrar Error
        }
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
       
        timerAttack += Time.deltaTime; //Activo el timer
        if(moveWithWayPoints){//Si la lista de waypoints no esta vacia y se decidio moverse por wayPoints:
            wayPointsMove();//me muevo por waypoints
        }
        Ears();//Funcion para escuchar cuando el jugador este cerca
        Follow();//Funcion para seguir al jugador
        
        Death();//Funcion para animar la muerte, y eliminar el gameObject
        
        Attack(); //Funcion de ataque
        if(isDeath){ //Si el enemigo se murio
            moveWithWayPoints = false; //No me muevo
            isFollowing = false; //No persigo
            canAttack = false; //No puedo atacar
        }
        if(isHurt && !isDeath){
            isFollowing = true;
        }
    }
    private void FixedUpdate() {
        if(!moveWithWayPoints){
            transform.LookAt(player.transform.position);
        }    
    }
 

    private void wayPointsSettings(){//Funcion para facilitar la creacion de los waypoints
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
            transform.position = Vector3.MoveTowards(transform.position,wayPoints[nextWayPoint].position, vars.speed * Time.deltaTime); //Me muevo hacia el siguiente waypoint
        }
    }

    void Ears(){
        if(Input.GetKey(KeyCode.LeftShift)){ //Si el jugador esta corriendo:
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))//Si se pulsa alguna tecla para mover, saltar o disparar:
            {
                if(Vector3.Distance(transform.position,player.transform.position) <= vars.hearRunRange){ //Si la distancia entre el jugador es menor o igual a vars.hearRange:
                    if(Vector3.Distance(transform.position,player.transform.position) > vars.keepDistance){ //Si la distancia entre el jugador es mayor vars.keepDistance:
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
        if(Vector3.Distance(transform.position,player.transform.position) <= vars.hearRange){ //Si la distancia entre el jugador es menor o igual a vars.hearRange:
            if(Vector3.Distance(transform.position,player.transform.position) > vars.keepDistance){ //Si la distancia entre el jugador es mayor a vars.keepDistance:
                isFollowing = true; //El enemigo va a perseguir al jugador
                moveWithWayPoints = false; //Dejo de moverme con wayPoints
            }
           
        }
         else{//Sino:
                isFollowing = false;//Dejo de perseguir
                if(wayPoints.Count > 0){ //Si la lista de waypoints no esta vacia:
                    moveWithWayPoints = true; //me  muevo con waypoints
                }
            }
    }

    protected virtual void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        //Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isFollowing && !isDeath){//Si se esta persiguiendo al jugador:
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            moveWithWayPoints = false;//Dejo de moverme con wayPoints
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, vars.runSpeed * Time.deltaTime); //Sigo al objetivo
        }

        if(Vector3.Distance(transform.position,player.transform.position) <= vars.keepDistance){//Si la distancia entre el jugador es menor o igual a vars.keepDistance y me estoy moviendo:
            isFollowing = false; //Dejo de moverme 
            if(timerAttack > vars.waitAttack){ //Si el timer llega a su wait:
                canAttack = true; //Activo el ataque
                timerAttack = 0; //Reinicio el timer
            }
        }
        if(Vector3.Distance(transform.position,player.transform.position) > vars.followRange){//Si la distancia con el jugador es mayor al rango para perseguir:
            isFollowing = false;//dejo de perseguir
            canAttack = false;//No puedo atacar
            if(wayPoints.Count > 0){//Si la lista de wayPoints no esta vacia:
                moveWithWayPoints = true;//Me muevo con wayPoints
            }
        }

    }
      
    protected virtual void Attack(){
        if(canAttack){   //Si puede atacar:
            if(timerAttack >= vars.timeToSetAttack){ //Si el timer es mayor o igual al tiempo para poner el ataque:
                hitBoxAttack.SetActive(true); //Activo el collider de ataque
            }
            if(timerAttack >= vars.timeToQuitAttack){ //Si es mayor o igual al tiempo para quitarlo:
                hitBoxAttack.SetActive(false); //Desactivo el collider de ataque
            }
        }

    }
    protected virtual void Death(){
        if(health <= 0){ //Si la vida es menor o igual a 0:
            isDeath = true;
            Destroy(GetComponent<Collider>());
            GetComponent<Rigidbody>().isKinematic = true;
            timerDead += Time.deltaTime; //Activo el timer de muerte
            if(timerDead >= vars.waitTimeDead){ //Si el timer llega a vars.waitTimeDead:
                Destroy(transform.gameObject); //Me elimino de la escena
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("WayPoint") && moveWithWayPoints) //Si entro a un waypoint:
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
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false;  //No se mueve por way points
                isHurt = true;
                Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "StaffSecondary BulletHole(Clone)": //Si recibe la bala secundaria del arma principal:
                health -= weaponSO.staffSecondaryDamage; //Pierde salud en base al daño de la bala
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false; //No se mueve por way points
                isHurt = true;
                Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "ShotgunSwordSecondary BulletHole(Clone)": //Si recibe la bala principal del arma principal:
                isFollowing = true; //Empieza a perseguir al jugador
                moveWithWayPoints = false; //No se mueve por way points
                health -= weaponSO.sSwordSecondaryDamage; //Pierde salud en base al daño de la bala
                isHurt = true;
                Debug.Log($"Enhealth is now at: {health}"); //Mensaje con la vida del enemigo
                break;
        }
    }
}
