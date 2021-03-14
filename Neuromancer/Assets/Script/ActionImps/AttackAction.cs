using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackAction : Action
{

    Character takenby;
    Vector3 space;
    AttackType type;
    int damage;
    HexTileController htc;

    public AttackAction(Character takenby,Vector3 space)//,AttackType type,int damage) 
    {
        this.space   = space;
        this.takenby = takenby;
        this.type    = type;//determine by character class
        this.damage  = damage;//determined by class
        htc = GameObject.Find("TileController").GetComponent<HexTileController>();
    }
    void Attack(){
        HexTile attackedtile = htc.FindHex(space);
        //apply damage to all in the range of the tile;

        List<Character> inarea = new List<Character>();
        foreach(Character c in inarea)
        {
            c.stats.health += damage;
        }
    }

    //needs to be aware of GameObjects
    public bool Execute() {

        // //Get the tile that the character is attacking from
        // HexTile characterLocation = takenby.gameCharacter.parent.gameObject.GetComponent<HexTile>();
        // //find the space indicated by space
        // HexTile attackLocation = characterLocation.nexts[space];
        // Character other = null;
        // //if there is a child node it is character
        // if(attackLocation.transform.childCount > 0) {
        //     //get the character
        //     other = attackLocation.transform.GetChild(0).GetComponent<Agent>().character;
        //     other.stats.health -= damage;
        // }
        // //play animation?
        // Debug.Log(takenby+" is attacking "+space+" with a " + type.ToString() + " attack.");

        //check if tile is in attack range

        //get tile
        if(false){
            //check if tile is in range
            return false;
        }
        
        Attack();
        return true;
    }

    public Character TakenBy() {
        return takenby;
    }
}
