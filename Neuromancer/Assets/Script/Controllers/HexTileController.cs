using System.Collections;
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

    public void Awake()
    {
        CreateHexPositions();
        GenerateTiles();
    }

    public HexTile FindHex(Vector3 position)
    {
        return FindHex(position, head);
    }

    public HexTile FindHex(Vector3 pos, HexTile ht)
    {
        HexTile previous = ht;
        HexTile rtn = FindNextHex(pos, ht);
        while (rtn != previous)
        {
            previous = rtn;
            rtn = FindNextHex(pos, rtn);
        }
        return rtn;
    }

    HexTile FindNextHex(Vector3 point, HexTile ht)
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

    public List<HexTile> FindRadius(HexTile hexTile, int radius, bool checkObstacle = false)
    {
        List<HexTile> list = new List<HexTile>();

        for (int i = 0; i < 6; i++)
        {
            FindRadius(hexTile.nexts[i], ref list, radius, i, checkObstacle);
        }
        return list;
    }

    void FindRadius(HexTile current, ref List<HexTile> list, int radius, int dir, bool checkObstacle)
    {
        if(current == null || radius <= 0)
        {
            return;
        }
        list.Add(current);

        if (checkObstacle && (current.IsObstacle || current.HoldingObject != null))
        {
            FindRadius(current.nexts[dir], ref list, radius - 2, dir, checkObstacle);
        }
        else
        {
            FindRadius(current.nexts[dir], ref list, radius - 1, dir, checkObstacle);
        }

        int x = (dir + 1) % 6;
        FindRadius(current.nexts[x], ref list, radius - 1, x, checkObstacle);
    }

    void GenerateTiles()
    {
        Queue<HexTile> queue = new Queue<HexTile>();
        HexTile currentHT = head;

        int max = amountTiles - 1;
        while (max > 0)
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

    HexTile AddHexTile(HexTile ht, int side)
    {
        HexTile newHT = Instantiate(hexPrefab, transform);
        newHT.Position = ht.Position + adjacentHexes[side];

        ht.InsertNext(side, newHT);
        int add = (side + 1) % 6;
        int minus = side - 1;
        if (minus == -1)
            minus = 5;

        if (ht.nexts[add] != null)
            ht.nexts[add].InsertNext(minus, newHT);

        if (ht.nexts[minus] != null)
            ht.nexts[minus].InsertNext(add, newHT);

        return newHT;
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
