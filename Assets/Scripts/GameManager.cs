using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int targetPlants = 5;
    public float timeLimit = 60f;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI infoText;
    public GameObject plantPrefab;

    private int grownPlants = 0;
    private float timer;
    public bool IsGameOver { get; private set; }

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
        messageText.text = "";
        UpdateInfoText();
    }

    void Update()
    {
        if (!IsGameOver)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                CheckLose();
            }
            UpdateInfoText();
        }

        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateInfoText()
    {
        infoText.text = $"Plants grown: {grownPlants}/{targetPlants}\nTime left: {timer:F1}s";
    }

    public void SpawnPlantAt(Plot plot)
    {
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
        messageText.text = "You win! Llama meadow is full.\nPress R to restart.";
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
        messageText.text = "You lose. Not enough plants.\nPress R to restart.";
    }
}
