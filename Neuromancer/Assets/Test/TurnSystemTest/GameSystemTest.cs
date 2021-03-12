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
        public void GameSetup() {
            //create players
            Player     me    = new Player("me");
            Player     notme = new Player("not_me");

            //make the gamesystem
            GameSystem gs    = new GameSystem(me,notme);
            //this should be easier to read
            CharacterClass[] charactersInGame = 
                new CharacterClass[] {
                    CharacterClass.MELEE,
                    CharacterClass.RANGED,
                    CharacterClass.PSYONIC,
                    CharacterClass.HACKER
                    };
            gs.AssignPlayerCharacters(me,charactersInGame);
            gs.AssignPlayerCharacters(notme,charactersInGame);     

            //control should be in players hands now
            
            //other stuff ...

        }



    }
}
