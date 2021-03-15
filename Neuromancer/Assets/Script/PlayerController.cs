﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public HexTileController htc;


    public void Move(){
        

        //demonstration
        foreach (Character c in player.characters.Values)
        {
            float x = Random.Range(-10f,10f);
            float y = Random.Range(-10f,10f);
            Action a = MoveActionFactory.getInstance().CreateAction(c,new Vector3(x,y,0));
            GameSystem.CurrentGame().ExecuteCharacterAction(player,a);
        }

    }


    public void EndMyTurn() 
    {
        GameSystem.CurrentGame().EndTurn(player);
    }

    public void Attack() {

    }
}