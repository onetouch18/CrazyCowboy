using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float forwardMovementSpeed = 5.0f;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    public Texture2D starIconTexture;
    public AudioClip starCollectSound;
    public GameObject restartButton, musicButton, pauseButton, quitButton, moveButton, highScoreElement;
    public AudioSource footstepsAudio;
    public static bool grounded;

    Animator animator;
    public Text text, highscoreText;
    bool dead = false;
    static int highscore;
    int stars = 0;


    void Awake()
    {
        animator = GetComponent<Animator>();
        highscore = PlayerPrefs.GetInt("highscore", highscore);
    }

    void OnGUI()
    {
        DisplayStarsCount();
        DisplayRestartButton();
    }

    void Update()
    {
        if (!dead)
        {
            Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = forwardMovementSpeed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustFootstepsSound();
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("grounded", grounded);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        HitByObstacle(collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Stars"))
            CollectStars(collider);
    }

    void CollectStars(Collider2D starCollider)
    {
        stars++;
        //PlayerPrefs.SetInt("stars", stars);
        //PlayerPrefs.Save();

        Destroy(starCollider.gameObject);

        AudioSource.PlayClipAtPoint(starCollectSound, transform.position);
    }

    void HitByObstacle(Collision2D cactusCollision)
    {
        cactusCollision.gameObject.GetComponent<AudioSource>().Play();
        dead = true;

        animator.SetBool("dead", true);
    }

    void DisplayStarsCount()
    {
        text.text = stars.ToString();
        highscoreText.text = "Best:\n" + highscore.ToString();
    }

    void DisplayRestartButton()
    {
        if (dead)
        {
            pauseButton.SetActive(false);
            moveButton.SetActive(false);
            restartButton.SetActive(true);
            musicButton.SetActive(true);
            quitButton.SetActive(true);
            if (highscore > stars)
            {
                SetHighscore(highscore);
            }
            else
            {
                highscore = stars;
                SetHighscore(highscore);
            }
            highScoreElement.SetActive(true);
        }
    }

    void AdjustFootstepsSound()
    {
        footstepsAudio.enabled = !dead && grounded && Time.timeScale > 0;
    }

    void SetHighscore(int highscore)
    {
        highscoreText.text = "Best:\n" + highscore.ToString();
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
    }
}
