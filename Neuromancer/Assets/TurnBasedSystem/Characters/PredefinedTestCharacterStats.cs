using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

[CreateAssetMenu(fileName = "CharacterTestStats", menuName = "ScriptableObjects/CharacterTestingStats", order = 1)]
public class PredefinedTestCharacterStats : ScriptableObject {

	public int health;
	public int energy;
	public int speed;
	public int character_class;
	
}
