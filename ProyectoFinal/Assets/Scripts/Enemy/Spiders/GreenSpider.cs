using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSpider : SpiderHandler
{
    [SerializeField] float slowTimeEffect = 4f;
    [SerializeField] float slowIntensity = 8f;

    [SerializeField] float poisonTimeEffect = 10f;
    [SerializeField] float poisonDamage = 1f;
    // Start is called before the first frame update
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.CompareTag("Player")){
            player.GetComponent<PlayerHandler>().OnSlowed(slowTimeEffect,slowIntensity);
            player.GetComponent<PlayerHandler>().OnPoisoned(poisonTimeEffect,poisonDamage);
        }
    }
}
