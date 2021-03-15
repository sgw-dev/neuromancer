using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    public HexTileController hexTileController;
    HexTile hexTile;
    SpriteRenderer sprite;
    [SerializeField] Color red;
    [SerializeField] Color blue;

    public int testCount;

    public void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        hexTile = hexTileController.FindHex(transform.position);
        hexTile.setHighlight(true);
        transform.position = hexTile.Position;
        MoveHighlight();
    }

    public void Update()
    {
        MoveHighlight();
        testCount = hexTileController.FindHexDistance(Vector3.zero, hexTile.Position);
    }

    public void MoveHighlight()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        hexTile.setHighlight(false);
        hexTile = hexTileController.FindHex(mousePos, hexTile);
        hexTile.setHighlight(true);

        if (hexTile.IsObstacle)
        {
            sprite.color = red;
        }
        else
        {
            sprite.color = blue;
        }
        transform.position = hexTile.Position;
    }

    public Vector3 Position
    {
        get { return hexTile.Position; }
        set { hexTile.Position = value; }
    }

    public HexTile HexTile
    {
        get { return hexTile; }
        set { hexTile = value; }
    }
}
