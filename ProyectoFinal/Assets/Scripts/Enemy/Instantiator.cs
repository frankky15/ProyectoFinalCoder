using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] private GameObject objectToInstantiate;
    private List<GameObject> instantiatePoints = new List<GameObject>();

    private void Start() {
        int maxPoints = this.gameObject.transform.childCount;
        for (var i = 0; i < maxPoints; i++)
        {
            instantiatePoints.Add(this.gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void SpawInPoint(){
        int i = 0;
        foreach (var GameObject in instantiatePoints)
        {
            Instantiate(objectToInstantiate, instantiatePoints[i].transform);
            i++;
        }
    }
}
