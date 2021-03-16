using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    public GameObject agent;
    public PointerController pc;
    public HexTileController htc;

    private HexTile destTile;
    private bool movingFlag = false;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !movingFlag)
        {
            destTile = pc.HexTile;
            
            HexTile startTile = htc.FindHex(agent.transform.position);
            if (!destTile.Equals(startTile))
            {
                List<int> path = Search.GreedySearch(startTile, destTile, htc);
                StartCoroutine(Move(path, 0.5f));
                movingFlag = true;
            }
            
            
        }
    }
    public IEnumerator Move(List<int> path, float frameTime)
    {
        foreach(int step in path)
        {
            agent.transform.position = htc.FindHex(agent.transform.position).nexts[step].Position;
            yield return new WaitForSeconds(frameTime);
        }
        movingFlag = false;
    }
}
