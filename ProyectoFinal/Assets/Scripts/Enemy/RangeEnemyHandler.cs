using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyHandler : EnemyHandler
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weapon;
   protected virtual void Attack(){
        if(canAttack){   //Si puede atacar:
                if(timerAttack >= vars.timeToSetAttack){ //Si el timer es mayor o igual al tiempo para poner el ataque:
                    Instantiate(bullet,weapon);
                }
                if(timerAttack >= vars.timeToQuitAttack){ //Si es mayor o igual al tiempo para quitarlo:
                    canAttack = false;
                }
        }
    }
}
