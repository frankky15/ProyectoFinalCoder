using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class WayDownScript : MonoBehaviour
{
    [SerializeField] private GameObject wayDownGO;
    [SerializeField] private float spawnProbab = 30;
    private bool alreadyRan;
    private bool isActive;
    private void Awake()
    {
        if (Random.Range(0, 100) < spawnProbab) 
        {
            isActive = true;
            wayDownGO.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && isActive)
        {
            // aca tendria que hacer el prompt con un billboard ui que diga precionar la letra F para pasar de lvl o algo asi...

            if (Input.GetKeyDown(KeyCode.F) && !alreadyRan)
            {
                // Debug.Log("player pressed F");
                alreadyRan = true;
                StartCoroutine(Restart());
            }
        }
    }

    private IEnumerator Restart()
    {
        ScoreManager.Instance.AddScore();
        yield return new WaitForEndOfFrame();

        ScoreManager.Instance.SaveScore();
        yield return new WaitForEndOfFrame();

        LoadingScreenScript.Instance.ReloadLevel();
    }
}
