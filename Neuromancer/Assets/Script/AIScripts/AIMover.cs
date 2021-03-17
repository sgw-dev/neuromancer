using System.Collections;
using System.Collections.Generic;
using TurnBasedSystem;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    public GameObject agent;
    public PointerController pointer;
    public HexTileController htc;

    private HexTile destTile;
    private bool movingFlag = false;

    private Player player;

    CharacterClass[] classes =
        new CharacterClass[] {
                CharacterClass.MELEE,
                CharacterClass.RANGED,
                CharacterClass.PSYONIC,
                CharacterClass.HACKER
            };

    // Start is called before the first frame update
    void Start()
    {
        //player = GameSystem.CurrentGame().Players()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
        player = GameSystem.CurrentGame().Players()[1];
        //If it is my turn
        if (player.name.Equals(GameSystem.CurrentGame().WhosTurn()))
        {
            foreach(CharacterClass cc in classes)
            {
                Character character = player.characters[cc.ToString()];
                
            }
        }
        
        if (Input.GetButtonDown("Fire1") && !movingFlag)
        {
            destTile = pointer.HexTile;
            
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
