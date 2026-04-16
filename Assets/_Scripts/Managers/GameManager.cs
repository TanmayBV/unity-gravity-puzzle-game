using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    //Game States
    public enum GameState
    {
        Start,
        Playing,
        GameOver,
        Win
    }

    public GameState currentState;

    [Header("References")]
    public PlayerController player;
    public UI timer;
    public GameObject GameMenu;
    public GameObject GamePlayUI;
    public GameObject GameOver;

    public TextMeshProUGUI wintext;

    [Header("Settings")]
    public float fallThreshold = 50f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetState(GameState.Start);
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            CheckFallCondition();
        }
    }

    //STATE HANDLER
    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;

            case GameState.Playing:
                HandlePlay();
                break;

            case GameState.GameOver:
                HandleGameOver();
                break;

            case GameState.Win:
                HandleWin();
                break;
        }
    }

    // START STATE
    void HandleStart()
    {
        Time.timeScale = 0f;
        player.EnableControl(false);

        GameMenu.SetActive(true);
    }

    // PLAY STATE
    void HandlePlay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameMenu.SetActive(false);

        Time.timeScale = 1f;
        player.EnableControl(true);

        if (timer != null)
            timer.isRunning = true;
    }

    // GAME OVER
    void HandleGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.EnableControl(false);
        Time.timeScale = 0f;
        GamePlayUI.SetActive(false);
        GameOver.SetActive(true);

    }

    // WIN
    void HandleWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.EnableControl(false);
        Time.timeScale = 0f;
        wintext.text = "Level Complete";
        GameOver.SetActive(true);
    }

    // FALL CHECK
    void CheckFallCondition()
    {
        if (player.transform.position.y < -fallThreshold || player.transform.position.y > fallThreshold)
        {
            SetState(GameState.GameOver);
        }
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    // CALLED BY TIMER
    public void OnTimeUp()
    {
        if (currentState == GameState.Playing)
        {
            SetState(GameState.GameOver);
        }
    }

    // CALLED WHEN ALL CUBES COLLECTED
    public void OnLevelComplete()
    {
        if (currentState == GameState.Playing)
        {
            SetState(GameState.Win);
        }
    }
}
