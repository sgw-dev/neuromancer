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

        float time = dis / speed;
        StartCoroutine(Stop(time));
        StartCoroutine(SelfDestruct(time + 1));

        transform.position = sp;
        RotateToTarget(Vector3.right);
        velocity = transform.right * speed;
    }

    protected IEnumerator Stop(float time)
    {
        yield return new WaitForSeconds(time);
        velocity = Vector3.zero;
    }

    public void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
