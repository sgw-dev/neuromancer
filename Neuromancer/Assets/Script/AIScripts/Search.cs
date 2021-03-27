using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Search
{
    public class GameState
    {
        public List<Character> aiChars;
        public List<Character> playerChars;
        public HexTile selfTile;
        public Character selfChar;
        public GameState(List<Character> ai, List<Character> pc, HexTile st, Character sc)
        {
            aiChars = ai;
            playerChars = pc;
            selfTile = st;
            selfChar = sc;
        }
    }
    public class MiniAction
    {
        public String type;
    }
    public class MiniAttack : MiniAction
    {
        public Character toAttack;
    }
    public class MiniMove : MiniAction
    {
        public HexTile Dest;
    }
    public static MiniAction DecideAction(GameState gameState, HexTileController htc)
    {
        int radius = gameState.selfChar.stats.range;
        // **** Spencer Notes ******
        /*  Could possible imporve this by checking if any of the 4 player
         *  Characters are in range for an attack, instead of checking every hex
         *  in range for a character.
         *  
         *  Note: Had to do this since ai was attacking its own team
         */
        List<HexTile> attackRange = htc.FindRadius(gameState.selfTile, radius);
        Character closestChar = null;
        int closestDistance = int.MaxValue;
        foreach (Character character in gameState.playerChars)
        {
            int distance = htc.FindHexDistance(gameState.selfChar.gameCharacter.position, character.gameCharacter.position);
            if (distance <= radius && distance < closestDistance)
            {
                closestDistance = distance;
                closestChar = character;
            }
        }
        if(closestDistance < int.MaxValue)
        {
            return new MiniAttack() { type="Attack", toAttack = closestChar};
        }
        // **** Spencer Notes ******
        /* Could not find anything to attack so move towards the closest player character
         */
        HexTile destTile = null;
        closestDistance = int.MaxValue;
        foreach (Character character in gameState.playerChars)
        {
            int distance = htc.FindHexDistance(gameState.selfChar.gameCharacter.position, character.gameCharacter.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                destTile = htc.FindHex(character.gameCharacter.position);
            }
        }
        if (closestDistance < int.MaxValue)
        {
            return new MiniMove() { type = "Move", Dest = destTile };
        }
        return new MiniAction() { type = "fail"};

    }

    public static List<HexTile> ValidateRadius(HexTile startTile, List<HexTile> group, int steps, List<Character> allChars, HexTileController htc)
    {
        List<HexTile> result = new List<HexTile>();
        foreach (HexTile hex in group)
        {
            if (!IsOnCharatcer(hex.Position, allChars) && !hex.IsObstacle)
            {
                List<int> path = Search.GreedySearch(startTile, hex, htc);
                if (path.Count <= steps)
                {
                    result.Add(hex);
                }
            }
        }
        return result;
    }
    private static bool IsOnCharatcer(Vector3 position, List<Character> allChars)
    {
        foreach (Character c in allChars)
        {
            if (c.gameCharacter.position.Equals(position))
            {
                return true;
            }
        }
        return false;
    }

    public static List<int> GreedySearch(HexTile start, HexTile end, HexTileController htc)
    {
        List<HexTile> closeSet = new List<HexTile>();
        PriorityQueue fringe = new PriorityQueue();

        fringe.Push((start, new List<int>()), 0);

        int sentinel = 100;
        
        while (fringe.HasNext() && sentinel > 0){
            (HexTile state, List<int> actions) = fringe.Pop();
            if (state.Position.Equals(end.Position))
            {
                return actions;
            }
            if (!closeSet.Contains(state))
            {
                closeSet.Add(state);
            }

            int action = 0;
            foreach(HexTile nextState in state.nexts)
            {
                //Check if the next tile is the goal state, 
                // if it is, allow the AI to step onto it
                bool validMove = false;
                if(nextState != null)
                {
                    if(nextState.HoldingObject == null)
                    {
                        validMove = true;
                    }else if (nextState.Position.Equals(end.Position))
                    {
                        validMove = true;
                    }
                }
                if(validMove)
                {
                    List<int> newActions = new List<int>();
                    foreach (int i in actions)
                    {
                        newActions.Add(i);
                    }
                    newActions.Add(action);
                    float cost = htc.FindHexDistance(nextState.Position, end.Position);
                    fringe.Push((nextState, newActions), cost);
                }
                
                action++;
            }
            sentinel--;
        }
        return new List<int>() { };
        
    }
    
    public class PriorityQueue
    {
        
        private List<Pair> list;
        public PriorityQueue()
        {
            list = new List<Pair>();
        }
        public void Push((HexTile, List<int>) item, float cost)
        {
            list.Add(new Pair(item, cost));
            list.Sort((a, b) => a.cost.CompareTo(b.cost));
        }
        public bool HasNext()
        {
            return list.Count > 0;
        }
        public (HexTile, List<int>) Pop()
        {
            Pair p = list[0];
            list.RemoveAt(0);
            return p.item;
        }

        private class Pair
        {
            public (HexTile, List<int>) item;
            public float cost;
            public Pair((HexTile, List<int>) i, float c)
            {
                item = i;
                cost = c;
            }
        }
    }
}
