using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TurnBasedSystem
{

	public class CharacterFactory
	{
		static CharacterFactory factory;

		public static CharacterFactory getInstance() {
			if(factory == null) {
				return new CharacterFactory();
			}
			return factory;
		}

		CharacterFactory() 
		{
			//load templates into a pool, maybe a Dictionary
		}

		public Character CreateCharacter(Class characterclass)
		{
			//should have the factory load templates

			//this is temp for testing
			return new Character("NONAME",characterclass,new Stat{health=0,speed=0,energy=0});
		}

		
		//helper to mimic attached scriptable objects
        //https://answers.unity.com/questions/1425758/how-can-i-find-all-instances-of-a-scriptable-objec.html
        T[] GetAllInstances<T>() where T : ScriptableObject {
            string[] guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for(int i =0;i<guids.Length;i++) {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
        }
		
	}
}