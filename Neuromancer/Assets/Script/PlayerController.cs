using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public HexTileController htc;
    public PointerController pointer;

    private Character activeChar;
    private bool inSecondarySelect;
    private bool attacking;
    private bool moving;

    public GameObject PlayerButtons;
    public GameObject ButtonCover;

    public GameObject CancelButton;

    void Start()
    {
        activeChar = null;
        PlayerButtons.SetActive(false);
        CancelButton.SetActive(false);
    }

    
    void Update()
    {
        //If it is my turn
        if (player.name.Equals(GameSystem.CurrentGame().WhosTurn().name))
        {
            List<Character> myChars = new List<Character>(player.characters.Values);
            if (Input.GetButtonDown("Fire1"))
            {
                if(activeChar == null)
                {
                    Vector3 pos = pointer.HexTile.Position;
                    foreach (Character c in myChars)
                    {
                        if (c.gameCharacter.position.Equals(pos))
                        {
                            activeChar = c;
                        }
                    }
                }
                if(activeChar != null && !inSecondarySelect)
                {
                    Debug.Log("Active character is " + activeChar.name);
                    //Show action buttons
                    PlayerButtons.SetActive(true);
                    if (activeChar.ActionTakenThisTurn)
                    {
                        Debug.Log("Cannot act");
                        ButtonCover.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Can act");
                        ButtonCover.SetActive(false);
                    }
                }

            }
            if (moving)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Action a = MoveActionFactory.getInstance().CreateAction(activeChar, pointer.HexTile.Position);
                    GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                    moving = false;
                    inSecondarySelect = false;
                    ButtonCover.SetActive(true);
                    CancelButton.SetActive(false);
                }
            }

            //Debug.Log(player.name +"(Player) Turn");




            //Determining if the player is all done
            bool allDone = true;
            foreach(Character character in myChars)
            {
                if (!character.ActionTakenThisTurn)
                {
                    allDone = false;
                }
            }
            if (allDone)
            {
                EndMyTurn();
            }
            
        }
    }


    public void EndMyTurn() 
    {
        GameSystem.CurrentGame().EndTurn(player);
    }
    public void Wait()
    {

    }

    public void Move()
    {
        //demonstration
        /*foreach (Character c in player.characters.Values)
        {
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            Action a = MoveActionFactory.getInstance().CreateAction(c, new Vector3(x, y, 0), new Vector3(3f, 3f, 3f));
            GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
        }*/
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        moving = true;
        /*Action a = MoveActionFactory.getInstance().CreateAction(activeChar, pointer.HexTile.Position);
        GameSystem.CurrentGame().ExecuteCharacterAction(player, a);*/
    }
    public void Attack() {
        // Action a = AttackActionFactory.getInstance().CreateAction();
    }
    public void Cancel()
    {
        CancelButton.SetActive(false);
        inSecondarySelect = false;
    }
}
