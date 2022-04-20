using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BillboardScript : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private float cameraSearchIn;
    [SerializeField] private bool alwaysVisible;
    [SerializeField] private float visibleDistance = 1f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject gameObjectToToggle;
    [SerializeField] private bool interactable;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private float interactionCooldown;
    [SerializeField] private UnityEvent onInteraction;
    private bool cameraFound;
    private bool collided;
    private bool hide;
    private float cooldown;

    private void Start()
    {
        StartCoroutine(CameraSearch());
    }
    private void Update()
    {
        collided = Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, visibleDistance, playerMask);
        if (!hide)
        {
            if (!alwaysVisible) MakeVisible();
            if (interactable) Interaction();
        }
    }
    private void LateUpdate()
    {
        if (cameraFound) transform.LookAt(cameraTransform.position);
    }

    private void MakeVisible()
    {        
        if (collided) gameObjectToToggle.SetActive(true);
        else gameObjectToToggle.SetActive(false);
    }
    
    private void Interaction()
    {
        if (collided && Input.GetKeyDown(interactKey) && cooldown < Time.time)
        {
            onInteraction?.Invoke();
            cooldown = Time.time + interactionCooldown;
        }
    }
    public void HidePrompt()
    {
        hide = true;

        Destroy(gameObject, 0.1f);
    }

    private IEnumerator CameraSearch()
    {
        yield return new WaitForSeconds(cameraSearchIn);
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraFound = true;
    }
}
