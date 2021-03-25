using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class AttackActionFactory : ActionFactory
{

    static AttackActionFactory factory;

    //for now values[0] should be the position, and anything else after
    public Action CreateAction(Character c, params Vector3[] values)
    {
        return new AttackAction(c,values[0]);//,(AttackType)values[1].x,(int)values[1].y);
    }

    public static AttackActionFactory GetInstance()
    {
        if(factory is null) {
            return new AttackActionFactory();
        }
        return factory;
    }
    /*
    AttackActionFactory()
    {
        if(factory==null) 
        {
            factory=new AttackActionFactory();
        }
    }*/

}
