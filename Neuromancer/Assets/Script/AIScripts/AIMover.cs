using System.Collections;
using System.Collections.Generic;
using TurnBasedSystem;
using UnityEngine;
using static Search;

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
            Player enemyPlayer = GameSystem.CurrentGame().Players()[0];
            List<Character> myChars = new List<Character>(player.characters.Values);
            List<Character> enemyChars = new List<Character>(enemyPlayer.characters.Values);
            GameState gameState = new GameState(myChars, enemyChars, htc.FindHex(myChars[0].gameCharacter.position), myChars[0]);
            MiniAction miniAction = Search.DecideAction(gameState, htc);
            Debug.Log(miniAction.type);
            
            switch (miniAction.type)
            {
                case "Move":
                    HexTile startTile = htc.FindHex(myChars[0].gameCharacter.position);
                    MiniMove move = miniAction as MiniMove;
                    List<int> path = Search.GreedySearch(startTile, move.Dest, htc);
                    //Path ends on the destination (the enemy player), remove the last element
                    //Because we can't move ontop of a player
                    Debug.Log("Full path is " + printArray(path));
                    //Only send the first few steps
                    int speed = myChars[0].stats.speed;
                    Debug.Log("Stride is " + speed);
                    path = TrimPath(path, speed);
                    Debug.Log("Trimmed Path " + printArray(path));
                    StartCoroutine(Move(myChars[0].gameCharacter.gameObject, path, 0.5f));
                    break;
                case "Attack":
                    break;
            }
            /*destTile = pointer.HexTile;
            
            HexTile startTile = htc.FindHex(agent.transform.position);
            if (!destTile.Equals(startTile))
            {
                List<int> path = Search.GreedySearch(startTile, destTile, htc);
                StartCoroutine(Move(path, 0.5f));
                movingFlag = true;
            }*/
            
            
        }
    }
    public List<int> TrimPath(List<int> path, int moveRadius)
    {
        //Remove last
        path.RemoveAt(path.Count - 1);
        //Limit to agent's range
        List<int> temp = new List<int>();
        for(int i = 0; i < Mathf.Min(moveRadius, path.Count); i++)
        {
            temp.Add(path[i]);
        }
        return temp;
    }
    public string printArray(List<int> list)
    {
        string s = "";
        foreach(int i in list)
        {
            s += i + ", ";
        }
        return s;
    }
    public IEnumerator Move(GameObject agent, List<int> path, float frameTime)
    {
        foreach(int step in path)
        {
            agent.transform.position = htc.FindHex(agent.transform.position).nexts[step].Position;
            yield return new WaitForSeconds(frameTime);
        }
        movingFlag = false;
    }
}
