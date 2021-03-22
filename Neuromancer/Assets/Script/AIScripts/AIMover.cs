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

    public Player player;

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
        
        //If it is my turn
        /*if (player.name.Equals(GameSystem.CurrentGame().WhosTurn().name))
        {
            foreach(CharacterClass cc in classes)
            {
                Character character = player.characters[cc.ToString()];
                
            }
        }*/
        //Debug.Log("It is " + GameSystem.CurrentGame().WhosTurn().name + "'s turn");
        if (player.name.Equals(GameSystem.CurrentGame().WhosTurn().name))
        {
            Debug.Log(player.name + "(AI) Turn");
            //If it is my turn, I am players[0] and the enemy is players[1]
            Player enemyPlayer = GameSystem.CurrentGame().Players()[1];
            List<Character> myChars = new List<Character>(player.characters.Values);
            List<Character> enemyChars = new List<Character>(enemyPlayer.characters.Values);
            foreach(Character character in myChars)
            {
                GameState gameState = new GameState(myChars, enemyChars, htc.FindHex(character.gameCharacter.position), character);
                MiniAction miniAction = Search.DecideAction(gameState, htc);
                //Debug.Log(character.name+" will "+miniAction.type);

                switch (miniAction.type)
                {
                    case "Move":
                        HexTile startTile = htc.FindHex(character.gameCharacter.position);
                        MiniMove move = miniAction as MiniMove;
                        List<int> path = Search.GreedySearch(startTile, move.Dest, htc);
                        //Path ends on the destination (the enemy player), remove the last element
                        //Because we can't move ontop of a player
                        //Only send the first few steps
                        int speed = character.stats.speed;
                        //Debug.Log("Path before : " + printArray(path));
                        List<Vector3> moves = new List<Vector3>();
                        if (path.Count > 0)
                        {
                            path = TrimPath(path, speed);
                            //Debug.Log("Path after : " + printArray(path));
                            moves.Add(startTile.nexts[path[0]].Position);
                            for (int i = 1; i < path.Count; i++)
                            {
                                int neighbor = path[i];
                                moves.Add(htc.FindHex(moves[i - 1]).nexts[path[i]].Position);

                            }
                        }
                        else
                        {
                            moves.Add(character.gameCharacter.position);
                        }
                        
                        //Debug.Log("Moving to " + moves[moves.Count - 1]);
                        //Action a = MoveActionFactory.getInstance().CreateAction(character, moves[moves.Count-1]);
                        Action a = MoveActionFactory.getInstance().CreateAction(character, moves.ToArray());
                        GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                        //StartCoroutine(Move(character.gameCharacter.gameObject, path, 0.5f));
                        break;
                    case "Attack":
                        break;
                }
            }
            //End the AI's turn
            EndMyTurn();


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
    public void EndMyTurn()
    {
        GameSystem.CurrentGame().EndTurn(player);
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
