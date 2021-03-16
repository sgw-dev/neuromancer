using System;
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

        [Test]
        public void CharacterPlayerDictionaryReturnedByGameSystem()
        {
            Player     me    = new Player("me");
            Player     notme = new Player("not_me");
            GameSystem gs    = new GameSystem(me,notme);
            CharacterClass[] charactersInGame = 
                new CharacterClass[] {
                    CharacterClass.MELEE,
                    CharacterClass.RANGED,
                    CharacterClass.PSYONIC,
                    CharacterClass.HACKER
                    };
            gs.AssignPlayerCharacters(me,charactersInGame);
            gs.AssignPlayerCharacters(notme,charactersInGame); 
            Dictionary<Player,Character[]> tmp = gs.GetPlayersCharacters();
            
            Assert.NotNull(tmp);
            Assert.AreEqual(tmp.Count,2);
            
            Character[] chs = new Character[charactersInGame.Length];
            tmp.TryGetValue(me,out chs);
            Array.ForEach(chs,c => Debug.Log(c.name));

            tmp.TryGetValue(notme,out chs);
            Array.ForEach(chs,c => Debug.Log(c.name));


        }

        [Test]
        public void CharactersReturnedByGameSystem()
        {
            Player     me    = new Player("me");
            Player     notme = new Player("not_me");
            GameSystem gs    = new GameSystem(me,notme);
            CharacterClass[] charactersInGame = 
                new CharacterClass[] {
                    CharacterClass.MELEE,
                    CharacterClass.RANGED,
                    CharacterClass.PSYONIC,
                    CharacterClass.HACKER
                    };
            gs.AssignPlayerCharacters(me,charactersInGame);
            gs.AssignPlayerCharacters(notme,charactersInGame); 

            Assert.AreEqual(gs.AllCharacters().Count,charactersInGame.Length * gs.Players().Count);

        }

    }
}
