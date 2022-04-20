using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySpider : SpiderHandler
{
    [SerializeField] int pointsForScare = 2000;
    [SerializeField] float waitTimeOnScare = 5f;
    private float timerOnScare = 0;
    private bool isScared;
    protected override void Update(){
        if(isScared){
            timerOnScare += Time.deltaTime;
            if(timerOnScare <= waitTimeOnScare){
                // transform.position = Vector3.MoveTowards(transform.position,-player.transform.position,vars.runSpeed* Time.deltaTime);
                rigidbody.MovePosition(transform.position - transform.forward * vars.runSpeed * Time.deltaTime);
            }
            else{
                isScared = false;
            }

        }
        else{
            base.Update();
        }
    }
    public override void OnEnemyDeathReaction(Vector3 enemyDeathPosition){
        if(Vector3.Distance(transform.position,enemyDeathPosition) <= vars.visionRange){
            isScared = true;
        }
    }
}
