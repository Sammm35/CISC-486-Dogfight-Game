using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class winTextScript : MonoBehaviour
{
    private TMP_Text txt;
   
    void Start()
        // Sets the text based on who won the game in the previous scene:
    {
        txt = GetComponent<TMP_Text>();
        if (livesController.winner == 1)
        {
            txt.text = "You Win!";
        }
        else if (livesController.winner == 2)
        {
            txt.text = "You Lose.";
        }
        else
        {
            txt.text = "Game Over.";
        }
        
    }
}
