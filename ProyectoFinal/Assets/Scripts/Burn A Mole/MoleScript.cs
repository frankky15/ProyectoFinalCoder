using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleScript : MonoBehaviour
{
    private float speed;
    private int health;
    private void Start()
    {
        health = BurnAMoleScript.Instance.moleHealth;
    }
    private void Update()
    {
        speed = BurnAMoleScript.Instance.stayTime * 2f;
        if (gameObject.transform.position.y < 1f) gameObject.transform.position += Vector3.up * speed * Time.deltaTime;

        if (health < 1)
        {
            BurnAMoleScript.Instance.molesKilled ++;
            BurnAMoleScript.Instance.RingBell();
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.name == "PyroHandPrimary(Clone)") health --;
        if (other.name == "PyroHandPrimary(Clone)") Debug.Log("HitReg");;
    }
}
