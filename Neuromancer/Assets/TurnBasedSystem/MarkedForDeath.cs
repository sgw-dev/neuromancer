using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TurnBasedSystem
{
    public class MarkedForDeath : MonoBehaviour
    {
        
        public bool die=false;
        float timetodie=float.MaxValue;
        float time = 0;
        void Update()
        {
            if(die) {
                time+=Time.deltaTime;
            }
            if(time>timetodie) {
                Death();
            }
        }


        public void Setup(float time_to_die)
        {
            timetodie=time_to_die;
            die=true;
        }


        public void Death()
        {
            //some animation stuff or something idk
            Destroy(this.gameObject);
        }
    }
}