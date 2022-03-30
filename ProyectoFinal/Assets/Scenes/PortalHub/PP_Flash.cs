using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PP_Flash : MonoBehaviour
{
    [SerializeField] private Volume PPvolume;
    [SerializeField] private float flashTime = 1f;
    private void Start()
    {
        PPvolume.weight = 1f;
        PPvolume.priority = 3;
    }
    private void Update()
    {
        if (PPvolume.weight > 0)
        {
            PPvolume.weight -= flashTime * Time.deltaTime;
        }
        if (PPvolume.weight <= 0)
        {
            PPvolume.priority = 0;
        }
    }
}
