using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpesificZone : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAreaEnter;
    [SerializeField] private UnityEvent OnAreaExit;
    
    private void OnTriggerEnter(Collider other) {
       if (other.CompareTag("Player"))
       {
           OnAreaEnter?.Invoke();
           Debug.Log("Specific Zone invoco el evento OnAreaEnter");
       }
   }
   private void OnTriggerExit(Collider other) {
       if(other.CompareTag("Player")){
           OnAreaExit?.Invoke();
           Debug.Log("Specific Zone invoco OnAreaExit");
       }
   }
   
}
