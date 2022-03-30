using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShootingPPManager : MonoBehaviour
{
    [SerializeField] private Volume PPvolume;
    [SerializeField] private float weightMult = 1f;
    [SerializeField] private float weightMult2 = 0.5f;

    private void Start()
    {
        PPvolume.weight = 0;
    }
    private void Update()
    {
        if (PPvolume.weight > 1) PPvolume.weight = 1;
        if (PPvolume.weight < 0) PPvolume.weight = 0;
        if (EquipmentHandler.Instance.isShooting && PPvolume.weight < 1) PPvolume.weight += weightMult * Time.deltaTime;
        else if (!EquipmentHandler.Instance.isShooting && PPvolume.weight > 0) PPvolume.weight -= weightMult2 * Time.deltaTime;
    }
}
