using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class Player 
    {

        public string   name;
        public bool     ReadyToEndTurn;
        
        //Characters and items?
        public Dictionary<string,string>    items;
        
        //one character name per character,
        //if using more than one class create a name like MELEE1,2..
        public Dictionary<string,Character> characters;

        public Player(string n) 
        {
            
            name       = n;
            items      = new Dictionary<string,string>();
            characters = new Dictionary<string,Character>();
            
            ReadyToEndTurn = false;

        }

        public void TurnReset() 
        {
            ReadyToEndTurn = true;
        }

        public void EndTurn() 
        {
            ReadyToEndTurn=true;
            GameSystem.CurrentGame().EndTurn(this);
        }

    }
}