using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    private bool canJump = true;
    public bool isWalking = false;

    public Camera mainCamera;

    public GameObject[] platformPrefabs;
    public Animator playerAnim;

    public AudioSource jumpSound;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private Rigidbody2D playerRb;

    private float highestCameraY;
    private float highestPlatformY;
    private float highestPlayerY;

    private float score;
    private float highScore;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

        highestCameraY = mainCamera.transform.position.y;
        highestPlayerY = transform.position.y;
        highestPlatformY = transform.position.y - 2f;

        score = 0;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);

        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }

        UpdateScoreUI();
    }

    void Update()
    {
        playerAnimation();
        MovePlayer();
        MoveCamera();
        SpawnPlatformsAbovePlayer();
        UpdateScore();
        CheckDeath();
    }

    void MovePlayer()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            transform.position = transform.position + new Vector3(-0.05f , 0f, 0f);
            transform.localScale = new Vector2(-2.445469f, 2.445469f);
            isWalking = true;


        }

        if (Keyboard.current.dKey.isPressed)
        {
            transform.position = transform.position + new Vector3(0.05f, 0f, 0f);
            transform.localScale = new Vector2(2.445469f, 2.445469f);
            isWalking = true;
            
        }

        if ((Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && canJump)
        {
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 10f);
            playerJumpSound();
            canJump = false;
        }
        else
        {

        }
        
    }

    void playerAnimation()
    {
        if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            playerAnim.SetTrigger("Run");
        }
        else
        {
            playerAnim.SetTrigger("Idle");
        }
    }

    void playerJumpSound()
    {
        jumpSound.Play();
    }
    
    void MoveCamera()
    {
        if (transform.position.y > highestCameraY)
        {
            highestCameraY = transform.position.y;
        }

        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, highestCameraY, mainCamera.transform.position.z);
    }

    void SpawnPlatformsAbovePlayer()
    {
        while (highestPlatformY < transform.position.y + 10f)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        float randomX = Random.Range(0f, 3f);

        highestPlatformY += 3f;

        Vector2 spawnPosition = new Vector2(randomX, highestPlatformY);

        int randomPlatform = Random.Range(0, platformPrefabs.Length);

        Instantiate(platformPrefabs[randomPlatform], spawnPosition, Quaternion.identity);
    }

    void UpdateScore()
    {
        if (transform.position.y > highestPlayerY)
        {
            highestPlayerY = transform.position.y;
            score = highestPlayerY;

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetFloat("HighScore", highScore);
                PlayerPrefs.Save();
            }

            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString("F1");
        highScoreText.text = "High Score: " + highScore.ToString("F1");
    }

    void CheckDeath()
    {
        float cameraBottom = mainCamera.transform.position.y - 6f;

        if (transform.position.y < cameraBottom)
        {
            SceneManager.LoadScene("gameplay");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;
        }
    }
}