using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsState : MonoBehaviour
{
     public bool wingIsHurt = false;
    public void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Bullet")){
            wingIsHurt = true;
        }
    }   
}
