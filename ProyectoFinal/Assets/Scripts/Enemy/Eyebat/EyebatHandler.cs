using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class EyebatHandler : MeleEnemyHandler
{
    [SerializeField] WingsState wing1;
    [SerializeField] WingsState wing2;
    bool wing1isHurt = false;
    bool wing2isHurt = false;

    //Animator animator;
   //Rigidbody rigidbody;
    protected override void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        
        base.Start();
        rigidbody.useGravity = false;
    }
    protected override void Update()
    {
        base.Update();
        wing1isHurt = wing1.wingIsHurt;
        wing2isHurt = wing2.wingIsHurt; 
        SetAnimations();
        hurtWings();
    }
    protected void hurtWings(){
        if((wing1isHurt && !wing2isHurt) || (!wing1isHurt && wing2isHurt)){
            rigidbody.useGravity = true;
        }
        else if(wing1isHurt && wing2isHurt){
            
        }
    }
    private void SetAnimations(){ 
        if(canAttack){   //Si puede atacar:
            if(timerAttack >= vars.timeToSetAttack && timerAttack <= vars.timeToQuitAttack){ //Si el timer es mayor o igual al tiempo para poner el ataque:
                animator.SetTrigger("attack_01");
            }
            /*if(timerAttack >= vars.timeToQuitAttack){ //Si es mayor o igual al tiempo para quitarlo:
                hitBoxAttack.SetActive(false); //Desactivo el collider de ataque
                animator.SetTrigger("attack_01",false);
            }*/
        }
        if(isFollowing && (!canAttack || !(timerAttack >= vars.timeToSetAttack && timerAttack <= vars.timeToQuitAttack))){
            animator.SetTrigger("run");
        }
        if(randomMove){
            animator.SetTrigger("walk");
        }
        if(isHurt){
            animator.SetTrigger("damage");
        }
        if(isDeath){
            animator.SetTrigger("die");
        }
    }

}
