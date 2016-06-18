using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Text highscoreText;
    public GameObject levelButton, aboutButton, desertButton;

    int highscore = 0;
    bool about = false;
    bool choseLevel = false;

    void Awake()
    {
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        highscoreText.text = "Best:\n" + highscore.ToString();
        aboutButton.GetComponent<Text>().text = "Beat your record and recieve aditional jumps!\nJumps = best / 50 + 2 = " + (highscore/50+2).ToString();
    }

    public void DesertButton()
    {
        SceneManager.LoadScene("Desert");
    }

    public void ChoseLevel()
    {
        if (choseLevel == false)
        {
            aboutButton.SetActive(false);
            desertButton.SetActive(true);
        }
        else if(choseLevel == true)
        {
            aboutButton.SetActive(true);
            desertButton.SetActive(false);
        }
        choseLevel = !choseLevel;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
