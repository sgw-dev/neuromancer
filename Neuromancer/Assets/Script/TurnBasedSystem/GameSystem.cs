using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public class GameSystem {

        int            turn;        //indicates the round
        List<Player>   players;     //set of players
        
        //should have a some pattern here 
        //only need to know that it is a template, not what kind
        public PredefinedTestCharacterStats char_stat_loader;

        List<Action> combinedActionsSet;
        
        List<Character> character_pool;


        /*
         * Initial Game Setup
         */
        public GameSystem(params Player[] all)     
        {

            turn=0;
            players = new List<Player>();

            for (int i = 0 ; i < all.Length; i++) 
            {
                players.Add(all[i]);
            }


            ForcePlayersReady();
            CreatePlayersActions();
        }


        /*
         * Call on game initialiation to read templates
         * and create characte pools
         */
        public Character CreateCharacter(string charname,PredefinedTestCharacterStats template)
        {
            Character c = CharacterFactory.getInstance().CreateCharacter((Class)template.character_class);
            c.stats = new Stat{
                health = template.health,
                energy = template.energy,
                speed = template.speed
            };
            c.name=charname;
            return c;
        }

        public void AssignPlayerCharacters(Player p, params Character[] chars)
        {
            foreach(Character c in chars){
                p.characters.Add(c.name,c);
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
            //mark all players as a new turn begins

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

        void TurnCleanUp() {
            //mark players as ready
            ForcePlayersReady();
            //populate next turns moves, and remove other
            CreatePlayersActions();
        }

        void ForcePlayersReady() {

            for (int i = 0 ; i < PlayerCount() ; i++ ) 
            {
                players[i].TurnReset();
            }

        }

        void CreatePlayersActions() {

            for (int i = 0 ; i < PlayerCount() ; i++ ) 
            {
                //clear their list
                players[i].possible_character_actions.Clear();
                //get new list
                players[i].possible_character_actions 
                    = ActionFactory.GetPlayerActions(players[i]);
            }
            
        }

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
            public void TEST_CreatePlayersActions() {
                CreatePlayersActions();
            }

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