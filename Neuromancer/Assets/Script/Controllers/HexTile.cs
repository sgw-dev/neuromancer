using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public HexTile[] nexts = new HexTile[6];
    Vector3 position;

    public void Start()
    {
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
}
