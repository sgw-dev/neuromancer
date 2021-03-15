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
        //take character off the current tile
        takenby.gameCharacter.GetComponent<Agent>().currentlyOn.ObjectOnTile=null;
        //put character onto tile
        takenby.gameCharacter.position = tile.transform.position;
        tile.ObjectOnTile = takenby.gameCharacter.gameObject;
        //update the tile character refernce
        takenby.gameCharacter.GetComponent<Agent>().currentlyOn = tile;

    }

    public bool Execute() 
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