using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;

public class Agent : MonoBehaviour
{
   
    public Character character;
    public HexTile   currentlyOn;

    public HealthBar healthBar;
    public Projectile projectile;

    public void Start()
    {
        GameObject tmp = (GameObject)Resources.Load("HealthBar", typeof(GameObject));
        healthBar = Instantiate(tmp, transform).GetComponent<HealthBar>();
        healthBar.SetMax(Health());
    }


    void Update()
    {
        //probably animate characters here
    }

    //parker
    public void SpawnProjectile(Vector3 targetPos)
    {
        Projectile tmp = Instantiate(projectile, Vector3.zero, Quaternion.identity).GetComponent<Projectile>();
        tmp.InstantiateProjectile(currentlyOn.Position, targetPos);

    }

    public int Health()
    {
        return character.stats.health;
    }

    public int Energy()
    {
        return character.stats.energy;
    }

    public CharacterClass Type()
    {
        return character.characterclass;
    }

    public int Speed()
    {
        return character.stats.speed;
    }
    public int Range()
    {
        return character.stats.range;
    }

    public void Health(int change)
    {
        character.stats.health -= change;
        healthBar.ChangeHealth(Health());
    }

    public void Energy(int change)
    {
        character.stats.energy += change;
    }

    public void Speed(int change) 
    {
        character.stats.speed += change;
    }

    public void AttackDmg(int change)
    {
        character.stats.attackdmg += change;
    }

    public void Projectile(Projectile p)
    {
        projectile = p;
    }
}
