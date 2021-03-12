using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackActionFactory : ActionFactory
{

    static AttackActionFactory factory;


    public static AttackActionFactory GetInstance()
    {
        if(factory is null) {
            return new AttackActionFactory();
        }
        return factory;
    }


    //options - check all actions -works for bot
    //        - check actions given though GUI forces bot to enumerate all actions
    public List<Action> GetActions(Character c) 
    {
        List<Action> possibleActions = new List<Action>();
        //check something
            //check character class
            //check surrounding tiles that can be attacked
            //create actions

        //still needs to change
        possibleActions.Add(new AttackAction(c,2,AttackType.SINGLE,10));
        
        return possibleActions;
    }

    
    AttackActionFactory()
    {
        if(factory==null) 
        {
            factory=new AttackActionFactory();
        }
    }

}
