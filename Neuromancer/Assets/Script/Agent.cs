using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class Agent : MonoBehaviour
{
   
    public Character character;
    public HexTile   currentlyOn;
    

    void Update()
    {
        //probably animate characters here
    }


    public int Health()
    {
        return character.stats.health;
    }

    public int Energy()
    {
        return character.stats.energy;
    }

    public CharacterClass Type()
    {
        return character.characterclass;
    }

    public int Speed()
    {
        return character.stats.speed;
    }
    public int Range()
    {
        return character.stats.range;
    }

    public void Health(int change)
    {
        Debug.Log("Health Before: " + character.stats.health);
        character.stats.health -= change;
        Debug.Log("Health After: " + character.stats.health);
    }

    public void Energy(int change)
    {
        character.stats.energy += change;
    }

    public void Speed(int change) 
    {
        character.stats.speed += change;
    }

    public void AttackDmg(int change)
    {
        character.stats.attackdmg += change;
    }

}
