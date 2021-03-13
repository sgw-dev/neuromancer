using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class GameManagerPOC : MonoBehaviour
{
    public GameSystem gameSystem;

    [Tooltip("Put in order of the enum")]
    public Sprite[] sprites;

    public GameObject characterPrefab;
    
    CharacterClass[] classes = 
        new CharacterClass[] {
                CharacterClass.MELEE,
                CharacterClass.RANGED,
                CharacterClass.PSYONIC,
                CharacterClass.HACKER
            };
    

    //creates players,
    //the game,
    //the character objects
    //
    void Start()
    {
        Player player1 = new Player("PLAYER_1");
        Player player2 = new Player("PLAYER_2");
        gameSystem = new GameSystem(player1, player2);
        
        gameSystem.AssignPlayerCharacters(player1,classes);
        gameSystem.AssignPlayerCharacters(player2,classes);

        Material sprites_default = new Material(Shader.Find("Sprites/Default"));;
        //create the actual gameobject
        for(int i = 0 ; i < gameSystem.Players().Count; i++) 
        {
            Player p = gameSystem.Players()[i];
            foreach(Character c in p.characters.Values)
            {
                //rename characters
                c.name=p.name+"_"+c.name;

                GameObject tmp = new GameObject(p.name+" "+c.characterclass.ToString());
                //add the MonoBehaviour OR ELSE! 
                PlayerCharacter ctmp = tmp.AddComponent<PlayerCharacter>();
                ctmp.character = c;//dont forget 
                c.gameCharacter = tmp.transform;
                //this part is fine
                SpriteRenderer renderer = tmp.AddComponent<SpriteRenderer>();
                renderer.sprite=sprites[(int)c.characterclass];
                renderer.material = sprites_default;
                //hide until ready
                renderer.enabled = false;
                
            }
        }
    }

    //call from button
    public void EndRound() 
    {
        for( int i = 0 ; i < gameSystem.PlayerCount() ; i++ )
        {
            //should rotate through all players
            gameSystem.EndTurn(gameSystem.Players()[0]);
        }
        Debug.Log("FORCED ROUND ENDED");
    }

    //filler code for now
    public void PutCharactersOnBoard()
    {
        
        GameObject tc = GameObject.Find("TileController");
        
        foreach(Player p in gameSystem.Players())
        {
            foreach(Character c in p.characters.Values)
            {
                //Debug.Log(c.name);
                //game is ready, show characters
                SpriteRenderer r = c.gameCharacter.gameObject.GetComponent<SpriteRenderer>();
                r.enabled = true;
            }
        }

        GameObject.Find("Start").SetActive(false);
    }

    // public void GetActions() {
    //     //display in the gui actions available to character selected
    //     //get the actions from the ActionManger/fact/
    // }

    // public void AddActionTo(Player p , Action a) {
    //     //put the action into the players list of actions
    //     // gameSystem.AddCharacterAction();
    // }

    // public void CreateAllActions() {
    //     //not sure about this yet
    // }

}
