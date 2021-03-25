using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : HealthBarValidate
{
    public float changeRate = .5f;
    ParticleSystem particles;

    public float max, current;

    public override void Initialize()
    {
        base.Initialize();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void SetMax(int value)
    {
        Initialize();
        max = value;
        ChangeHealth(value);
    }

    public void ChangeHealth(int value)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeHealthBar(value));
    }


    IEnumerator ChangeHealthBar(float to)
    {
        particles.Play();
        float from = current;
        float progress = 0;
        while (progress < changeRate)
        {
            current = Mathf.Lerp(from, to, progress / changeRate);
            MoveHandle(current / max);
            yield return null;
            progress += Time.deltaTime;
        }
        particles.Stop();
        MoveHandle(to / max);
    }
}
