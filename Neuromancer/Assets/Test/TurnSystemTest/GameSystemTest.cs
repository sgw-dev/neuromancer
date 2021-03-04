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
        public void LoadCharacterTemplates() { 
            Assert.AreEqual(
                CharacterFactory.getInstance().CreateCharacter(CharacterClass.MELEE).characterclass,
                CharacterClass.MELEE);
            Assert.AreEqual(
                CharacterFactory.getInstance().CreateCharacter(CharacterClass.RANGED).characterclass,
                CharacterClass.RANGED);
            Assert.AreEqual(
                CharacterFactory.getInstance().CreateCharacter(CharacterClass.HACKER).characterclass,
                CharacterClass.HACKER);
            Assert.AreEqual(
                CharacterFactory.getInstance().CreateCharacter(CharacterClass.PSYONIC).characterclass,
                CharacterClass.PSYONIC);
            Assert.AreEqual(
                CharacterFactory.getInstance().CreateCharacter(CharacterClass.TEST).characterclass,
                CharacterClass.TEST);
        }

        [Test]
        public void GameWithPlayerNumGreaterThanZero() {
            GameSystem gametest = new GameSystem(new Player("Me"),new Player("TEST"));
            Assert.IsTrue(gametest.PlayerCount()>0);
        }


        [Test]
        public void OneCharacterActionPerTurn() {
            //create players
            Player     me    = new Player("me");
            Player     notme = new Player("not_me");

            //make the gamesystem
            GameSystem gs    = new GameSystem(me,notme);


            //Let gs make the charcter load and assign characters
            CharacterClass[] charactersInGame = 
                new CharacterClass[] {
                    CharacterClass.MELEE,
                    CharacterClass.RANGED,
                    CharacterClass.PSYONIC,
                    CharacterClass.HACKER
                    };

            gs.AssignPlayerCharacters(me,charactersInGame);
            gs.AssignPlayerCharacters(notme,charactersInGame);
            
            //get actions
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

            //action factory not finished, update this as needed
            Assert.AreEqual(16,gs.TEST_combinedActionsSet().Count);
        }

    }
}
