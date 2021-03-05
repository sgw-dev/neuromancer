using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TurnBasedSystem
{
    public class AttackAction : Action
    {

        Character takenby;
        int space;
        public AttackAction(Character takenby,int space) {
            this.space=space;
            this.takenby=takenby;
        }

        public void Execute() {
            Debug.Log("Exectuing AttackAction by "+takenby.name);
        }

        public Character TakenBy() {
            return takenby;
        }
    }
}