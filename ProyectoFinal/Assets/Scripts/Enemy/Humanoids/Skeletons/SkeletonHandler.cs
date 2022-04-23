using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandler : MeleHumanoidHandler
{
    protected int attack = 0;
    int lastAttack = 2;
    [SerializeField] GameObject[] bodyPartsThatCanBeGetDown;
    [SerializeField] float initialHealthBodyParts = 30;

    [SerializeField] float waitToGetDown = 0.9f;
    [SerializeField] float waitToDissapeir = 5f;

    bool thereIsAPartGettingDown = false;
    bool thereIsAPartOnTheFloor = false;
    float timerToGetDown = 0;
    float timerToDissapeir = 0;

    bool alreadyAttack = false;

    List<bool> partsOnTheFloor;
    Dictionary<GameObject,float> bodyParts = new Dictionary<GameObject, float>();
    protected override void Start(){
        base.Start();
        attack = 0;
        /*int i = 0;
        foreach (var item in bodyPartsThatCanBeGetDown)
        {
            bodyParts.Add(bodyPartsThatCanBeGetDown[0],initialHealthBodyParts);
            partsOnTheFloor.Add(false);
            i++;
        }*/
    }
    protected override void Update()
    {
        base.Update();
        if(canAttack){
            isFollowing = false;
        }
        else if(playerDetected && timerAttack >= vars.timeToQuitAttack || timerAttack <= vars.timeToSetAttack){
            isFollowing = true;
        }
        /*if(thereIsAPartGettingDown){
            timerToGetDown += Time.deltaTime;
        }
        if(timerToGetDown >= waitToGetDown){
            thereIsAPartGettingDown = false;
            thereIsAPartOnTheFloor = true;
            timerToGetDown = 0;
        }
        if(thereIsAPartOnTheFloor){
            timerToDissapeir += Time.deltaTime;
        }
        if(timerToDissapeir >= waitToDissapeir){
            int i = 0;
            foreach (var item in bodyPartsThatCanBeGetDown)
            {
                if(partsOnTheFloor[i]){
                    Destroy(bodyPartsThatCanBeGetDown[i]);
                }
                i++;
            }
        }*/
        SetAnimations();
    }
    /*protected override void OnTriggerEnter(Collider other){
        base.OnTriggerEnter(other);
        if(other.CompareTag("Bullet") || other.gameObject.layer == 9){
            Debug.Log("Hurt");
            int i = 0;
            foreach (var item in bodyPartsThatCanBeGetDown)
            {
                if(other.transform.position == bodyPartsThatCanBeGetDown[i].transform.position){
                    bodyParts[bodyPartsThatCanBeGetDown[i]] -= vars.damage * 3;
                    if(bodyParts[bodyPartsThatCanBeGetDown[i]] <= 0 && !thereIsAPartGettingDown){
                        bodyPartsThatCanBeGetDown[i].AddComponent<Rigidbody>();
                        thereIsAPartGettingDown = true;
                    }
                }
                else{
                    i++;
                }
            }
        }
    }*/
    private void SetAnimations()
    {
        if(randomMove){
            animator.SetTrigger("Walk");
        }
        if(isFollowing){
            animator.SetTrigger("Run");
            if(Input.GetKeyDown(KeyCode.Space)){
                animator.SetTrigger("Jump");
            }
        }
        if(canAttack){
            Debug.Log("attack" +attack);
            if(attack <= 0) animator.SetTrigger("Attack");
            else if(attack >= 1) animator.SetTrigger("Attack1");
            /*else if(attack >= 2){
                animator.SetTrigger("Attack2");
            }*/
            else{
                attack = 0;
            }
        }
        if(attack > 2 && timerAttack < vars.timeToSetAttack){
            attack = 0;
        }
            
        if(isHurt){
            animator.SetTrigger("Hurt");
            isHurt = false;
        }
        if(isDeath){
            animator.SetTrigger("Death");
        }
    }
    private void SetAttackWithAnimation(){
        attack++;
        Debug.Log(attack);
    }
}
