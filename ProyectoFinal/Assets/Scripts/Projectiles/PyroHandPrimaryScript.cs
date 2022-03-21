using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroHandPrimaryScript : ProjectileScript
{
    public ProjectileScriptableObject pSO;
    private void Update()
    {
        Movement(pSO.speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (((1 << other.gameObject.layer) & pSO.aimColliderMask) != 0) // si es cualquier cosa, menos un proyectil, un enemigo o un player.
        {
            OnImpact(pSO.defaultImpact, pSO.aimColliderMask);
        }
        if (((1 << other.gameObject.layer) & pSO.enemyColliderMask) != 0)
        {
            OnImpact(pSO.enemyImpact, pSO.enemyColliderMask);
        }
    }
}