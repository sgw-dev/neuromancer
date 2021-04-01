using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject winText;
    public GameObject loseText;
    public GameObject tieText;

    public GameObject blocker;
    public GameObject playagain;

    bool calledonce = false;
    public void GameOverFor(Player p)
    {
        if(!calledonce) {
            if(p.name.Equals("PLAYER_2"))//the player wins
            {
                Debug.Log("Victory!");
                blocker.SetActive(true);
                winText.SetActive(true);
                playagain.SetActive(true);
            } 
            else if(p.name.Equals("PLAYER_1")) //the ai wins
            {
                Debug.Log("Better luck next time");
                blocker.SetActive(true);
                loseText.SetActive(true);
                playagain.SetActive(true);
            }
            else
            {
                Debug.LogError("Something is wrong with player names");

            }
            calledonce=true;
        }
    }

    public void Tie()
    {
        Debug.Log("ITS A TIE");
        blocker.SetActive(true);
        tieText.SetActive(true);
        playagain.SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}