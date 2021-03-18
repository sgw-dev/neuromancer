using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
    public interface ActionFactory 
    {
        Action CreateAction(Character c,params Vector3[] values);
    }
    
}