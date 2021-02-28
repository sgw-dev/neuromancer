using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TurnBasedSystem
{
    public class MoveAction : Action
    {
        Character takenby;
        int moveto;
        public MoveAction(Character c, int tile) 
        {
            takenby = c;
            moveto = tile;
        }

        public int MoveTo(int tile) 
        {
            //Code to describe how to move to a tile
            return tile;
        }

        void Action.Execute() 
        {
            MoveTo(moveto);
        }

        public Character TakenBy() 
        {
            return takenby;
        }
    }
}