using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CameraScoller 
{
    public class CameraMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("How fast the camera moves on the axis")]
        public float movespeed;
        
        [Tooltip("Only Use horizontal or vertical")]
        public bool horizontal;
        [Tooltip("Only Use horizontal or vertical")]
        public bool vertical;

        //keep camera over the board
        public float maxclamp;
        public float minclamp;

        Camera main;
        bool hovered = false;

        void Start() 
        {
            main = Camera.main;
        }

        void Update()
        {
            if(hovered)
            {
                //move camera based on parameters
                main.transform.position= new Vector3(
                        Mathf.Clamp(main.transform.position.x + 
                                    movespeed*Time.deltaTime *(horizontal?1:0),
                                minclamp,maxclamp),
                        Mathf.Clamp(main.transform.position.y + 
                                    movespeed*Time.deltaTime *(vertical?1:0),
                                minclamp,maxclamp),
                            main.transform.position.z 
                        );
            }
        }

        public void OnPointerEnter(PointerEventData data)
        {
            hovered = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            hovered = false;
        }

    }
}