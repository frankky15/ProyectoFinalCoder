using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPrimary : ProjectileScript
{
    public ProjectileScriptableObject pSO;
    private float speed;
    private void Start()
    {
        speed = pSO.speed - transform.localScale.y;
    }
    private void Update()
    {
        Movement(speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (((1 << other.gameObject.layer) & pSO.aimColliderMask) != 0) // si es cualquier cosa, menos un proyectil, un enemigo o un player.
        {
            OnImpact(pSO.defaultImpact, pSO.aimColliderMask, 5f, pSO.explosionRadius * transform.localScale.y);
        }
        if (((1 << other.gameObject.layer) & pSO.enemyColliderMask) != 0)
        {
            OnImpact(pSO.enemyImpact, pSO.enemyColliderMask, 5f, pSO.explosionRadius * transform.localScale.y);
        }
    }
}
