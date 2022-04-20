using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BillboardScript))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] protected EnemyVar vars;

    [SerializeField] private Transform eyes; //Transform desde el que se va a tirar un rayCast para que detecte al jugador
    [SerializeField] protected ProjectileScriptableObject pyroPrimary;
    [SerializeField] protected ProjectileScriptableObject staffPrimary;
    [SerializeField] protected ProjectileScriptableObject staffSecondary;

    [SerializeField] private float revolverDamage = 60f; 
    //El daño del revolver y de la escopeta son las ultimas añadidas asi que aun no le hemos puesto daño en sus scriptableObjects, asi que por ahora les usamos el daño asi
    [SerializeField] private float shotgunDamage = 40f;
    [SerializeField] protected LayerMask layersDetect = new LayerMask();
    
    [System.NonSerialized]public float health = 300; //Salud de la araña, la inicializo en un valor mayor a 0 solo para que no muera al inicio

    [System.NonSerialized] public float damage = 1;

    private Vector3 initialPosition;
    public event Action<Vector3> OnEnemyDeath;    
    private float timerDead = 0; //Timer para saber cuanto dura la animacion de muerte
    protected float timerAttack = 0;//Timer para el vars.waitAttack
    protected float timerMove = 0;
    
    protected float timerDetect = 0;

    [SerializeField] float waitTimeDetect = 10f;
    [SerializeField] float waitRotate = 1f;

    [SerializeField] float waitMove = 1f;

    [SerializeField] private float staffPrimaryDamageMult; //Daño de la bala del disparo cargado

    [SerializeField] protected bool playerDetected = false;
    [SerializeField] protected bool isFollowing;//Variable para saber si se esta siguiendo al jugador
    [SerializeField] float footstepInterval = 0.1f;

    [SerializeField] PlayAudio playAudio;
    
    protected bool canAttack = false;//Variable para saber cuando se puede atacar
    protected bool randomMove = true;//Variable para saber si se mueve con wayPoints

    protected bool isHurt = false; 
    protected bool isDeath = false;

    [SerializeField] protected GameObject player;//El jugador
    protected GameObject hitBoxAttack; //Collider que simula el ataque

    protected Vector3 rayCastDirection; //Direccion del rayo

    protected RaycastHit hit; //Variable para el raycast al moverse
    protected RaycastHit hitOnWall;

    protected Animator animator;//Variable q sea utilizada por la mayoria de los scripts hijos

    private GameObject modelContainter;

    //protected BillboardScript lookPlayer;
    protected System.Random rnd = new System.Random();

    protected Rigidbody rigidbody;

    private GameObject particles;
    private void Awake() { //Al instanciarse:
        int i= 0;
        randomMove = false; //Empieza moviendose hacia cualquier lado
        playerDetected = false;
        isFollowing = false; //No va a empezar siguiendo al jugador
        canAttack = false; //No va a poder atacar
        isHurt = false;
        health = vars.health;
        damage = vars.damage;
        initialPosition = transform.position;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        //cameraTransform = GameObject.Find("Main Camera").transform;
        animator = GetComponent<Animator>();
        EnemyHandler[] enemies = GameObject.FindObjectsOfType<EnemyHandler>(); //Busco todos los scripts de enemigos para el evento OnEnemyDeath
        //lookPlayer = GetComponent<BillboardScript>();
        //lookPlayer.enabled = false;
        int i = 0;
        //transform.parent = GameObject.Find("EnemiesContainer").transform;
        hitBoxAttack = transform.GetChild(0).gameObject; //Obtengo la hitbox de ataque
        if(eyes == null){
            eyes = transform.GetChild(1);
        }
        modelContainter = transform.GetChild(2).gameObject;
        particles = transform.GetChild(3).gameObject;
        particles.SetActive(false);

        //OnEnemyDeath += GameObject.Find("UI").GetComponent<HUDManager>().morePoints;
        //player = GameObject.Find("ProtoPlayer");//Asigno el jugador a su variable
        foreach (var EnemyHandler in enemies)
        {
            OnEnemyDeath += enemies[i].OnEnemyDeathReaction;
            i++;
        }
        Debug.Log("PointsManager del UI se suscribio al evento de OnEnemyDeath");
        Debug.Log("OnEnemyDeathCloseReaction se suscribio al evento de OnEnemyDeath");

        hitBoxAttack.SetActive(false); //Desactivo la hitbox de ataque



        
        if(vars.runSpeed < vars.speed){  //Si la velocidad al correr es menor a la velocidad:
            Debug.LogError("la velocidad al correr debe ser mayor o igual a la velocidad normal"); //Mostrar Error
        }
        if(vars.timeToSetAttack >= vars.timeToQuitAttack){ //Si el tiempo para que aparezca el ataque es mayor o igual al tiempo para quitarlo
            Debug.LogError("El tiempo para poner el ataque no debe ser ni mayor ni igual al tiempo para quitarlo"); //Mostrar Error
        }
        if(DungeonManager.Instance.finished){
            transform.parent = GameObject.Find("EnemiesContainer").transform;
            Debug.Log("Bien instanciado");
        }
        else{
            Debug.LogError("Mal instanciado");
        }
        StartCoroutine(Footsteps());

        //randomMove = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.DrawRay(eyes.position,rayCastDirection,Color.white);

       if(canAttack){
            timerAttack += Time.deltaTime; //Activo el timer
       }
       
        //StateMachine();
        
        
        Death();//Funcion para animar la muerte, y eliminar el gameObject
        
        Attack(); //Funcion de ataque


        if(isHurt){ //Si el enemigo no detecto al jugador y esta herido:
            //Debug.Log("Look at the player");
            LookAtPlayer();
        }
        //Debug.Log(hit.collider);
        if(hit.collider != null){
            if(hit.collider.CompareTag("Player")){
                playerDetected = true;
            }
        }
        if(transform.position.y <= -9999){
            //transform.position = initialPosition;
            //transform.position = new Vector3(0f,transform.position.y,0f);
            Destroy(this.gameObject);
        }
        

        if(Physics.Raycast(eyes.position,eyes.transform.forward,out hit, vars.visionOnWallRange,layersDetect)){
            if(hit.collider != null){
                //Debug.Log(hit.collider);
                if(hit.transform.CompareTag("Player")){
                   Debug.DrawRay(eyes.transform.position,rayCastDirection,Color.green); //Se muestra en el editor un raycast
                }
                else{
                    Debug.DrawRay(eyes.transform.position,rayCastDirection,Color.red); //Se muestra en el editor un raycast
                }
            }
        }
        Eyes();//Funcion para detectar cuando el jugador este cerca
        Ears();
        if(Vector3.Distance(transform.position,player.transform.position) < 50f){
            randomMove = false;
        }
       if(randomMove){
           if(Physics.Raycast(eyes.position,eyes.transform.forward,out hitOnWall, vars.visionRange,layersDetect)){
                if(hitOnWall.collider != null){
                    
                }
                if(timerMove >= waitMove);
                    RandomMovement();
            }
            
        }
    }
    private void FixedUpdate() {
        
        
        if(randomMove && !isHurt && !playerDetected){//Si se esta moviendo random, y no se esta herido, y no se detecto al player;
            timerMove += Time.deltaTime;
        }
        else if(playerDetected){
            //Debug.Log("looking player");
            //lookPlayer.enabled = true;
            randomMove = false;
            LookAtPlayer();
            //transform.rotation = Quaternion.Euler(0f,eulerRotation.y,0f);
            //transform.localRotation = Quaternion;
            //transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position,Vector3.up);
            
            Follow();//Funcion para seguir al jugador
        }
        
        rayCastDirection = player.transform.position - eyes.position; //direccion del raycast para detectar al jugador
        
    }
 
    protected virtual void LookAtPlayer(){
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
        

    protected virtual void RandomMovement(){
        // Debug.DrawRay(eyes.transform.position,Vector3.forward,Color.green); //Se muestra en el editor un raycast

        // transform.Translate(Vector3.forward * Time.deltaTime * vars.speed);
        rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * vars.speed);

        /*if(hitOnWall.transform.tag != null){
            if(hitOnWall.transform.tag != player.transform.tag){
                float newDirection = rnd.Next(0,380);
                //Debug.Log(newDirection);
                Quaternion newRotation = Quaternion.Euler(0f,newDirection,0f);//Creo una rotacion
                transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
            }   
        }*/
        if(timerMove >= waitRotate){
                float newDirection = rnd.Next(0,380);
                //Debug.Log(newDirection);
                Quaternion newRotation = Quaternion.Euler(0f,newDirection,0f);//Creo una rotacion
                transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
                timerMove = 0;
            }
    }

    void Eyes(){
        if(Physics.Raycast(eyes.position,rayCastDirection,out hit, vars.visionRange,layersDetect)){
            if(hit.collider != null){
                //Debug.Log(hit.collider);
                if(hit.transform.tag == player.transform.tag || hit.transform.gameObject.layer == 3){
                    //Debug.Log("player found it");
                    playerDetected = true;
                    randomMove = false;//Dejo de moverme al azar
                }
                else if(playerDetected){
                    timerDetect += Time.deltaTime;
                    if(timerDetect >= waitTimeDetect){
                        playerDetected = false;
                        randomMove = true;
                    }
                    //Debug.DrawRay(eyes.transform.position,rayCastDirection,Color.blue);
                }
                else{
                   // Debug.DrawRay(eyes.transform.position,rayCastDirection,Color.blue);
                }
            }
            else{
                playerDetected = false;
                //randomMove = true;
            }
            /*else{
                isFollowing = false;
            }*/
        }
        /*else{
            playerDetected = false;
            Debug.Log("No hay raycast");
        }*/
    }
    void Ears(){
        if(Vector3.Distance(transform.position,player.transform.position) <= vars.hearRange){
            playerDetected = true;
            randomMove = false;
            isFollowing = true;
        }
    }

    protected virtual void Follow(){ //Despues agregar un raycast para que la IA esquive las paredes
        //Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isFollowing && !isDeath){//Si se esta persiguiendo al jugador:
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            if(Vector3.Distance(transform.position,player.transform.position) > vars.keepDistance){
                // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, vars.runSpeed * Time.deltaTime); //Sigo al objetivo
                rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * vars.speed);
            }
        }

        if(Vector3.Distance(transform.position,player.transform.position) <= vars.keepDistance){//Si la distancia entre el jugador es menor o igual a vars.keepDistance y me estoy moviendo:
            isFollowing = false;
            canAttack = true; //Activo el ataque
            //Debug.Log("La distancia es menor");
        }
        else{
            isFollowing = true;
            canAttack = false;
        }
        if(Vector3.Distance(transform.position,player.transform.position) > vars.followRange){//Si la distancia con el jugador es mayor al rango para perseguir:
            isFollowing = false;//dejo de perseguir
            canAttack = false;//No puedo atacar
        }

    }
      
    protected virtual void Attack(){
        //Funcion vacia q sera tomada por los hijos
    }
    protected virtual void Death(){
        if(health <= 0){ //Si la vida es menor o igual a 0:
            isDeath = true;
            
            Destroy(GetComponent<Collider>());
            modelContainter.SetActive(false);
            particles.SetActive(true);
            //rigidbody.isKinematic = true;
            Vector3 positionOnDeath = transform.position;
            timerDead += Time.deltaTime; //Activo el timer de muerte
            if(this.gameObject != null && timerDead == 0){
                OnEnemyDeath?.Invoke(positionOnDeath);

            }
            if(timerDead >= vars.waitTimeDead){ //Si el timer llega a vars.waitTimeDead:
                
                Destroy(transform.gameObject); //Me elimino de la escena
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision other) {
        //Debug.Log(other.transform.name);
        if(other.transform.CompareTag("Bullet") || other.transform.gameObject.layer == 9){
            //Debug.Log(other.transform.name);
             switch (other.transform.name)
            {
            case "StaffPrimaryProjectile(Clone)": //Si recibe la bala principal del arma principal:
                health -= (staffPrimary.damage + staffPrimaryDamageMult * other.transform.localScale.y); //Pierde salud en base al daño de la bala y tamaño de la bala
                randomMove = false;  //No se mueve por way points
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "StaffSecondaryEnemy(Clone)": //Si recibe la bala secundaria del arma principal:
                health -= staffSecondary.damage; //Pierde salud en base al daño de la bala
                randomMove = false; //No se mueve por way points
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "PyroHandSecondaryEnemy(Clone)":
                health -= pyroPrimary.damage;
                randomMove = false;
                isHurt = true;
                break;
            case "ShotgunEnemyHit(Clone)":
                health -= shotgunDamage;
                randomMove = false;
                isHurt = true;
                break;
            case "RevolverEnemyHit(Clone)":
                health -= revolverDamage;
                randomMove = false;
                isHurt = true;
                break;
            /*case "ShotgunSwordSecondary BulletHole(Clone)": //Si recibe la bala principal del arma principal:
                randomMove = false; //No se mueve por way points
                health -= weaponSO.sSwordSecondaryDamage; //Pierde salud en base al daño de la bala
                isHurt = true;
                Debug.Log($"Enhealth is now at: {health}"); //Mensaje con la vida del enemigo
                break;*/
            default:
                randomMove = false; //No se mueve por way points
                Debug.Log("Bala no reconocida");
                health -= 40; //Pierde salud en base al daño de la bala
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}"); //Mensaje con la vida del enemigo
                break;
            }
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.name);
        if(other.transform.CompareTag("Bullet") || other.transform.gameObject.layer == 9){
             switch (other.transform.name)
            {
            case "StaffPrimaryProjectile(Clone)": //Si recibe la bala principal del arma principal:
                health -= (staffPrimary.damage + staffPrimaryDamageMult * other.transform.localScale.y); //Pierde salud en base al daño de la bala y tamaño de la bala
                randomMove = false;  //No se mueve por way points
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "StaffSecondaryEnemy(Clone)": //Si recibe la bala secundaria del arma principal:
                health -= staffSecondary.damage; //Pierde salud en base al daño de la bala
                randomMove = false; //No se mueve por way points
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}");//Mensaje con la vida del enemigo
                break;
            case "PyroHandSecondaryEnemy(Clone)":
                health -= pyroPrimary.damage;
                randomMove = false;
                isHurt = true;
                break;
            case "ShotgunEnemyHit(Clone)":
                health -= shotgunDamage;
                randomMove = false;
                isHurt = true;
                break;
            case "RevolverEnemyHit(Clone)":
                health -= revolverDamage;
                randomMove = false;
                isHurt = true;
                break;
            /*case "ShotgunSwordSecondary BulletHole(Clone)": //Si recibe la bala principal del arma principal:
                randomMove = false; //No se mueve por way points
                health -= weaponSO.sSwordSecondaryDamage; //Pierde salud en base al daño de la bala
                isHurt = true;
                Debug.Log($"Enhealth is now at: {health}"); //Mensaje con la vida del enemigo
                break;*/
            default:
                randomMove = false; //No se mueve por way points
                Debug.LogWarning("Bala no reconocida");
                health -= 40; //Pierde salud en base al daño de la bala
                isHurt = true;
                //Debug.Log($"Enhealth is now at: {health}"); //Mensaje con la vida del enemigo
                break;
            }
        }
    }
    public virtual void OnEnemyDeathReaction(Vector3 enemyDeathPosition){
        //Funcion vacia, que sera reescrita en los scripts hijos
    }
    private IEnumerator Footsteps()
    {
        while (true)
        {
            playAudio.PlayAudioOneShot(rnd.Next(0, 3));
            // Debug.Log("audio");
            yield return new WaitForSeconds(footstepInterval);
        }
    }
    protected virtual void StateMachine(){
        if(!isDeath){
            if(randomMove && !playerDetected){
                isFollowing = false;
                canAttack = false;
            }
            if(playerDetected && !canAttack){
                isFollowing = true;
                randomMove = false;
            }
            if(canAttack || playerDetected){
               randomMove = false;
            }
            if(!playerDetected && isHurt){
                randomMove = false;
            }
        }
        else{
            isFollowing = false;
            canAttack = false;
            playerDetected = false;
            randomMove = false;
        }
    }
    
}
