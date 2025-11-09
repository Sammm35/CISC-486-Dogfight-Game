using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class livesController : MonoBehaviour
{
    [SerializeField] private Text pLives;
    [SerializeField] private Text eLives;
    [SerializeField] private AudioClip hit1;
    [SerializeField] private AudioClip hit2;
    [SerializeField] private AudioClip hit3;
    [SerializeField] private AudioClip hitMarker;
    [SerializeField] private AudioClip pop;
    private AudioSource audioSource;
    public int startingLives;
    public Transform pPos;
    public Transform ePos;
    int playerLives;
    int enemyLives;
    int itemLives = 10;
    public static int winner = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerLives = startingLives;
        enemyLives = startingLives;
        updateText();
    }

    public void hit(int plane)
    // planes lose 1 life for getting hit
    // plane 0 is player
    // plane 1 is enemy
    {
        int rand = Random.Range(0, 3);
        AudioClip clip = hit1;
        if (rand == 0 ) { clip = hit1; }
        else if (rand == 1) { clip = hit2; }
        else { clip = hit3; }
        if (plane == 0 && playerLives > 0)
        {
            playerLives--;
            AudioSource.PlayClipAtPoint(clip, pPos.position, 1);
        }
        else if (plane == 1 && enemyLives > 0)
        {
            enemyLives--;
            AudioSource.PlayClipAtPoint(clip, ePos.position, 1);
            AudioSource.PlayClipAtPoint(hitMarker, pPos.position, 0.45f);
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
            AudioSource.PlayClipAtPoint(pop, pPos.position, 1);
            audioSource.Play();

        }
        else if (plane == 1)
        {
            enemyLives += itemLives;
            AudioSource.PlayClipAtPoint(pop, ePos.position, 1);
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
