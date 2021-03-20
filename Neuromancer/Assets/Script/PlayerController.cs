using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;
using UnityEngine.EventSystems;

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
            if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
            {
                if(!inSecondarySelect)
                {
                    Vector3 pos = pointer.HexTile.Position;
                    //Assume no characters are selected (hide buttons)
                    PlayerButtons.SetActive(false);
                    activeChar = null;
                    foreach (Character c in myChars)
                    {
                        if (c.gameCharacter.position.Equals(pos))
                        {
                            activeChar = c;
                            c.SetSelected(true);
                            PlayerButtons.SetActive(true);
                            if (activeChar.ActionTakenThisTurn)
                            {
                                ButtonCover.SetActive(true);
                            }
                            else
                            {
                                ButtonCover.SetActive(false);
                            }
                        }
                        else
                        {
                            c.SetSelected(false);
                        }
                    }
                    if (activeChar != null)
                    {
                        
                    }
                }
                

            }
            if (moving)
            {
                if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
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
                PlayerButtons.SetActive(false);
                foreach (Character character in myChars)
                {
                    character.SetSelected(false);
                }
                activeChar = null;
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
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        moving = true;
    }
    public void Attack() {
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        attacking = true;
    }
    public void Cancel()
    {
        CancelButton.SetActive(false);
        inSecondarySelect = false;
    }
}
