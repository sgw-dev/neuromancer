using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class GameSystem {

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


            //ForcePlayersReady();
            //CreatePlayersActions();//for turn 1

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

        /*
         * Adds an action to a players list of actions that player wishes
         * to take for a turn.
         */
        public void AddCharacterAction(Action a,Player owner) {
            if (!owner.inprogress_character_actions.Contains(a)) {
                owner.inprogress_character_actions.Add(a);
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

            //combine all players moves in order to determine playout
            //CombineActions(players.ToArray());

            //execute the moves
            //ExecuteActions();
            
            //end
            TurnCleanUp();//does nothing right now
            
            //
            turn++;

            //add player back to queue
            players.Enqueue(endingturn);

        }


        // bool allReady() {
        //     bool isEveryoneReady = true;            
        //     foreach(Player p in players) {
        //         isEveryoneReady &= p.ReadyToEndTurn;
        //     }
        //     return isEveryoneReady;
        // }

        void TurnCleanUp() 
        {

            // //mark players as ready
            // ForcePlayersReady();
            // //clear players list of possible actions
            // //clear actions about to be taken(already executed)
            // for(int i = 0 ; i < players.Count; i++ ){
            //     players[i].possible_character_actions.Clear();
            //     players[i].inprogress_character_actions.Clear();
            // }
            
        }

        // void ForcePlayersReady() {
        //     for (int i = 0 ; i < PlayerCount() ; i++ )  {
        //         players[i].TurnReset();
        //     }
        // }


        public int PlayerCount() {
            return players.Count;
        }

    
        // public void CombineActions(params Player[] players) 
        // {

        //     combinedActionsSet = new List<Action>();
            
        //     foreach(Player p in players)
        //     {
        //         combinedActionsSet.AddRange(p.inprogress_character_actions);
        //     }

        //     //compare based on character speed, see compare to in character class
        //     combinedActionsSet.Sort((a1,a2) => 
        //             a1.TakenBy().CompareTo(a2.TakenBy())
        //     );

        // }

        /*
         * Calls action interface to execute actions,
         * Each action should know "how" each action
         * is to be executed based on class
         */
        // void ExecuteActions()
        // {

        //     foreach(Action a in combinedActionsSet) 
        //     {
        //         a.Execute();
        //     }

        // }

        public List<Player> Players() 
        {
                return new List<Player>(players.ToArray());
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