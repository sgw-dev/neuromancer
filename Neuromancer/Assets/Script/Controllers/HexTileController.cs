﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileController : MonoBehaviour
{
    public HexTile hexPrefab;
    [SerializeField] HexTile head;

    [SerializeField] int amountTiles;

    [SerializeField] float cellSize;
    float halfX;
    float halfY;
    Vector3[] adjacentHexes;

    public void Start()
    {
        CreateHexPositions();
        GenerateTiles();
    }

    void GenerateTiles()
    {
        Queue<HexTile> queue = new Queue<HexTile>();
        HexTile currentHT = head;

        int max = amountTiles - 1;
        while(max > 0)
        {
            for (int i = 0; i < currentHT.nexts.Length && max > 0; i++)
            {
                if (currentHT.nexts[i] == null)
                {
                    queue.Enqueue(AddHexTile(currentHT, i));
                    max--;
                }
            }
            currentHT = queue.Dequeue();
        }
    }

    public HexTile AddHexTile(HexTile ht, int side)
    {
        HexTile newHT = Instantiate(hexPrefab, transform);
        newHT.Position = ht.Position + adjacentHexes[side];

        ht.InsertNext(side, newHT);
        int add = (side + 1) % 6;
        int minus = side - 1;
        if (minus == -1)
            minus = 5;

        if(ht.nexts[add] != null)
            ht.nexts[add].InsertNext(minus, newHT);

        if(ht.nexts[minus] != null)
            ht.nexts[minus].InsertNext(add, newHT);

        return newHT;
    }

    public HexTile FindClosestHex(Vector3 point, HexTile ht)
    {
        HexTile rtn = ht;
        float minMag = (ht.Position - point).sqrMagnitude;

        for (int i = 0; i < 6; i++)
        {
            HexTile temp = ht.nexts[i];
            if (temp == null)
                continue;

            float mag = (temp.Position - point).sqrMagnitude;
            if (mag < minMag)
            {
                rtn = temp;
                minMag = mag;
            }
        }

        return rtn;
    }

    public int FindHexDistance(Vector3 a, Vector3 b)
    {
        float x = Mathf.Abs(b.x - a.x);
        float y = Mathf.Abs(b.y - a.y);
        int hexCount = 0;

        while (y >= halfY)
        {
            y -= halfY;
            x -= halfX;
            hexCount++;
        }
        while (x >= cellSize)
        {
            x -= cellSize;
            hexCount++;
        }

        return hexCount;
    }

    public float CellSize
    {
        get { return CellSize; }
    }

    public float HalfX
    {
        get { return halfX; }
    }

    public float HalfY
    {
        get { return halfY; }
    }

    public Vector3[] CloseHexes
    {
        get { return adjacentHexes; }
    }

    public HexTile Head
    {
        get { return head; }
        set { head = value; }
    }

    void CreateHexPositions()
    {
        halfX = cellSize / 2;
        halfY = cellSize * .75f;
        adjacentHexes = new Vector3[6];
        adjacentHexes[0] = new Vector3(cellSize, 0);
        adjacentHexes[1] = new Vector3(halfX, halfY);
        adjacentHexes[2] = new Vector3(-halfX, halfY);
        adjacentHexes[3] = new Vector3(-cellSize, 0);
        adjacentHexes[4] = new Vector3(-halfX, -halfY);
        adjacentHexes[5] = new Vector3(halfX, -halfY);
    }
}