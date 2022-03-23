using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHandler : EnemyHandler
{
    private RaycastHit hit; //Variable para el raycast al moverse
    private Vector3 rayCastDirection; //Direccion del rayo
    private AnimationsController animator; //Script que activa las animaciones
    public float damage { get; private set;}

    private void Awake() {
        damage = vars.damage;
    }
    protected override void Start(){
        base.Start();
        animator = GetComponent<AnimationsController>(); //Obtengo las animaciones
    }

    protected override void Update() {
        base.Update();
        SetAnimations();
    }

   protected override void Follow(){
       base.Follow();
        Debug.DrawRay(transform.position,rayCastDirection,Color.red); //Se muestra en el editor un raycast
        rayCastDirection = player.transform.position - transform.position;
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
    }

    private void SetAnimations(){
        if(health <= 0){
            animator.SetDead(); //Activo la animacion de muerte
        }
        if(moveWithWayPoints || isFollowing){
            animator._animateWhenRun = true; //Activo la animacion de moverse
        }
        if(canAttack){
            animator._animateWhenRun = false; //Cancelo la animacion de correr de la araña
            animator.Attack(); //Llamo a la funcion de animacion de ataque en el script de animacion
        }
        if(isHurt){
            animator.Hit(); //Hace una animacion para recibir el daño
            isHurt = false;
        }
    }
}
