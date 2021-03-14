using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackAction : Action
{

    Character takenby;
    int space;
    AttackType type;
    int damage;
    public AttackAction(Character takenby,int space,AttackType type,int damage) 
    {
        this.space   = space;
        this.takenby = takenby;
        this.type    = type;
        this.damage  = damage;
    }


    //needs to be aware of GameObjects
    public void Execute() {
        //Get the tile that the character is attacking from
        HexTile characterLocation = takenby.gameCharacter.parent.gameObject.GetComponent<HexTile>();
        //find the space indicated by space
        HexTile attackLocation = characterLocation.nexts[space];
        Character other = null;
        //if there is a child node it is character
        if(attackLocation.transform.childCount > 0) {
            //get the character
            other = attackLocation.transform.GetChild(0).GetComponent<Agent>().character;
            other.stats.health -= damage;
        }
        //play animation?
        Debug.Log(takenby+" is attacking "+space+" with a " + type.ToString() + " attack.");
    }

    public Character TakenBy() {
        return takenby;
    }
}
