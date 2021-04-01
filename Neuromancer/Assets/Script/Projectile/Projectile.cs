using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector3 startPos;
    protected Vector3 targetPos;

    public virtual void InstantiateProjectile(Vector3 sp, Vector3 tp)
    {
        startPos = sp;
        targetPos = tp;
    }

    protected IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void RotateToTarget(Vector3 dir)
    {
        float angle = Vector3.SignedAngle(dir, targetPos - startPos, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
