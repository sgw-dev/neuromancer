using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class GameSystem {

        int            turn;        //indicates the round
        List<Player>   players;     //set of players
        
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
            players = new List<Player>();

            for (int i = 0 ; i < all.Length; i++) 
            {
                players.Add(all[i]);
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


        public void EndTurn() {

            //check if all players are ready for the turns to end
            if (!allReady()) 
            {
                return;
            }
            //combine all players moves in order to determine playout
            CombineActions(players.ToArray());
            //execute the moves
            ExecuteActions();
            
            //end
            TurnCleanUp();

            turn++;
        }


        bool allReady() {
            
            bool isEveryoneReady = true;
            
            foreach(Player p in players)
            {
                isEveryoneReady &= p.ReadyToEndTurn;
            }
            
            return isEveryoneReady;
        }

        void TurnCleanUp() 
        {

            //mark players as ready
            ForcePlayersReady();
            //clear players list of possible actions
            //clear actions about to be taken(already executed)
            for(int i = 0 ; i < players.Count; i++ ){
                players[i].possible_character_actions.Clear();
                players[i].inprogress_character_actions.Clear();
            }
            
        }

        void ForcePlayersReady() {

            for (int i = 0 ; i < PlayerCount() ; i++ ) 
            {
                players[i].TurnReset();
            }

        }

        //should rely on outside 
        // void CreatePlayersActions() {

        //     //come back to this
            
        //     for (int i = 0 ; i < PlayerCount() ; i++ ) 
        //     {
        //         //clear their list
        //         players[i].possible_character_actions.Clear();
        //         //get new list
        //         // players[i].possible_character_actions 
        //         //     = ActionManager.GetAttackFactory().GetPlayerActions(players[i]);

        //         foreach(Character kv in players[i].characters.Values) {
        //             players[i].possible_character_actions.AddRange(
        //                 ActionManager.GetAttackFactory().GetActions(kv)
        //             );
        //         }
        //     }
            
        // }

        public int PlayerCount() {
            return players.Count;
        }

    
        public void CombineActions(params Player[] players) 
        {
            
            combinedActionsSet = new List<Action>();
            
            foreach(Player p in players)
            {
                combinedActionsSet.AddRange(p.inprogress_character_actions);
            }

            //compare based on character speed, see compare to in character class
            combinedActionsSet.Sort((a1,a2) => 
                    a1.TakenBy().CompareTo(a2.TakenBy())
            );

        }

        /*
         * Calls action interface to execute actions,
         * Each action should know "how" each action
         * is to be executed based on class
         */
        void ExecuteActions()
        {

            foreach(Action a in combinedActionsSet) 
            {
                a.Execute();
            }

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

            public List<Player> TEST_players() {
                return players;
            }
        #endif

    }

}