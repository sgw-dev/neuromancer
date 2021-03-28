using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class GameOver : MonoBehaviour
{
    
    bool calledonce = false;
    public void GameOverFor(Player p)
    {
        if(!calledonce) {
            if(p.name.Equals("PLAYER_1"))//the player wins
            {
                Debug.Log("Victory!");
            } 
            else if(p.name.Equals("PLAYER_2")) //the ai wins
            {
                Debug.Log("Better luck next time");
            }
            else
            {
                Debug.LogError("Something is wrong with player names");
            }
            calledonce=true;
        }
    }

    public void Tie()
    {
        Debug.Log("ITS A TIE");
    }
}