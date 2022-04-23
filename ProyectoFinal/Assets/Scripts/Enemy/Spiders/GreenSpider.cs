using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSpider : SpiderHandler
{
    // Start is called before the first frame update
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.CompareTag("Player")){
            player.GetComponent<PlayerHandler>().OnSlowed(vars.timeEffect,vars.effectInstensity);
        }
    }
}
