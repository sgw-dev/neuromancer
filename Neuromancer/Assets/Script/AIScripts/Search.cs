using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Search
{

    public static List<int> GreedySearch(HexTile start, HexTile end, HexTileController htc)
    {
        List<HexTile> closeSet = new List<HexTile>();
        PriorityQueue fringe = new PriorityQueue();

        fringe.Push((start, new List<int>()), 0);
        
        while (fringe.HasNext()){
            (HexTile state, List<int> actions) = fringe.Pop();
            if (state.Position.Equals(end.Position))
            {
                return actions;
            }
            if (!closeSet.Contains(state))
            {
                closeSet.Add(state);
            }

            int action = 0;
            foreach(HexTile nextState in state.nexts)
            {
                if(nextState != null && !nextState.IsObstacle)
                {
                    List<int> newActions = new List<int>();
                    foreach (int i in actions)
                    {
                        newActions.Add(i);
                    }
                    newActions.Add(action);
                    float cost = htc.FindHexDistance(nextState.Position, end.Position);
                    fringe.Push((nextState, newActions), cost);
                }
                
                action++;
            }
        }
        return new List<int>() { };
        
    }
    
    public class PriorityQueue
    {
        
        private List<Pair> list;
        public PriorityQueue()
        {
            list = new List<Pair>();
        }
        public void Push((HexTile, List<int>) item, float cost)
        {
            list.Add(new Pair(item, cost));
            list.Sort((a, b) => a.cost.CompareTo(b.cost));
        }
        public bool HasNext()
        {
            return list.Count > 0;
        }
        public (HexTile, List<int>) Pop()
        {
            Pair p = list[0];
            list.RemoveAt(0);
            return p.item;
        }

        private class Pair
        {
            public (HexTile, List<int>) item;
            public float cost;
            public Pair((HexTile, List<int>) i, float c)
            {
                item = i;
                cost = c;
            }
        }
    }
}
