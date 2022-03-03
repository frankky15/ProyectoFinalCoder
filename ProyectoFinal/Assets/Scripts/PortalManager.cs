using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PortalManager : MonoBehaviour
{
    [SerializeField] private float portalTime = 4f;
    private float timeNow;
    [SerializeField] private string portalScene = "Forest"; 
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            timeNow = portalTime + Time.time;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && timeNow < Time.time)
        {
            SceneManager.LoadScene(portalScene);
        }
    }
}
