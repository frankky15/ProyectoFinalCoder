using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGray : SpiderHandler
{
    // Update is called once per frame
    [SerializeField] float jumpOnImpressed = 3f;

    public override void OnEnemyDeathReaction(Vector3 enemyDeathPosition)
    {
       // transform.position = new Vector3(transform.position.x,transform.position.y + jumpOnImpressed, transform.position.z);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.CompareTag("Player")){
            player.GetComponent<PlayerHandler>().OnSlowed(vars.timeEffect,vars.effectInstensity);
        }
    }
}
