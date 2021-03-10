using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search
{
    public Search()
    {

    }
    public ArrayList GreedySearch(HexTile start, HexTile end)
    {
        ArrayList closeSet = new ArrayList();

        /*
        closeSet = set([])
    fringe = util.PriorityQueue()

    initialState = problem.getStartState()
    fringe.update((initialState, []), 0)

    while not fringe.isEmpty():
        state, actions = fringe.pop()
        if problem.isGoalState(state):
            return actions
        if state not in closeSet:
        closeSet.add(state)
            successors = problem.getSuccessors(state)
            #successors.append((nextState, actions, cost))
            #successors.reverse()
            for nextState, action, cost in successors:
                new_actions = actions + [action]
                h_val = heuristic(nextState, problem);
                g_val = problem.getCostOfActions(new_actions)
                fringe.update((nextState, new_actions), h_val + g_val)
                */
        return closeSet;
    }
    
    public class PriorityQueue
    {

    }
}
