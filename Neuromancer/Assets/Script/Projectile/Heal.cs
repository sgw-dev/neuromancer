using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Projectile
{
    public float duration;
    public override void InstantiateProjectile(Vector3 sp, Vector3 tp)
    {
        base.InstantiateProjectile(sp, tp);
        StartCoroutine(SelfDestruct(duration));
        transform.position = tp;
    }
}
