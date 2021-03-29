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
    public GameObject enemyHighlight;
    public GameObject playerHighlight;
    public GameObject selectHighlight;

    private Vector3[] player1_chars = 
        new Vector3[] {
            new Vector3(10f, 0f, 0f),
            new Vector3(12f, 0f, 0f),
            new Vector3(11f, 1.5f, 0f),
            new Vector3(11f, -1.5f, 0f)
        };

    private Vector3[] player2_chars =
        new Vector3[] {
            new Vector3(-10f, 0f, 0f),
            new Vector3(-12f, 0f, 0f),
            new Vector3(-11f, 1.5f, 0f),
            new Vector3(-11f, -1.5f, 0f)
        };

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
        gameSystem.monoref = this;

        //make sure all players get their characters, for >2 make a loop
        gameSystem.AssignPlayerCharacters(player1,classes);
        gameSystem.AssignPlayerCharacters(player2,classes);

        //assign the player script to the human player
        GameObject.Find("Player").GetComponent<PlayerController>().player = player1;
        //assign the player script to the AI player
        Camera.main.GetComponent<AIMover>().player = player2;

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

                //Add the agent stats for each Character
                switch (c.characterclass)
                {
                    case CharacterClass.MELEE:
                        c.stats.health = 14;
                        c.stats.maxHealth = 14;
                        c.stats.speed = 3;
                        c.stats.range = 1;
                        c.stats.aoeRange = 0;
                        c.stats.attackdmg = 4;
                        break;
                    case CharacterClass.HACKER:
                        c.stats.health = 7;
                        c.stats.maxHealth = 7;
                        c.stats.speed = 2;
                        c.stats.range = 4;
                        c.stats.aoeRange = 1;
                        c.stats.attackdmg = 2;
                        break;
                    case CharacterClass.RANGED:
                        c.stats.health = 7;
                        c.stats.maxHealth = 7;
                        c.stats.speed = 2;
                        c.stats.range = 4;
                        c.stats.aoeRange = 0;
                        c.stats.attackdmg = 3;
                        break;
                    case CharacterClass.PSYONIC:
                        c.stats.health = 7;
                        c.stats.maxHealth = 7;
                        c.stats.speed = 3;
                        c.stats.range = 3;
                        c.stats.aoeRange = 0;
                        c.stats.attackdmg = -2;
                        break;
                }

                //put the sprite onto
                SpriteRenderer renderer = tmp.AddComponent<SpriteRenderer>();
                renderer.sprite=sprites[(int)c.characterclass];
                renderer.material = sprites_default;
                
                //hide until ready
                renderer.enabled = false;
                
            }
        }
        PutCharactersOnBoard();
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
                r.sortingLayerName = "Characters";
            }
        }
        //GameObject.Find("Start").SetActive(false);
        //RandomlyPlace();

        HexTileController htc = GameObject.Find("TileController").GetComponent<HexTileController>();

        Player player1 = GameSystem.CurrentGame().Players()[0];
        int index = 0;
        foreach (Character c in player1.characters.Values)
        {
            Agent a = c.gameCharacter.GetComponent<Agent>();
            // c.gameCharacter.position
            //update character ref
            HexTile hex = htc.FindHex(player1_chars[index]);
            a.currentlyOn = hex;
            a.transform.position = player1_chars[index];
            //update tile ref
            hex.SetObject(c.gameCharacter.gameObject, false);
            index++;

            //Add highlight to players
            Instantiate(playerHighlight, c.gameCharacter);
            GameObject temp = Instantiate(selectHighlight, c.gameCharacter);
            c.selectedHighlight = temp;
            c.SetSelected(false);
        }
        Player player2 = GameSystem.CurrentGame().Players()[1];
        index = 0;
        foreach (Character c in player2.characters.Values)
        {
            Agent a = c.gameCharacter.GetComponent<Agent>();
            // c.gameCharacter.pte character ref
            HexTile hex = htc.FindHex(player2_chars[index]);
            a.currentlyOn = hex;
            a.transform.position = player2_chars[index];
            //update tile ref
            hex.SetObject(c.gameCharacter.gameObject, false);
            index++;

            //Add highlight to enemies
            Instantiate(enemyHighlight, c.gameCharacter);
            GameObject temp = Instantiate(selectHighlight, c.gameCharacter);
            c.selectedHighlight = temp;
            c.SetSelected(false);
        }
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
