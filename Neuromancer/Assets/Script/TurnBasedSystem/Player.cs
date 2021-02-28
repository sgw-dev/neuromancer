﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class Player 
    {

        public string   name;
        public int      currency;
        public bool     ReadyToEndTurn;
        
        //Characters and items?
        public Dictionary<string,string>    items;
        public Dictionary<string,Character> characters;
        
        //Actions 
        public List<Action> inprogress_character_actions;
        public List<Action> possible_character_actions;

        public Player(string n) 
        {
            
            name       = n;
            currency   = 0;
            items      = new Dictionary<string,string>();
            characters = new Dictionary<string,Character>();
            
            inprogress_character_actions = new List<Action>();
            possible_character_actions   = new List<Action>();

            ReadyToEndTurn = false;

        }

        public void TurnReset() 
        {
            ReadyToEndTurn = true;
        }

        public void EndTurn() 
        {
            ReadyToEndTurn=true;
        }

    }
}