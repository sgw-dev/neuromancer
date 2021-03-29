using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour , IEquatable<HexTile>
{
    public HexTile[] nexts = new HexTile[6];
    Vector3 position;
    [SerializeField] SpriteRenderer highlight;
    [SerializeField] Color red;
    Color defaultColor;
    GameObject holdingObject = null;
    bool isObstacle;

    public void Start()
    {
        defaultColor = highlight.color;
        highlight.enabled = false;
        transform.position = position;
    }

    public void InsertNext(int side, HexTile other)
    {
        nexts[side] = other;
        side = (side + 3) % 6;
        other.nexts[side] = this;
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public GameObject HoldingObject
    {
        get { return holdingObject; }
    }

    public bool IsObstacle
    {
        get { return isObstacle; }
    }

    public void SetObject(GameObject obj, bool isObstacle = false)
    {
        holdingObject = obj;
        this.isObstacle = isObstacle;
        if (isObstacle)
        {
            highlight.color = red;
        }
    }

    public void setHighlight(bool b)
    {
        if (isObstacle)
            highlight.color = red;
        else
            highlight.color = defaultColor;

        highlight.enabled = b;
    }

    //By Spencer
    public void setHighlight(bool b, Color c)
    {
        highlight.color = c;
        highlight.enabled = b;
    }
    public bool Equals(HexTile other)
    {
        return Position.Equals(other.position);
    }
}
