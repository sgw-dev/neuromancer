using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTest : MonoBehaviour
{
    HexTileController hexTileController;
    public HexTile hexTile;

    public void Start()
    {
        hexTileController = GameObject.FindGameObjectWithTag("HexController").GetComponent<HexTileController>();
        hexTile = hexTileController.FindHex(transform.position);
        transform.position = hexTile.Position;

        hexTile.SetObject(gameObject);
    }
}
