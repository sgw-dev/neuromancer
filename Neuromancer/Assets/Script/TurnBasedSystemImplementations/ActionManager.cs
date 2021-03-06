using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public static class ActionManager
{
    public static ActionFactory GetAttackFactory()
    {
        return AttackActionFactory.GetInstance();
    }

    public static ActionFactory GetMoveFactory() 
    {
        return AttackActionFactory.GetInstance();
    }
}
