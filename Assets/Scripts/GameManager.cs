using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Win / Lose Settings")]
    public int targetPlants = 5;
    public float timeLimit = 60f;

    [Header("UI")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI startText;
    public TextMeshProUGUI pauseText;

    [Header("Prefabs")]
    public GameObject plantPrefab;

    private int grownPlants = 0;
    private float timer;

    public bool IsGameOver { get; private set; }
    public bool GameStarted { get; private set; }
    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        timer = timeLimit;
        IsGameOver = false;
        GameStarted = false;
        IsPaused = false;

        if (messageText != null) messageText.text = "";
        if (infoText != null) UpdateInfoText();

        // show start, hide pause
        if (startText != null) startText.gameObject.SetActive(true);
        if (pauseText != null) pauseText.gameObject.SetActive(false);
    }

    void Update()
    {
        // --- START SCREEN ---
        if (!GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            return; // do not run timer or anything yet
        }

        // --- RESTART AFTER GAME OVER ---
        if (IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        // --- PAUSE TOGGLE ---
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (IsPaused)
        {
            return; // freeze timer and growth
        }

        // --- NORMAL GAME TIMER ---
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            CheckLose();
        }
        UpdateInfoText();
    }

    void StartGame()
    {
        GameStarted = true;
        if (startText != null)
        {
            startText.gameObject.SetActive(false);
        }
    }

    void TogglePause()
    {
        IsPaused = !IsPaused;

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(IsPaused);
        }

        // optional: also stop Unity time (not strictly needed)
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    void UpdateInfoText()
    {
        if (infoText != null)
        {
            infoText.text = $"Plants grown: {grownPlants}/{targetPlants}\nTime left: {timer:F1}s";
        }
    }

    public void SpawnPlantAt(Plot plot)
    {
        if (!GameStarted || IsGameOver) return;

        Vector3 spawnPos = plot.transform.position;
        GameObject plantObj = Instantiate(plantPrefab, spawnPos, Quaternion.identity);

        Plant plant = plantObj.GetComponent<Plant>();
        if (plant != null)
        {
            plant.SetParentPlot(plot);
        }
    }

    public void PlantGrew()
    {
        if (IsGameOver) return;

        grownPlants++;
        UpdateInfoText();

        if (grownPlants >= targetPlants)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        IsGameOver = true;
        if (pauseText != null) pauseText.gameObject.SetActive(false);
        if (messageText != null)
        {
            messageText.text = "You win! The llamas are proud.\nPress R to restart.";
        }
        Time.timeScale = 1f;
    }

    void CheckLose()
    {
        if (grownPlants < targetPlants)
        {
            LoseGame();
        }
    }

    void LoseGame()
    {
        IsGameOver = true;
        if (pauseText != null) pauseText.gameObject.SetActive(false);
        if (messageText != null)
        {
            messageText.text = "You lose. Not enough plants.\nPress R to restart.";
        }
        Time.timeScale = 1f;
    }
}
