using System;
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

        //create the game
        gameSystem = new GameSystem(player1, player2);
        
        //make sure all players get their characters, for >2 make a loop
        gameSystem.AssignPlayerCharacters(player1,classes);
        gameSystem.AssignPlayerCharacters(player2,classes);

        //assign the player script to the human player
        GameObject.Find("Player").GetComponent<PlayerController>().player = player1;

        Material sprites_default = new Material(Shader.Find("Sprites/Default"));;
        
        //create the actual sprites and tie the scripts to gameobjects
        for(int i = 0 ; i < gameSystem.Players().Count; i++) 
        {
            Player p = gameSystem.Players()[i];
            foreach(Character c in p.characters.Values)
            {
                //rename characters
                c.name=p.name+"_"+c.name;
                GameObject tmp = new GameObject(p.name+" "+c.characterclass.ToString());

                //add the MonoBehaviour OR ELSE! 
                Agent ctmp = tmp.AddComponent<Agent>();

                //reference to mono and script
                ctmp.character = c;//dont forget 
                c.gameCharacter = tmp.transform;

                //put the sprite onto
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
        RandomlyPlace();
    }

    // public void AddActionTo(Player p , Action a) {
    //     //put the action into the players list of actions
    //     // gameSystem.AddCharacterAction();
    // }

    //get rid of this 
    public void RandomlyPlace(){
        HexTileController htc = GameObject.Find("TileController").GetComponent<HexTileController>();

        Debug.Log(htc.Head.Position);
        // hexTileController.FindHex(mousePos, hexTile);

        int totalcharactercount = GameSystem.CurrentGame().PlayerCount() * classes.Length;

        HashSet<HexTile> tiles = new HashSet<HexTile>();
        while(tiles.Count<totalcharactercount) {
            try{
                tiles.Add(
                    htc.FindHex(new Vector3(
                                        UnityEngine.Random.Range(-10f,10f),
                                        UnityEngine.Random.Range(-10f,10),0f)
                                        )
                );
            } catch(Exception e){}
        }

        HexTile[] tilearray = new HexTile[totalcharactercount];
        tiles.CopyTo(tilearray);
        
        int cc = 0;

        foreach(Player p in GameSystem.CurrentGame().Players()) {
            foreach(Character c in p.characters.Values) {
                
                Agent a = c.gameCharacter.GetComponent<Agent>();
                // c.gameCharacter.position
                //update character ref
                a.currentlyOn = tilearray[cc];
                a.transform.position = tilearray[cc].transform.position;
                //update tile ref
                tilearray[cc].SetObject(c.gameCharacter.gameObject,false);
                cc++;
            }
        }

    }
}
