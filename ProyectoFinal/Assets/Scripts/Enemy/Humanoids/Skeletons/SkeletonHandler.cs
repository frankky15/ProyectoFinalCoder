using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandler : MeleHumanoidHandler
{
    protected int attack = 0;
    [SerializeField] GameObject[] bodyPartsThatCanBeGetDown;
    [SerializeField] float initialHealthBodyParts = 30;

    [SerializeField] float waitToGetDown = 0.9f;
    [SerializeField] float waitToDissapeir = 5f;

    bool thereIsAPartGettingDown = false;
    bool thereIsAPartOnTheFloor = false;
    float timerToGetDown = 0;
    float timerToDissapeir = 0;

    List<bool> partsOnTheFloor;
    Dictionary<GameObject,float> bodyParts = new Dictionary<GameObject, float>();
    /*protected override void Start(){
        int i = 0;
        base.Start();
        foreach (var item in bodyPartsThatCanBeGetDown)
        {
            bodyParts.Add(bodyPartsThatCanBeGetDown[0],initialHealthBodyParts);
            partsOnTheFloor.Add(false);
            i++;
        }
    }*/
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
            if(timerAttack == 0){
                attack = Random.Range(0,2);
                switch(attack){
                    case 0:
                        animator.SetTrigger("Attack");
                        break;
                    case 1:
                        animator.SetTrigger("Attack1");
                        break;
                    case 2:
                    animator.SetTrigger("Attack2");
                        break;
                    default:
                        animator.SetTrigger("Attack");
                        Debug.LogWarning("variable attack no esta dentro del rango especificado");
                        break;
                }
            }
        }
        if(isHurt){
            animator.SetTrigger("Hurt");
        }
        if(isDeath){
            animator.SetTrigger("Death");
        }
    }
}
