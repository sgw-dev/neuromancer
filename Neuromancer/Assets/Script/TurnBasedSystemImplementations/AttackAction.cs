using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackAction : Action
{

    Character takenby;
    int space;
    AttackType type;
    public AttackAction(Character takenby,int space,AttackType type) {
        this.space=space;
        this.takenby=takenby; 
    }
    public void Execute() {
        //check takenby character tile
        
        //HexTile parent = takenby.transform.parent.gameObject.GetComponent<HexTile>();
            //find the space indicated by space
            //apply attack to that space
        Debug.Log(takenby+" is attacking "+space+" with a " + type.ToString() + " attack.");
    }

    public Character TakenBy() {
        return takenby;
    }
}
