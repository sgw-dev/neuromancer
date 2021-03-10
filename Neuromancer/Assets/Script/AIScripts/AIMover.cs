using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMover : MonoBehaviour
{
    public GameObject agent;
    public PointerController pc;

    private HexTile hexTile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            hexTile = pc.HexTile;
            agent.transform.position = hexTile.Position;
        }
    }
}
