using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    public GameObject agent;
    public PointerController pc;
    public HexTileController htc;

    private HexTile hexTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            hexTile = pc.HexTile;
            
            Search search = new Search();
            List<int> path = search.GreedySearch(htc.FindHex(agent.transform.position), hexTile, htc);
            //agent.transform.position = hexTile.Position;
            foreach(int i in path)
            {
                Debug.Log(i);
            }
            Debug.Log("---------");
        }
    }
}
