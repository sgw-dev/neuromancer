using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackAction : TurnBasedSystem.Action
{

    Character takenby;
    Vector3 space;
    int range;
    int damage;
    HexTileController htc;

    public AttackAction(Character takenby,Vector3 space)//,AttackType type,int damage) 
    {
        this.space   = space;
        this.takenby = takenby;
        this.range   = takenby.stats.range;//determine by character class
        this.damage  = takenby.stats.attackdmg;//determined by class
        htc = GameObject.Find("TileController").GetComponent<HexTileController>();
    }

    void Attack()
    {

        HexTile attackedtile = htc.FindHex(space);
        if(takenby.characterclass.Equals(CharacterClass.HACKER))
        {
            //deals AOE
            ///2 is hard coded, in future replace with a character class variable
            List<Character> inarea = GetAllInAOERange(attackedtile,2);
            foreach(Character c in inarea)
            {
                c.stats.health += damage;
                foreach(Player p in GameSystem.CurrentGame().Players())
                {
                    GameSystem.CurrentGame().CheckDeath(c,p);
                }
            }
        } 
        else 
        {//deal damage to single object
            if(attackedtile.HoldingObject != null)
            {
                Agent totakedamage = attackedtile.HoldingObject.GetComponent<Agent>();
                
                if(totakedamage != null) 
                {
                    totakedamage.Health(damage);
                    foreach(Player p in GameSystem.CurrentGame().Players())
                    {
                        GameSystem.CurrentGame().CheckDeath(totakedamage.character,p);
                    }

                }

            }
        }
        
    }

    public List<Character> GetAllInAOERange(HexTile tile,int tiles)
    {
        List<Character> inrange = new List<Character>();
        int radius = tiles;
        
        GameObject[] tmp =  Array.ConvertAll<HexTile,GameObject>(htc.FindRadius(tile,radius).ToArray(), t => t.HoldingObject);
        for(int i = 0 ; i < tmp.Length; i++ ){
            Agent agent = tmp[i].GetComponent<Agent>();  
            if(agent != null) {
                inrange.Add(agent.character);
            }
        }

        return inrange;

    }

    public bool Execute() 
    {
        // Debug.Log(takenby+" is attacking "+space+" with a " + type.ToString() + " attack.");

        //check if tile is in attack range
        //get tile
        int totaldistance = htc.FindHexDistance(takenby.gameCharacter.position,space);
        
        //check if tile is in range
        if(totaldistance > this.range )
        {
            //failed to take action so return false and allow something else to happen
            return false;
        }
        //*****Added by Spencer ******
        takenby.ActionTakenThisTurn = true;

        Debug.Log(takenby+" attacks " + space);
        Attack();
        return true;
    }

    public Character TakenBy() {
        return takenby;
    }
}
