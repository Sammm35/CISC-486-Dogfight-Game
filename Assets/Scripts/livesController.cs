using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class livesController : MonoBehaviour
{
    [SerializeField] private Text pLives;
    [SerializeField] private Text eLives;
    public int startingLives;
    int playerLives;
    int enemyLives;
    int itemLives = 10;
    public static int winner = 0;

    private void Start()
    {
        playerLives = startingLives;
        enemyLives = startingLives;
        updateText();
    }

    public void hit(int plane)
    // planes lose 1 life for getting hit
    // plane 0 is player
    // plane 1 is enemy
    {
        if (plane == 0 && playerLives > 0)
        {
            playerLives--;
        }
        else if (plane == 1 && enemyLives > 0)
        {
            enemyLives--;
        }
        updateText();
        EndGameCheck();
    }

    public void crash(int plane)
    // planes lose half lives for crashing with a minumum of 3 lives lost
    // plane 0 is player
    // plane 1 is enemy
    {
        if (plane == 0)
        {
            if (playerLives <= 6)
            {
                playerLives -= 3;
                if (playerLives <= 0) { playerLives = 0; }
            }
            else { playerLives = playerLives / 2; }
        }
        else if (plane == 1)
        {
            if (enemyLives <= 6)
            {
                enemyLives -= 3;
                if (enemyLives <= 0) { enemyLives = 0; }
            }
            else { enemyLives = enemyLives / 2; }
        }
        updateText();
        EndGameCheck();
    }

    public void itemCollected(int plane)
    // planes gains health when picking up an item
    // plane 0 is player
    // plane 1 is enemy
    {
        if (plane == 0)
        {
            playerLives += itemLives;
        }
        else if (plane == 1)
        {
            enemyLives += itemLives;
        }
        updateText();
    }

    void updateText()
    {
        pLives.text = "" + playerLives;
        eLives.text = "" + enemyLives;

    }

    void EndGameCheck()
    {
        // winner 0 is a draw, winner 1 is player wins, winner 2 is NPC wins
        if (playerLives == 0)
        {
            winner = 2;
            SceneManager.LoadScene(2);
        }
        else if (enemyLives == 0)
        {
            winner = 1;
            SceneManager.LoadScene(2);
        }
    }
}
