using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnBasedSystem;
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
        public Vector3 attackLocation;
    }
    public class MiniMove : MiniAction
    {
        public HexTile Dest;
    }
    public static MiniAction DecideAction(GameState gameState, HexTileController htc)
    {
        MoveDebug md = GameObject.FindGameObjectWithTag("MoveDebug").GetComponent<MoveDebug>();
        int attackRadius = gameState.selfChar.stats.range;
        // **** Spencer Notes ******
        /*  If you can attack, do
         *  Else evaluate all the locations you could possible move to, 
         *  then take the largest and move to it
         */
        //If this character is the hacker, find best possible place to put the attack
        if (gameState.selfChar.characterclass == CharacterClass.HACKER)
        {
            List<HexTile> attackRange = htc.FindRadius(gameState.selfTile, attackRadius);
            PriorityQueue attackFringe = new PriorityQueue();
            md.ClearAll();
            foreach (HexTile hex in attackRange)
            {
                GameState gs = new GameState(gameState.aiChars, gameState.playerChars, hex, gameState.selfChar);
                float value = EvaluateAOEAttackState(gs, htc);
                md.SetText(hex, value);
                //If this attack will do more damage to yourself, don't add it to the list
                if (value < -1)
                {
                    attackFringe.Push((hex, null), value);
                }
                    
            }
            //If there is nothing in the list (No good attacks) Move intead
            if (attackFringe.HasNext())
            {
                ((HexTile attackHex, List<int> a), float value) = attackFringe.PopWithVal();
                //If this attack only did damage to 1 character (or 1 more than your own chars)
                /* *** Come back if you have more time ****
                if (value <= gameState.selfChar.stats.attackdmg)
                {
                    //Find see if there is a better place to attack and move to that spot

                }*/
                return new MiniAttack() { type = "AOEAttack", attackLocation = attackHex.Position };
            }

        }
        else if (gameState.selfChar.characterclass == CharacterClass.PSYONIC)
        {
            Character needestChar = null;
            Character cantReachChar = null;
            float cantReachHealth = 1;
            float leastHealth = 1;
            foreach (Character character in gameState.aiChars)
            {
                if (character.CompareTo(gameState.selfChar) != 0)
                {
                    int distance = htc.FindHexDistance(gameState.selfChar.gameCharacter.position, character.gameCharacter.position);
                    float health = character.stats.health / character.stats.maxHealth;
                    //If you can heal the character, and the health is less than 1
                    if (health < leastHealth)
                    {
                        if(distance <= attackRadius)
                        {
                            leastHealth = health;
                            needestChar = character;
                        }
                        else
                        {
                            cantReachHealth = health;
                            cantReachChar = character;
                        }
                        
                    }
                }
            }
            if (leastHealth < 1)
            {
                return new MiniAttack() { type = "Attack", toAttack = needestChar };
            }else if(cantReachHealth < 1)
            {
                //If there is a character in need that you can't reach, move towards it
                Debug.Log("Healer Moving towards " + cantReachChar.name);
                HexTile toMovTo = MoveTowards(cantReachChar, gameState, htc, true);
                return new MiniMove() { type = "Move", Dest = toMovTo };
            }
            else
            {
                //Can't heal yourself*******
                //Move towards the Ranger
                Character target = GetCharacter(gameState.aiChars, CharacterClass.RANGED);
                if(target != null)
                {
                    HexTile toMovTo = MoveTowards(target, gameState, htc, true);
                    return new MiniMove() { type = "Move", Dest = toMovTo };
                }
                target = GetCharacter(gameState.aiChars, CharacterClass.HACKER);
                if (target != null)
                {
                    HexTile toMovTo = MoveTowards(target, gameState, htc, true);
                    return new MiniMove() { type = "Move", Dest = toMovTo };
                }
                target = GetCharacter(gameState.aiChars, CharacterClass.MELEE);
                if (target != null)
                {
                    HexTile toMovTo = MoveTowards(target, gameState, htc, true);
                    return new MiniMove() { type = "Move", Dest = toMovTo };
                }
                //No other characters but you exist, await the sweet release of death
                return new MiniMove() { type = "Move", Dest = gameState.selfTile };
            }
        }
        else
        {
            Character weakestChar = null;
            Character deadChar = null;
            float leastHealth = int.MaxValue;
            foreach (Character character in gameState.playerChars)
            {
                int distance = htc.FindHexDistance(gameState.selfChar.gameCharacter.position, character.gameCharacter.position);
                
                if (distance <= attackRadius)
                {
                    //If your attack would kill this character, target this one above all else
                    if ((Mathf.Max(0, character.stats.health - gameState.selfChar.stats.attackdmg) / character.stats.maxHealth) <= 0)
                    {
                        deadChar = character;
                    }
                    if ((character.stats.health / character.stats.maxHealth) < leastHealth)
                    {
                        leastHealth = (character.stats.health / character.stats.maxHealth);
                        weakestChar = character;
                    }
                }
            }
            //Attack the soon to be dead character if it can
            if(deadChar != null)
            {
                return new MiniAttack() { type = "Attack", toAttack = deadChar };
            }
            if (weakestChar != null)
            {
                return new MiniAttack() { type = "Attack", toAttack = weakestChar };
            }
        }

        // **** Spencer Notes ******
        /* Could not find anything to attack so move to the best possible location
         */

        List<HexTile> possibleMoves = htc.FindRadius(gameState.selfTile, gameState.selfChar.stats.speed);
        List<Character> allChars = gameState.aiChars.Concat(gameState.playerChars).ToList();
        List<(HexTile, List<int>)> possibleMoves2 = ValidateRadius(gameState.selfTile, possibleMoves, gameState.selfChar.stats.speed, allChars, htc);
        //possibleMoves = ValidateRadius(gameState.selfTile, possibleMoves, gameState.selfChar.stats.speed, allChars, htc);
        possibleMoves = new List<HexTile>();
        foreach ((HexTile hex, List<int> path) in possibleMoves2)
            possibleMoves.Add(hex);
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
    public static Character GetCharacter(List<Character> list, CharacterClass cClass)
    {
        Character character = null;
        foreach(Character c in list)
        {
            if (c.characterclass == cClass)
                character = c;
        }
        return character;
    }
    public static HexTile MoveTowards(Character target, GameState gameState, HexTileController htc, bool runFromEnemy = false)
    {

        //choose the tile you want to move to, either the character directly, or a tile next to the character that is away from the enemy
        HexTile targetTile = htc.FindHex(target.gameCharacter.position);
        if (runFromEnemy)
        {
            List<HexTile> tilesAroundTarget = htc.FindRadius(targetTile, 1);
            int farthest = 0;
            foreach (HexTile hexTile in tilesAroundTarget)
            {
                if (!hexTile.IsObstacle && hexTile.HoldingObject == null)
                {
                    //Find distance to all player characters
                    int newDist = 0;
                    foreach (Character c in gameState.playerChars)
                    {
                        newDist += htc.FindHexDistance(hexTile.Position, c.gameCharacter.position);
                    }
                    //You want the distance to the player to be largest
                    if (newDist > farthest)
                    {
                        farthest = newDist;
                        targetTile = hexTile;
                    }
                }
            }
        }
        List<int> moves = GreedySearch(gameState.selfTile, targetTile, htc);

        //If run from enemy is false, your target in ON a character, so remove that last step
        if (!runFromEnemy)
        {
            //Remove the last step (the one on the distination character)
            if (moves.Count > 0)
                moves.RemoveAt(moves.Count - 1);
        }

        Debug.Log("Psyonic wants to move ...");
        foreach(int i in moves)
        {
            Debug.Log(i);
        }
        Debug.Log("To get to " + target.name);
        
        //If you were already right next to the target, don't go anywhere
        if (moves.Count == 0)
            return gameState.selfTile;
        //Only keep the number of steps that you are allowed to move
        //Starting hex is your own
        HexTile hex = gameState.selfTile;
        //Repet until you hit your step count, or you run out of moves
        for (int i = 0; i < Mathf.Min(gameState.selfChar.stats.speed, moves.Count); i++)
        {
            //Get the neighbor that the path tells you too
            hex = hex.nexts[moves[i]];
        }
        return hex;

    }
    public static float EvaluateHideState(GameState gameState, HexTileController htc)
    {
        return 0;
    }
    public static float EvaluateAOEAttackState(GameState gameState, HexTileController htc)
    {
        //Data is going into a MinQ, smaller number is better
        float aiHealthLost = 0;
        float playerLost = 0;
        List<HexTile> attackRange = htc.FindRadius(gameState.selfTile, gameState.selfChar.stats.aoeRange);
        attackRange.Add(gameState.selfTile);
        foreach (HexTile hex in attackRange)
        {
            foreach (Character c in gameState.aiChars)
            {

                if (hex.Equals(htc.FindHex(c.gameCharacter.position)))
                {
                    aiHealthLost += gameState.selfChar.stats.attackdmg;
                }
            }
            foreach (Character c in gameState.playerChars)
            {
                if (hex.Equals(htc.FindHex(c.gameCharacter.position)))
                {
                    playerLost += gameState.selfChar.stats.attackdmg;
                }
            }
        }
            

        return aiHealthLost - playerLost;
        
        
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

    public static List<(HexTile, List<int>)> ValidateRadius(HexTile startTile, List<HexTile> group, int steps, List<Character> allChars, HexTileController htc)
    {
        List<(HexTile, List<int>)> result = new List<(HexTile, List<int>)>();
        foreach (HexTile hex in group)
        {
            if (!IsOnCharatcer(hex.Position, allChars) && !hex.IsObstacle)
            {
                List<int> path = Search.GreedySearch(startTile, hex, htc);
                if (path.Count <= steps && path.Count > 0)
                {
                    result.Add((hex, path));
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
                    if(nextState.HoldingObject == null && !nextState.IsObstacle)
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
        public ((HexTile, List<int>), float) PopWithVal()
        {
            Pair p = list[0];
            list.RemoveAt(0);
            return (p.item, p.cost);
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
