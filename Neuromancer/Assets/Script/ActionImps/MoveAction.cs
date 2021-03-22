using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;


public class MoveAction : Action
{
    Character takenby;
    Vector3[] moveto;
    HexTileController htc;
    
    int total_move_action;
    int current_move_action;

    public MoveAction(Character c, params Vector3[] tiles) 
    {
        takenby = c;
        moveto = tiles;
        total_move_action = tiles.Length;
        current_move_action = 0;
        htc = GameObject.Find("TileController").GetComponent<HexTileController>();
    }

    public IEnumerator MoveTo(params Vector3[] positions)
    {
        while(current_move_action!=total_move_action)
        {
            //get the tile to goto
            HexTile tile = htc.FindHex(positions[current_move_action]);
            //take character off the current tile
            takenby.gameCharacter.GetComponent<Agent>().currentlyOn.SetObject(null,false);
            //put character onto the next tile 
            takenby.gameCharacter.position = tile.transform.position;
            tile.SetObject(takenby.gameCharacter.gameObject,false);
            //update the tile character refernce
            takenby.gameCharacter.GetComponent<Agent>().currentlyOn = tile;
            //increment the action
            current_move_action++;
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    public bool Execute() 
    {
        if(false) {
            //check if the agent can move to the desired tile
            //is there already something there?
            //is it in the characters move range
            
            return false;//dont execute the move if these are true
        }
        takenby.ActionTakenThisTurn = true;
        GameSystem.CurrentGame().monoref.StartCoroutine(MoveTo(moveto));
        return true;
    }

    public Character TakenBy() 
    {
        return takenby;
    }
}