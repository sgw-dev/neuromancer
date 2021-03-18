using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;


public class MoveActionFactory : ActionFactory
{
    static MoveActionFactory factory;

    public List<Action> GetActions(Character c) {
        List<Action> actions = new List<Action>();
        //actions.Add(new MoveAction(c,2));
        return actions;
    }

    public Action CreateAction(Character c, params Vector3[] moves)
    {
        return new MoveAction(c,moves[0]);
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
