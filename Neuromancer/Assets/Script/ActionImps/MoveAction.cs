using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;


public class MoveAction : Action
{
    Character takenby;
    Vector3 moveto;
    HexTileController htc;

    public MoveAction(Character c, Vector3 tile) 
    {
        takenby = c;
        moveto = tile;
        htc = GameObject.Find("TileController").GetComponent<HexTileController>();
    }

    public void MoveTo(Vector3 position) 
    {
        
        //get the tile
        HexTile tile = htc.FindHex(position);
        //put character onto tile
        takenby.gameCharacter.position = tile.transform.position;
        //update the tile character refernce
        takenby.gameCharacter.GetComponent<Agent>().currentlyOn = tile;
        
        //if the tile holds references update that too

        //finally mark the character as actions taken
        //takenby.ActionTakenThisTurn = true;
    }

    bool Execute() 
    {
        if(false) {
            //check if the agent can move to the desired tile
            //is there already something there?
            //is it in the cahacters move range
            
            return false;//dont execute the move if these are true
        }
        MoveTo(moveto);
        return true;
    }

    public Character TakenBy() 
    {
        return takenby;
    }
}