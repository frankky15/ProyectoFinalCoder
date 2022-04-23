using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRed : SpiderHandler
{
    [SerializeField] float waitCalm = 2f;
    private float timerOnAngry = 0;
    private bool isAngry = false;
    
    protected override void Update() {
        if(isAngry){
            timerOnAngry += Time.deltaTime;
           // BetterFollow();
            BetterAttack();
            if(timerOnAngry >= waitCalm){
                isAngry = false;
            }
        }
        else{
            base.Update();
        }
    }
    public override void OnEnemyDeathReaction(Vector3 enemyDeathPosition){
        if(Vector3.Distance(transform.position,enemyDeathPosition) <= vars.visionRange){
            isAngry = true;
        }
    }
    /*private void BetterFollow(){
        //Debug.Log(Vector3.Distance(transform.position,player.transform.position));
        if(isFollowing && !isDeath){//Si se esta persiguiendo al jugador:
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            randomMove = false;//Dejo de moverme con wayPoints
            if(Physics.Raycast(transform.position,rayCastDirection,out hit, vars.followRange,layersDetect)){//Creo el rayo
                if(hit.collider != null){ //Si el rayo golpea algo:
                    if(hit.transform.tag == player.transform.tag){ //Si el objeto golpeado tiene el mismo tag que el jugador:
                        rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * vars.speed);                        Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador
                        transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion
                    } 
                    else{//Si no
                        if(Vector3.Distance(transform.position,hit.transform.position) <= vars.keepDistance){//Si la distancia con el objeto golpeado es menor o igual a vars.keepDistance:
                            transform.position = Vector3.MoveTowards(transform.position,Vector3.up, vars.runSpeed*2 * Time.deltaTime); //Trepo el objeto
                        }
                        else{ //Sino
                            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, vars.runSpeed*2 * Time.deltaTime); //Me sigo moviendo en direccion al jugador
                            Quaternion newRotation = Quaternion.LookRotation(player.transform.position - transform.position);//Creo una rotacion segun la posicion entre el jugador
                            transform.rotation = newRotation;//le asigno la nueva rotacion a mi rotacion                        }
                        }
                    }
           
                }
            }
        }

        if(Vector3.Distance(transform.position,player.transform.position) <= vars.keepDistance){//Si la distancia entre el jugador es menor o igual a vars.keepDistance y me estoy moviendo:
            isFollowing = false; //Dejo de moverme 
            if(timerAttack > vars.waitAttack/2){ //Si el timer llega a su wait:
                canAttack = true; //Activo el ataque
                timerAttack = 0; //Reinicio el timer
            }
        }
    }*/
    private void BetterAttack(){
        if(canAttack){   //Si puede atacar:
            if(timerAttack >= vars.timeToSetAttack/2){ //Si el timer es mayor o igual al tiempo para poner el ataque:
                hitBoxAttack.SetActive(true); //Activo el collider de ataque
            }
            if(timerAttack >= vars.timeToQuitAttack/2){ //Si es mayor o igual al tiempo para quitarlo:
                hitBoxAttack.SetActive(false); //Desactivo el collider de ataque
                canAttack = false;
                isFollowing = true;
            }
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.CompareTag("Player")){
            player.GetComponent<PlayerHandler>().OnPoisoned(vars.timeEffect,vars.effectInstensity);
        }
    }
}
