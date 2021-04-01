using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public float speed;
    Vector3 velocity;

    public override void InstantiateProjectile(Vector3 sp, Vector3 tp)
    {
        base.InstantiateProjectile(sp, tp);
        float dis = Vector3.Distance(sp, tp);
        StartCoroutine(SelfDestruct(dis / speed));
        transform.position = sp;
        RotateToTarget();
        velocity = transform.right * speed;
    }

    public void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
