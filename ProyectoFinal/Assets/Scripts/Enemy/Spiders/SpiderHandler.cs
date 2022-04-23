using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHandler : MeleEnemyHandler
{
    [SerializeField] float timeOnFire = 1f;
    [SerializeField] float damagePerFire = 2f;

  

    private float timerOnFire = 0;
    private bool isOnFire = false;

    private AnimationsController animatorController; //Script que activa las animaciones
    //public float damage { get; private set;}

    private void Awake() {
        //damage = vars.damage;
    }
    protected override void Start(){
        animatorController = GetComponent<AnimationsController>(); //Obtengo las animaciones
        base.Start();
    }

    protected override void Update() {
        base.Update();
        SetAnimations();
        if(isOnFire){
            timerOnFire += Time.deltaTime;
            health -= damagePerFire;
        }
        if(timerOnFire >= timeOnFire){
            isOnFire = false;
            timerOnFire = 0;
        }
    }

   /*protected override void Follow(){
       base.Follow();
        if(isFollowing && !isDeath){
            if(Physics.Raycast(transform.position,rayCastDirection,out hit, vars.followRange,layersDetect)){//Creo el rayo
                if(hit.collider != null){ //Si el rayo golpea algo:
                    if(hit.transform.tag == player.transform.tag){ //Si el objeto golpeado tiene el mismo tag que el jugador:
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, vars.runSpeed * Time.deltaTime); //Sigo al objetivo
                        Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador
                        transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
                    } 
                    else{//Si no
                        if(Vector3.Distance(transform.position,hit.transform.position) <= vars.keepDistance){//Si la distancia con el objeto golpeado es menor o igual a vars.keepDistance:
                            transform.position = Vector3.MoveTowards(transform.position,Vector3.up, vars.runSpeed * Time.deltaTime); //Trepo el objeto
                        }
                        else{ //Sino
                            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, vars.runSpeed * Time.deltaTime); //Me sigo moviendo en direccion al jugador
                            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador
                            transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion                        }
                        }
                    }
           
                }
            }
        }
    }*/

    protected virtual void SetAnimations(){ 
        if(health <= 0){
            animatorController.SetDead(); //Activo la animacion de muerte
        }
        if(randomMove || isFollowing){
            animatorController._animateWhenRun = true; //Activo la animacion de moverse
        }
        if(canAttack){
            animatorController._animateWhenRun = false; //Cancelo la animacion de correr de la araña
            animatorController.Attack(); //Llamo a la funcion de animacion de ataque en el script de animacion
        }
        if(isHurt){
            animatorController.Hit(); //Hace una animacion para recibir el daño
            isHurt = false;
        }
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.GetComponent<Collider>() != null){
            if(other.transform.name == "PyroHandSecondaryEnemy(Clone)"){
                isOnFire = true;
            }
        }
    }

    
}
