using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;
    public class Character : IComparable {
        public Transform gameCharacter;//need to get transform...etc
        public string name;
        public Stat stats;
        public CharacterClass characterclass;

        public bool ActionTakenThisTurn;
        
        public Character(string name,CharacterClass cclass,Stat charstats) 
        {
            name = name;
            stats = charstats;
            characterclass = cclass;
            ActionTakenThisTurn=false;
        }

        public int CompareTo(object obj) 
        {
            if(!(obj is Character )){
                throw new Exception(obj+" is not comparable to Character");
            }
            return ((Character)obj).stats.speed-stats.speed;
        }

        public void GetListOfActions() {
            //
            GameObject p = gameCharacter.gameObject;
        }

    }
