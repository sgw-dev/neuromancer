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

    public bool started = false;
    
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
        if (player.name.Equals(GameSystem.CurrentGame().WhosTurn().name))
        {
            if(!started)
                StartCoroutine(MoveAllCharacters()); 
        }
    }
    private IEnumerator MoveAllCharacters()
    {
        started = true;
        //If it is my turn, I am players[0] and the enemy is players[1]
        Player enemyPlayer = GameSystem.CurrentGame().Players()[1];
        List<Character> myChars = new List<Character>(player.characters.Values);

        yield return new WaitForSecondsRealtime(0.5f);
        //Move Charatcers in order of Melee, Hacker, sniper, psyonic
        // *** Melee ***
        Character activeChar = GetChar(myChars, CharacterClass.MELEE);
        if (activeChar != null)
        {
            activeChar.SetSelected(true);
            yield return new WaitForSecondsRealtime(0.5f);
            MoveCharacter(activeChar, myChars, enemyPlayer);
            yield return new WaitForSecondsRealtime(0.5f);
            activeChar.SetSelected(false);
        }
        // *** Hacker ***
        enemyPlayer = GameSystem.CurrentGame().Players()[1];
        myChars = new List<Character>(player.characters.Values);
        activeChar = GetChar(myChars, CharacterClass.HACKER);
        if (activeChar != null)
        {
            activeChar.SetSelected(true);
            yield return new WaitForSecondsRealtime(0.5f);
            MoveCharacter(activeChar, myChars, enemyPlayer);
            yield return new WaitForSecondsRealtime(0.5f);
            activeChar.SetSelected(false);
        }
        // *** Sniper ***
        enemyPlayer = GameSystem.CurrentGame().Players()[1];
        myChars = new List<Character>(player.characters.Values);
        activeChar = GetChar(myChars, CharacterClass.RANGED);
        if (activeChar != null)
        {
            activeChar.SetSelected(true);
            yield return new WaitForSecondsRealtime(0.5f);
            MoveCharacter(activeChar, myChars, enemyPlayer);
            yield return new WaitForSecondsRealtime(0.5f);
            activeChar.SetSelected(false);
        }
        // *** Psyonic ***
        enemyPlayer = GameSystem.CurrentGame().Players()[1];
        myChars = new List<Character>(player.characters.Values);
        activeChar = GetChar(myChars, CharacterClass.PSYONIC);
        if (activeChar != null)
        {
            activeChar.SetSelected(true);
            yield return new WaitForSecondsRealtime(0.5f);
            MoveCharacter(activeChar, myChars, enemyPlayer);
            yield return new WaitForSecondsRealtime(0.5f);
            activeChar.SetSelected(false);
        }

        //End the AI's turn
        EndMyTurn();
        started = false;
    }
    private Character GetChar(List<Character> myChars, CharacterClass charClass)
    {
        Character activeChar = null;
        foreach (Character c in myChars)
        {
            if (c.characterclass == charClass)
                activeChar = c;
        }
        return activeChar;
    }
    private bool MoveCharacter(Character activeChar, List<Character> myChars, Player enemyPlayer)
    {
        if (activeChar == null)
            return false;

        List<Character> enemyChars = new List<Character>(enemyPlayer.characters.Values);
        GameState gameState = new GameState(myChars, enemyChars, htc.FindHex(activeChar.gameCharacter.position), activeChar);

        MiniAction miniAction = Search.DecideAction(gameState, htc);
        //Debug.Log(character.name+" will "+miniAction.type);

        switch (miniAction.type)
        {
            case "Move":
                HexTile startTile = htc.FindHex(activeChar.gameCharacter.position);
                MiniMove move = miniAction as MiniMove;
                
                List<int> path = Search.GreedySearch(startTile, move.Dest, htc);
                //Path ends on the tile you want to move to
                List<Vector3> moves = new List<Vector3>();
                if (path.Count > 0)
                {
                    moves.Add(startTile.nexts[path[0]].Position);
                    for (int i = 1; i < path.Count; i++)
                    {
                        int neighbor = path[i];
                        moves.Add(htc.FindHex(moves[i - 1]).nexts[path[i]].Position);
                    }
                }
                else
                {
                    moves.Add(activeChar.gameCharacter.position);
                }

                Debug.Log(activeChar.name+" is Moving to " + moves[moves.Count - 1]);
                //Action a = MoveActionFactory.getInstance().CreateAction(character, moves[moves.Count-1]);

                Action m = MoveActionFactory.getInstance().CreateAction(activeChar, moves.ToArray());
                GameSystem.CurrentGame().ExecuteCharacterAction(player, m);

                //StartCoroutine(Move(character.gameCharacter.gameObject, path, 0.5f));
                break;
            case "Attack":
                MiniAttack attack = miniAction as MiniAttack;
                Action a = AttackActionFactory.GetInstance().CreateAction(activeChar, attack.toAttack.gameCharacter.position);
                GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                break;
        }
        return true;
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
    public IEnumerator WaitForIt(float frameTime)
    {
        movingFlag = true;
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(frameTime);
        Debug.Log("Done!");
        movingFlag = false;
    }
}
