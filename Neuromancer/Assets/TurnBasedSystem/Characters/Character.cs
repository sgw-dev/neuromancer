using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;
public class Character : IComparable {
    public Transform gameCharacter;//need to get transform...etc
    public GameObject selectedHighlight;
    public string name;
    public Stat stats; 
    public CharacterClass characterclass;
     
    public bool ActionTakenThisTurn;
        
    public Character(string name,CharacterClass cclass,Stat charstats) 
    {
        this.name = name;
        stats = charstats;
        characterclass = cclass;
        ActionTakenThisTurn=false;
    }

    public int CompareTo(object obj) 
    {
        if(!(obj is Character )){
            throw new Exception(obj+" is not comparable to Character");
        }
        return ((Character)obj).name.CompareTo(name);
    }

    public void GetListOfActions() {
        //GameObject p = gameCharacter.gameObject;
    }

    public void SetSelected(bool active)
    {
        selectedHighlight.SetActive(active);
    }
    public void SetSelected(bool active, Color color)
    {
        selectedHighlight.SetActive(active);
        selectedHighlight.GetComponent<SpriteRenderer>().color = color;
    }

}
