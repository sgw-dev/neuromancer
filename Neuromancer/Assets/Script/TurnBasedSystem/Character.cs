﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class Character : IComparable {
        public string name;
        public Stat stats;
        public CharacterClass characterclass;

        public bool ActionTakenThisTurn;
        
        public Character(string name,CharacterClass cclass,Stat charstats) 
        {
            this.name = name;
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
        }
    }
}