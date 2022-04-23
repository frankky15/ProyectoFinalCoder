using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] float[] probabilitySpawn;

    
    EnemiesLeftOnScene enemiesLeft;
    public bool instantiated;

    float probability = 100;
    private void Start() {
        if(probabilitySpawn.Length != enemies.Length){
            Debug.LogWarning("La cantidad de probabilidad y de enemigos al instanciarse debe ser la misma");
        }
        probability = Random.Range(0,100);
        Debug.Log("Dado: " + probability + "probabilidad" + probabilitySpawn[0]);
        /*if(GameObject.Find("EnemiesLeft") != null){
            if(GameObject.Find("EnemiesLeft").GetComponent<EnemiesLeftOnScene>() != null){
                enemiesLeft = GameObject.Find("EnemiesLeft").GetComponent<EnemiesLeftOnScene>();
            }
            
        }*/
        
    }

    private void Update() {
        Spawn();
    }
    public void Spawn(){
        if(DungeonManager.Instance.finished){
            int i = 0;
            
            do
            {
                //Debug.Log("Probabilidad de spawn" + probabilitySpawn);
                if(probability < probabilitySpawn[i]){
                    Debug.Log("i " + i);
                    Instantiate(enemies[i],transform);
                    //Debug.Log("true");
                    instantiated = true;
                    Destroy(this);
                }
                else{
                    i++;
                    probability = Random.Range(0,100);
                }
            } while(instantiated == false && i < enemies.Length);
        }
    }
}
