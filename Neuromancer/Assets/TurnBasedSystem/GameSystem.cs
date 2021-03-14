using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class GameSystem {

        public static GameSystem currentGame; 

        int             turn;        //indicates the round
        Queue<Player>   players;     //set of players
        
        //ScriptableObject Template
        public PredefinedTestCharacterStats char_stat_loader;

        List<Action>    combinedActionsSet;
        List<Character> character_pool;


        /*
         * Initial Game Setup
         * Creates players
         */
        public GameSystem(params Player[] all)     
        {

            turn = 0;
            players = new Queue<Player>();

            for (int i = 0 ; i < all.Length; i++) 
            {
                players.Enqueue(all[i]);
            }

            currentGame = this;

        }

        /*
         * Give Players their set of characters
         */
        public void AssignPlayerCharacters(Player p, params CharacterClass[] classes)
        {
            //create classes.length characters for player p
            foreach(CharacterClass c in classes){
                p.characters.Add(
                        c.ToString(),
                        CharacterFactory.getInstance().CreateCharacter(c)
                    );
            }
        }

        public string WhosTurn() 
        {
            return players.Peek().name;
        }

        public void EndTurn(Player p) {

            //Check if the player trying to end, is actually whos turn it is
            if (!players.Peek().Equals(p)) 
            {
                Debug.Log(p.name+" can't end " + players.Peek().name +"'s turn");
                return;
            }

            //get player reference
            Player endingturn = players.Dequeue();
            
            //end
            TurnCleanUp(endingturn);//does nothing right now
            

            turn++;
            Debug.Log(endingturn.name + " turn end.");

            //add player back to queue
            players.Enqueue(endingturn);

        }

        //reset some flags
        void TurnCleanUp(Player p) 
        {
            
            //reset the characters
            foreach(Character c in p.characters.Values) 
            {
                c.ActionTakenThisTurn = false;
            }

        }


        public int PlayerCount() {
            return players.Count;
        }

        public bool ExecuteCharacterAction(Player p,Action totake)
        {
            //check to see if the character is owned by player
            if(!p.characters.ContainsValue(totake.TakenBy())) {
                Debug.LogError(p.name + " can't use " + totake.TakenBy() );
                return false;
            }

            //otherwise take the action
            totake.Execute();

            return true;
        }

        public List<Player> Players() 
        {
                return new List<Player>(players.ToArray());
        }

        public static GameSystem CurrentGame() 
        {
            if(currentGame ==null )
            {
                Debug.LogError("Game has not started yet.");
            }

            return currentGame;
        }

        #if UNITY_EDITOR
            // public void TEST_CreatePlayersActions() {
            //     CreatePlayersActions();
            // }

            public List<Action> TEST_combinedActionsSet() 
            {
                return combinedActionsSet;
            }


            public List<Character> TEST_character_pool() {
                return character_pool;
            }

            
        #endif

    }

}