using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : Projectile
{
    public float duration;
    public override void InstantiateProjectile(Vector3 sp, Vector3 tp)
    {
        base.InstantiateProjectile(sp, tp);
        StartCoroutine(SelfDestruct(duration));
        RotateToTarget(Vector3.up);
        transform.position = sp;
    }
}
