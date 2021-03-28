using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int attackRadius = gameState.selfChar.stats.range;
        // **** Spencer Notes ******
        /*  If you can attack, do
         *  Else evaluate all the locations you could possible move to, 
         *  then take the largest and move to it
         */
        List<HexTile> attackRange = htc.FindRadius(gameState.selfTile, attackRadius);
        Character closestChar = null;
        int closestDistance = int.MaxValue;
        foreach (Character character in gameState.playerChars)
        {
            int distance = htc.FindHexDistance(gameState.selfChar.gameCharacter.position, character.gameCharacter.position);
            if (distance <= attackRadius && distance < closestDistance)
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
        /* Could not find anything to attack so move to the best possible location
         */

        List<HexTile> possibleMoves = htc.FindRadius(gameState.selfTile, gameState.selfChar.stats.speed);
        possibleMoves = ValidateRadius(gameState.selfTile, possibleMoves, gameState.selfChar.stats.speed, gameState.aiChars.Concat(gameState.playerChars).ToList(), htc);
        PriorityQueue fringe = new PriorityQueue();
        foreach (HexTile hex in possibleMoves)
        {
            GameState gs = new GameState(gameState.aiChars, gameState.playerChars, hex, gameState.selfChar);
            fringe.Push((hex, null), EvaluateState(gs, htc));
        }

        if (!fringe.HasNext())
        {
            //No avialable moves, move to self
            return new MiniMove() { type = "Move", Dest = gameState.selfTile };
        }
        (HexTile state, List<int> actions) = fringe.Pop();
        return new MiniMove() { type = "Move", Dest = state };


    }
    public static float EvaluateState(GameState gameState, HexTileController htc)
    {
        //Data is going into a MinQ, smaller number is better
        float inAttackRange = 0;//Bounis if you can attack characters
        float distanceToEnemy = 0;//Minimise this
        float enemyCanAttack = 0;//Minimise this (large is bad)
        foreach(Character c in gameState.playerChars)
        {
            float distance = htc.FindHexDistance(gameState.selfTile.Position, c.gameCharacter.position);
            //If you can attack this player character
            if (distance <= gameState.selfChar.stats.range)
                //Bigger number is better, (you are subtracting this later)
                // You want to be able to attack the player, but only just. (Incase you have a longer range then them).
                // The enemy has a range of 5, you are 5 tiles away (and can attack) = good (1)
                // The enemy has a range of 5, you are 1 tile away = worst (.2)
                // The enemy has a range of 2, you are 3 tiles away = great (1.5)
                inAttackRange += distance / c.stats.range;
            else if (distance <= c.stats.range)
                //If the enemy can attack you, but you can't attack them, this is bad
                //Only slight penality, don't want the AI to be scared
                enemyCanAttack += .25f;
            distanceToEnemy += distance;
        }
        //This is a MinQ, smaller is better!!!!
        /* Minimise distance to enemys (get into attack range), 
        * subtract inAttackRange(you get bounises for being able to attack a character)
        * */
        return distanceToEnemy - inAttackRange + enemyCanAttack;
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

        int sentinel = 200;
        //Greedy search is limited to 200 iterations to find goal
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
        //**** This is a min Q !!! ***
        private List<Pair> list;
        public PriorityQueue()
        {
            list = new List<Pair>();
        }
        public void Push((HexTile, List<int>) item, float cost)
        {
            list.Add(new Pair(item, cost));
            list.Sort((a, b) => a.CompareTo(b));
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

        public class Pair
        {
            public (HexTile, List<int>) item;
            public float cost;
            public Pair((HexTile, List<int>) i, float c)
            {
                item = i;
                cost = c;
            }
            public int CompareTo(Pair p)
            {
                
                return cost.CompareTo(p.cost);
            }
        }
    }
}
