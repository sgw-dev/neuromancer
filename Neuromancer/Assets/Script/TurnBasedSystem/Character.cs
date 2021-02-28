using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class Character : IComparable,ICloneable {
        public string name;
        public Stat stats;
        public Class characterclass;

        public bool ActionTakenThisTurn;
        
        public Character(string name,Class cclass,Stat charstats) 
        {
            this.name = name;
            stats = charstats;
            characterclass = cclass;
            ActionTakenThisTurn=false;
        }

        public object Clone() {
            return new Character(name,characterclass,stats);
        }

        public int CompareTo(object obj) 
        {
            return ((Character)obj).stats.speed-stats.speed;
        }
    }
}