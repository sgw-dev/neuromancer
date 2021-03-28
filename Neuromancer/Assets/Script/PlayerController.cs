using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Button EndTurnButton;

    public GameObject CancelButton;

    private List<HexTile> surrounding;
    private List<HexTile> subSelect;

    public Color defaultHighlight;
    public Color attackHighlight;
    public Color hackerAttack;
    public Color moveHighlight;
    public Color doneHighlight;

    [SerializeField]
    public List<Character> myChars;

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
            EndTurnButton.interactable = true;
            myChars = new List<Character>(player.characters.Values);
            //Debug.Log("I see " + myChars.Count + " Characters");
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
                            
                            PlayerButtons.SetActive(true);
                            if (activeChar.ActionTakenThisTurn)
                            {
                                ButtonCover.SetActive(true);
                                c.SetSelected(true, doneHighlight);
                            }
                            else
                            {
                                ButtonCover.SetActive(false);
                                c.SetSelected(true, defaultHighlight);
                            }
                        }
                        else
                        {
                            c.SetSelected(false);
                        }
                    }
                    //DebugCharacters();
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
                        activeChar.SetSelected(true, doneHighlight);
                        moving = false;
                        inSecondarySelect = false;
                        ButtonCover.SetActive(true);
                        CancelButton.SetActive(false);
                        pointer.SetCanHighlight(true);
                        foreach (HexTile ht in surrounding)
                            ht.setHighlight(false);
                    }
                    //DebugCharacters();
                }
            }
            if (attacking)
            {
                if (activeChar.characterclass == CharacterClass.HACKER)
                {
                    if (surrounding.Contains(pointer.HexTile))
                    {
                        if (subSelect != null) { 
                            foreach (HexTile ht in subSelect)
                                ht.setHighlight(false);
                        }
                        foreach (HexTile ht in surrounding)
                            ht.setHighlight(true);
                        subSelect = htc.FindRadius(pointer.HexTile, 2);
                        foreach (HexTile ht in subSelect)
                            ht.setHighlight(true, hackerAttack);
                    }
                }
                if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (surrounding.Contains(pointer.HexTile))
                    {
                        if(activeChar.characterclass == CharacterClass.HACKER)
                        {
                            Action a = AttackActionFactory.GetInstance().CreateAction(activeChar, pointer.HexTile.Position);
                            GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                            activeChar.SetSelected(true, doneHighlight);
                            attacking = false;
                            inSecondarySelect = false;
                            ButtonCover.SetActive(true);
                            CancelButton.SetActive(false);
                            pointer.SetCanHighlight(true);
                            foreach (HexTile ht in surrounding)
                                ht.setHighlight(false);
                            foreach (HexTile ht in subSelect)
                                ht.setHighlight(false);
                        }
                        if (IsOnCharatcer(pointer.HexTile.Position))
                        {
                            Action a = AttackActionFactory.GetInstance().CreateAction(activeChar, pointer.HexTile.Position);
                            GameSystem.CurrentGame().ExecuteCharacterAction(player, a);
                            activeChar.SetSelected(true, doneHighlight);
                            attacking = false;
                            inSecondarySelect = false;
                            ButtonCover.SetActive(true);
                            CancelButton.SetActive(false);
                            pointer.SetCanHighlight(true);
                            foreach (HexTile ht in surrounding)
                                ht.setHighlight(false);
                        }
                        //DebugCharacters();
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
                EndMyTurn();
            }

        }
        else
        {
            EndTurnButton.interactable = false;
        }
    }

    public bool IsOnCharatcer(Vector3 position)
    {
        List<Character> allChars = GameSystem.CurrentGame().AllCharacters();
        foreach (Character c in allChars)
        {
            if (c.gameCharacter.position.Equals(position))
            {
                return true;
            }
        }
        return false;
    }

    public void DebugCharacters()
    {
        Debug.Log("***Character States ***");
        foreach (Character character in myChars)
        {
            Debug.Log(character.name + " has taken it's action? " + character.ActionTakenThisTurn);
        }
        Debug.Log("--------------------");
    }

    public void EndMyTurn() 
    {
        GameSystem.CurrentGame().EndTurn(player);
        PlayerButtons.SetActive(false);
        foreach (Character character in myChars)
        {
            character.SetSelected(false);
        }
        if (surrounding != null)
        {
            foreach (HexTile ht in surrounding)
                ht.setHighlight(false);
        }
        if (subSelect != null)
        {
            foreach (HexTile ht in subSelect)
                ht.setHighlight(false);
        }
        activeChar = null;
        pointer.SetCanHighlight(true);
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
        attacking = false;

        activeChar.SetSelected(true, moveHighlight);

        pointer.SetCanHighlight(false);

        foreach (HexTile ht in subSelect)
            ht.setHighlight(false);
        foreach (HexTile ht in surrounding)
            ht.setHighlight(false);
        surrounding = htc.FindRadius(htc.FindHex(activeChar.gameCharacter.position), activeChar.stats.speed);
        surrounding = Search.ValidateRadius(htc.FindHex(activeChar.gameCharacter.position), surrounding, activeChar.stats.speed, GameSystem.CurrentGame().AllCharacters(), htc);
        foreach (HexTile ht in surrounding)
            ht.setHighlight(true);
    }
    public void Attack() {
        CancelButton.SetActive(true);
        inSecondarySelect = true;
        attacking = true;
        moving = false;

        activeChar.SetSelected(true, attackHighlight);
        pointer.SetCanHighlight(false);

        foreach (HexTile ht in surrounding)
            ht.setHighlight(false);
        surrounding = htc.FindRadius(htc.FindHex(activeChar.gameCharacter.position), activeChar.stats.range, false);
        foreach (HexTile ht in surrounding)
            ht.setHighlight(true);
    }
    public void Cancel()
    {
        CancelButton.SetActive(false);
        inSecondarySelect = false;
        attacking = false;
        moving = false;
        if(surrounding != null)
        {
            foreach (HexTile ht in surrounding)
                ht.setHighlight(false);
        }
        if (subSelect != null)
        {
            foreach (HexTile ht in subSelect)
                ht.setHighlight(false);
        }
        pointer.SetCanHighlight(true);
        activeChar.SetSelected(true, defaultHighlight);
    }
}
