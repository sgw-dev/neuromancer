using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

namespace TurnBasedSystem
{

	public class CharacterFactory
	{
		static CharacterFactory factory;
		Dictionary<CharacterClass,Stat> definedCharacters;

		public static CharacterFactory getInstance() {
			if(factory == null) {
				return new CharacterFactory();
			}
			return factory;
		}

		CharacterFactory() 
		{
			definedCharacters = LoadCharacterStats();
		}

		public Character CreateCharacter(CharacterClass characterclass)
		{
			Stat stats;
			definedCharacters.TryGetValue(characterclass,out stats);
			return new Character("",characterclass,stats);
		}

		//public T[] GetAllInstances<T>() where T:ScriptableObject {
		Dictionary<CharacterClass,Stat> LoadCharacterStats() {
			definedCharacters = new Dictionary<CharacterClass, Stat>();
			PredefinedTestCharacterStats[] so =  Resources.LoadAll<PredefinedTestCharacterStats>("Characters");
			foreach(PredefinedTestCharacterStats c in so) {
				
				definedCharacters.Add((CharacterClass)c.character_class,
                    new Stat{
						health=c.health,
						speed=c.speed,
						energy=c.energy
					});
				// Debug.Log(c.character_class + "\n" +
				//           c.name            + "\n" +
				// 		  c.speed           + "\n" +
				// 		  c.health          + "\n" +
				// 		  c.energy          + "\n" 
				// );
			}
			return definedCharacters;
		}


		
	}
}