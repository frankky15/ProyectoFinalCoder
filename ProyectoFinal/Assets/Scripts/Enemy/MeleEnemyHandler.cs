using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemyHandler : EnemyHandler
{
    protected override void Update()
    {
        base.Update();
        if(playerDetected){
            if(Vector3.Distance(transform.position,player.transform.position) <= vars.keepDistance){
                isFollowing = false;
            }
            else{
                isFollowing = true;
            }
        }
        
    }
    protected override void Attack(){
        if(canAttack){   //Si puede atacar:
            if(timerAttack >= vars.timeToSetAttack){ //Si el timer es mayor o igual al tiempo para poner el ataque:
                hitBoxAttack.SetActive(true); //Activo el collider de ataque
                isFollowing = true;
            }
            if(timerAttack >= vars.timeToQuitAttack){ //Si es mayor o igual al tiempo para quitarlo:
                hitBoxAttack.SetActive(false); //Desactivo el collider de ataque
                timerAttack = 0;
            }
        }
    }
}
