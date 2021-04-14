using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDebug : MonoBehaviour
{
    public GameObject textPrefab;
    private List<GameObject> texts;
    // Start is called before the first frame update
    void Start()
    {
        texts = new List<GameObject>();
    }

    public void SetText(HexTile hex, List<int> moves)
    {
        /*Vector3 screenPos = Camera.main.WorldToScreenPoint(hex.Position);
        string s = "";
        foreach(int i in moves)
        {
            s += "" + i + ", ";
        }
        GameObject text = Instantiate(textPrefab, this.gameObject.transform.parent);
        text.transform.position = screenPos;
        //text.GetComponent<RectTransform>().localPosition = screenPos;
        text.GetComponent<Text>().text = s;
        texts.Add(text);*/
    }
    public void SetText(HexTile hex, float value)
    {
        /*
        Vector3 screenPos = Camera.main.WorldToScreenPoint(hex.Position);
        string s = ""+value;
        
        GameObject text = Instantiate(textPrefab, this.gameObject.transform.parent);
        text.transform.position = screenPos;
        //text.GetComponent<RectTransform>().localPosition = screenPos;
        text.GetComponent<Text>().text = s;
        texts.Add(text);*/
    }
    public void ClearAll()
    {
        for(int i = 0; i< texts.Count; i++)
        {
            Destroy(texts[i]);
        }
        texts.Clear();
        /*foreach(GameObject text in texts)
        {
            texts.Remove(text);
            Destroy(text);
        }*/
    }
}
