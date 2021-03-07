using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;


public class MoveActionFactory : ActionFactory
{
    static Object board;

    static MoveActionFactory factory;
    // static void SetGameBoard(Object tileboard) {
    //     board = tileboard;
    // }

    static Action GetAttackAction() {
        return null;
    }

    public static List<Action> GetPlayerActions(Player p) {
        List<Action> actions = new List<Action>();

        foreach(KeyValuePair<string,Character> c in p.characters)
        {
            actions.AddRange(GetCharacterAction(c.Value));
        }

        return actions;
    }

    static List<Action> GetCharacterAction(Character c) {
        List<Action> actions = new List<Action>();
        
        //find player actions
        /*
            * Once game board is implmented check the board for the characters list of actions
            * 
            */
        
        //for now give each Character one move and one attack
        // actions.Add(new AttackAction(c,1,AttackType.SINGLE));
        actions.Add(new MoveAction(c,2));

        return actions;
    }

    public List<Action> GetActions(Character c) {
        return null;
    }

    
    public static MoveActionFactory getInstance() 
    {
        if(factory is null) 
        {
            return new MoveActionFactory();
        }
        return factory;
    }
}
