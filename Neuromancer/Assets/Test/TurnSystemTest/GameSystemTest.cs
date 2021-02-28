using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEditor;
using TurnBasedSystem;

namespace Tests
{

    public class GameSystemTest
    {

        [Test]
        public void PlayerInstatiated() {
            GameSystem gametest = new GameSystem(new Player("Me"),new Player("TEST"));
            Assert.IsTrue(gametest.PlayerCount()>0);
        }

        [Test]
        public void CreateCharacterWithStats() {
            Character char1 = new Character("TEST",Class.TEST,new Stat{health = 100, energy = 100, speed = 33});
            Assert.Greater(char1.stats.health,0);
            Assert.Greater(char1.stats.energy,0);
            Assert.Greater(char1.stats.speed,0);
        }

        [Test]
        public void LoadCharactersFromScriptable() {

        }

        [Test]
        public void OneCharacterActionPerTurn() {
            Player     me    = new Player("me");
            Player     notme = new Player("not_me");

            GameSystem gs    = new GameSystem(me,notme);

            Character me_char_1 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            Character me_char_2 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            Character me_char_3 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            Character notme_char_1 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            Character notme_char_2 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            Character notme_char_3 = CharacterFactory.getInstance().CreateCharacter(Class.TEST);
            me_char_1.stats.speed = 13;    me_char_1.name="p1_c1";
            me_char_2.stats.speed = 2;     me_char_2.name="p1_c2";
            me_char_3.stats.speed = 15;    me_char_3.name="p1_c3";
            notme_char_1.stats.speed = 4;  notme_char_1.name="p2_c1";
            notme_char_2.stats.speed = 20; notme_char_2.name="p2_c2";
            notme_char_3.stats.speed = 11; notme_char_3.name="p2_c3";

            gs.AssignPlayerCharacters(me,me_char_1,me_char_2,me_char_3);
            gs.AssignPlayerCharacters(notme,notme_char_1,notme_char_2,notme_char_3);
            
            
            gs.TEST_CreatePlayersActions();//calls gamesystem CreatePlayersActions
            

            //try adding a bunch of actions to each players list
            for(int i = 0 ; i < me.possible_character_actions.Count ; i++) {
                gs.AddCharacterAction(me.possible_character_actions[i],me);
            }
            gs.AddCharacterAction(me.possible_character_actions[2],me);//added twice
            gs.AddCharacterAction(me.possible_character_actions[3],me);//added twice
            for(int i = 0 ; i < notme.possible_character_actions.Count ; i++) {
                gs.AddCharacterAction(me.possible_character_actions[i],notme);
            }
            gs.AddCharacterAction(me.possible_character_actions[1],notme);//added twice
            gs.AddCharacterAction(me.possible_character_actions[1],notme);//added twice
            gs.AddCharacterAction(me.possible_character_actions[1],notme);//added twice

            
            gs.CombineActions(me,notme);//dont actually call this from a playmode, should rely
                                        //on gs.EndTurn();
            //12 comes from the 6 charactesr each being given 2 actions 1 move and one attack
            Assert.AreEqual(12,gs.TEST_combinedActionsSet().Count);
            


        }

         [Test]
        public void LoadCharactersFromTemplateAndAssign() {
            Player me = new Player("me");
            Player notme = new Player("not_me");
            GameSystem gs = new GameSystem(me, notme);
            CharacterFactory.getInstance();

        }


    }
}
