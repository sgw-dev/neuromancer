using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    Vector3 position;

    public HexTileController hexTile;

    public int testCount;

    public void Start()
    {
        position = transform.position;
    }

    public void Update()
    {
        MoveHighlight();
        testCount = hexTile.FindHexDistance(Vector3.zero, position);
    }

    public void MoveHighlight()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        position += hexTile.FindClosestHex(mousePos - position);
        transform.position = position;
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
}
