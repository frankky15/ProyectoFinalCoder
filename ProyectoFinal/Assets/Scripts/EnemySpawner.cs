using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float[] probabilitySpawn;
    
    EnemiesLeftOnScene enemiesLeft;
    public bool instantiated;

    private void Start() {
        if(GameObject.Find("EnemiesLeft") != null){
            Debug.Log("Game Object encontrado");
            if(GameObject.Find("EnemiesLeft").GetComponent<EnemiesLeftOnScene>() != null){
                Debug.Log("Componente encontrado");
                enemiesLeft = GameObject.Find("EnemiesLeft").GetComponent<EnemiesLeftOnScene>();
            }
            
        }
        
    }

    private void Update() {
        Spawn();
    }
    public void Spawn(){
        if(DungeonManager.Instance.finished){
            int i = 0;
            if(instantiated == false && i < enemies.Length)
            {
                float probability = Random.Range(1,100);
                if(probability <= probabilitySpawn[i]){
                    Instantiate(enemies[i],transform);
                    //Debug.Log("true");
                    instantiated = true;
                }
                i++;
            }
        }
    }
}
