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

    List<HexTile> surrounding;

    void Start()
    {
        activeChar = null;
        PlayerButtons.SetActive(false);
        CancelButton.SetActive(false);
        surrounding = new List<HexTile>();
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
                    if (surrounding.Contains(pointer.HexTile))
                    {
                        HexTile startTile = htc.FindHex(activeChar.gameCharacter.position);
                        List<int> path = Search.GreedySearch(startTile, pointer.HexTile, htc);
                        List<Vector3> moves = new List<Vector3>();
                        if (path.Count > 0)
                        {
                            moves.Add(startTile.nexts[path[0]].Position);
                            for (int i = 1; i < path.Count; i++)
                            {
                                int neighbor = path[i];
                                moves.Add(htc.FindHex(moves[i - 1]).nexts[path[i]].Position);

                            }
                        }
                        Action a = MoveActionFactory.getInstance().CreateAction(activeChar, moves.ToArray());
                        GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                        moving = false;
                        inSecondarySelect = false;
                        ButtonCover.SetActive(true);
                        CancelButton.SetActive(false);
                        pointer.SetCanHighlight(true);
                        foreach (HexTile ht in surrounding)
                            ht.setHighlight(false);
                    }
                    
                }
            }
            if (attacking)
            {
                if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (surrounding.Contains(pointer.HexTile))
                    {
                        bool isOnCharacter = false;
                        List<Character> allChars = GameSystem.CurrentGame().AllCharacters();
                        foreach (Character c in allChars)
                        {
                            if (c.gameCharacter.position.Equals(pointer.HexTile.Position))
                            {
                                isOnCharacter = true;
                            }
                        }
                        if (isOnCharacter)
                        {
                            Action a = AttackActionFactory.GetInstance().CreateAction(activeChar, pointer.HexTile.Position);
                            GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                            attacking = false;
                            inSecondarySelect = false;
                            ButtonCover.SetActive(true);
                            CancelButton.SetActive(false);
                            pointer.SetCanHighlight(true);
                            foreach (HexTile ht in surrounding)
                                ht.setHighlight(false);
                        }                        
                    }

                }
            }

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
                pointer.SetCanHighlight(true);
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
        Action a = MoveActionFactory.getInstance().CreateAction(activeChar, activeChar.gameCharacter.position);
        GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
        ButtonCover.SetActive(true);
    }

    public void Move()
    {
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        moving = true;

        pointer.SetCanHighlight(false);

        surrounding = htc.FindRadius(htc.FindHex(activeChar.gameCharacter.position), activeChar.stats.speed, true);
        foreach (HexTile ht in surrounding)
            ht.setHighlight(true);
    }
    public void Attack() {
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        attacking = true;

        pointer.SetCanHighlight(false);

        surrounding = htc.FindRadius(htc.FindHex(activeChar.gameCharacter.position), activeChar.stats.range, false);
        foreach (HexTile ht in surrounding)
            ht.setHighlight(true);
    }
    public void Cancel()
    {
        CancelButton.SetActive(false);
        inSecondarySelect = false;
        if(surrounding != null)
        {
            foreach (HexTile ht in surrounding)
                ht.setHighlight(false);
        }
        pointer.SetCanHighlight(true);
    }
}
