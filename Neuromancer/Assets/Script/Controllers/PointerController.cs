using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    public HexTileController hexTileController;
    HexTile hexTile;
    List<HexTile> surrounding;
    SpriteRenderer sprite;
    [SerializeField] Color red;
    [SerializeField] Color blue;

    public int testCount;

    private bool canHighlight = true;

    public void Start()
    {
        surrounding = new List<HexTile>();

        sprite = GetComponentInChildren<SpriteRenderer>();
        hexTile = hexTileController.FindHex(transform.position);
        hexTile.setHighlight(true);
        transform.position = hexTile.Position;
        MoveHighlight();
    }

    public void Update()
    {

        MoveHighlight();
        
        

        /*
        foreach (HexTile ht in surrounding)
            ht.setHighlight(false);
        surrounding = hexTileController.FindRadius(hexTile, 2);

        foreach(HexTile ht in surrounding)
            ht.setHighlight(true);
            */

        testCount = hexTileController.FindHexDistance(Vector3.zero, hexTile.Position);
    }

    public void MoveHighlight()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (canHighlight)
        {
            hexTile.setHighlight(false);
        }
        
        hexTile = hexTileController.FindHex(mousePos, hexTile);
        if (canHighlight)
        {
            hexTile.setHighlight(true);

            if (hexTile.IsObstacle)
            {
                sprite.color = red;
            }
            else
            {
                sprite.color = blue;
            }
        }
        
        transform.position = hexTile.Position;
    }
    public void SetCanHighlight(bool canH)
    {
        canHighlight = canH;
        hexTile.setHighlight(false);
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
