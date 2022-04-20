using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesLeftOnScene : MonoBehaviour
{
    public static EnemiesLeftOnScene instance;

    [SerializeField] GameObject enemiesContainer;
    [SerializeField] float enemiesPercent = 50f;

    [SerializeField] private int enemies;
    [SerializeField] private int enemiesCount = 0;

    [SerializeField] private float waitToAssign = 5.3f;

    float timer = 0;

    private bool alreadyAssigned = false;

    private bool alreadyRan = false;
    private bool allSpawn = false;

    // Start is called before the first frame update
    private void Awake() {
        enemiesPercent/= 100;
        alreadyAssigned = false;
    }
    void Start()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DungeonManager.Instance.finished){
            if(!alreadyAssigned){
                timer += Time.deltaTime;
            } 
            if(timer >= waitToAssign && !alreadyAssigned){
                enemiesCount = enemiesContainer.transform.childCount;
                alreadyAssigned = true;
            }
            enemies = enemiesContainer.transform.childCount;
            
            if (!alreadyRan && enemies <= enemiesCount*enemiesPercent && enemiesCount != 0 && alreadyAssigned)
            {
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
