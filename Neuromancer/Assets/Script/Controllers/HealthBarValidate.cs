using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarValidate : MonoBehaviour
{
    SpriteRenderer fill;
    SpriteRenderer background;
    Transform handle;

    public float width;
    [Range(0.0f, 1.0f)]
    public float value;

    float left;
    Vector3 handlePosition;
    Vector3 fillSize;
    Vector3 backgroundSize;

    public void Start()
    {
        UpdateWidth();
    }

    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }
        UnityEditor.EditorApplication.delayCall += () =>
        {
            try
            {
                Start();
                MoveHandle(value);
            }
            catch { }
        };
    }

    public void MoveHandle(float precent)
    {
        value = Mathf.Clamp(precent, 0, 1);
        float x = backgroundSize.x * value;
        handlePosition.x = x + left;
        fillSize.x = x;
        handle.position = handlePosition;
        fill.size = fillSize;
    }

    void UpdateWidth()
    {
        left = -width / 2;

        Transform[] t = GetComponentsInChildren<Transform>();
        fill = t[1].GetComponent<SpriteRenderer>();
        background = t[2].GetComponent<SpriteRenderer>();
        handle = t[3];

        fillSize = fill.size;
        backgroundSize = background.size;
        handlePosition = handle.position;

        Vector3 origin = fill.transform.position;
        origin = new Vector3(left, origin.y, origin.z);
        fill.transform.position = origin;
        background.transform.position = origin;

        backgroundSize.x = width;
        background.size = backgroundSize;
    }
}
