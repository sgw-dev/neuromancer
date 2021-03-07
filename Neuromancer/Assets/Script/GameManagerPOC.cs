using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class GameManagerPOC : MonoBehaviour
{
    public GameSystem gameSystem;
    void Start()
    {
        Player player1 = new Player("PLAYER_1");
        Player player2 = new Player("PLAYER_2");
        gameSystem = new GameSystem(player1, player2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void PutCharactersOnBoard() {
        //literally put the characters onto tiles
    }

    public void GetActions() {
        //display in the gui actions available to character selected
        //get the actions from the ActionManger/fact/
    }

    public void AddActionTo(Player p , Action a) {
        //put the action into the players list of actions
        // gameSystem.AddCharacterAction();
    }

    public void CreateAllActions() {
        //not sure about this yet
    }

}
