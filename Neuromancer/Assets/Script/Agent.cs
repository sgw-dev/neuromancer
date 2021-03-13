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

    public void Health(int change)
    {
        character.stats.health += change;
    }

    public void Energy(int change)
    {
        character.stats.energy += change;
    }

    public void Speed(int change) 
    {
        character.stats.speed += change;
    }

}
