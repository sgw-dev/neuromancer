using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : HealthBarValidate
{
    public float changeRate = .5f;
    ParticleSystem particles;

    public float max, current;
    public bool update;
    public float tohealth;

    public void Start()
    {
        base.Start();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Update()
    {
        if (update)
        {
            StartChangeHealth(tohealth);
            update = false;
        }
    }

    public void SetMax(float value)
    {
        max = value;
        StartChangeHealth(value);
    }

    public void StartChangeHealth(float value)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeHealth(value));
    }


    IEnumerator ChangeHealth(float to)
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
