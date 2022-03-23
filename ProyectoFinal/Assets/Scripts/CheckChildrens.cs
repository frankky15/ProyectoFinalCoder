using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckChildrens : MonoBehaviour
{
    [SerializeField] private UnityEvent thereIsNotChildren;
    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.childCount <= 0){
            thereIsNotChildren?.Invoke();
            Debug.Log("Un counteiner llamo a la funcion thereIsNotChildren");
        }       
    }
}
