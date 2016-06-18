using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    public GameObject gameMusic, musicButton, restartButton, quitButton, moveButton, highscore, player;
    static float musicVolume = 1.0f;
    public bool paused = false;
    int jumpTimes = 1;
    int avalaibleJumps;

    void Awake()
    {
        PlayerPrefs.GetFloat("musicVolume", musicVolume);
        gameMusic.GetComponent<AudioSource>().volume = musicVolume;
        avalaibleJumps = (PlayerPrefs.GetInt("highscore", avalaibleJumps) / 50) + 2;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        paused = false;
        Time.timeScale = 1;
    }

    public void MusicControl()
    {
        if (gameMusic.GetComponent<AudioSource>().volume == 0)
        {
            musicVolume = 1.0f;
            gameMusic.GetComponent<AudioSource>().volume = musicVolume;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.Save();
        }
        else
        {
            musicVolume = 0.0f;
            gameMusic.GetComponent<AudioSource>().volume = musicVolume;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.Save();
        }
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            musicButton.SetActive(true);
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            highscore.SetActive(true);
            moveButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            musicButton.SetActive(false);
            restartButton.SetActive(false);
            quitButton.SetActive(false);
            highscore.SetActive(false);
            moveButton.SetActive(true);
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Move()
    {
        if (PlayerController.grounded == true || jumpTimes < avalaibleJumps)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 400f));
            jumpTimes++;
            if (PlayerController.grounded == true)
                jumpTimes = 1;
        }
    }
}
