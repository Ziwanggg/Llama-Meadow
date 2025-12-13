using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Rules")]
    public int targetPlants = 5;
    public float timeLimit = 60f;

    [Header("Plant Selection")]
    public PlantType SelectedPlantType = PlantType.Grass;

    [Header("Prefabs")]
    public GameObject grassPlantPrefab;
    public GameObject flowerPlantPrefab;

    [Header("UI (TextMeshProUGUI)")]
    public TextMeshProUGUI messageText; // win/lose
    public TextMeshProUGUI infoText;    // timer + count
    public TextMeshProUGUI startText;   // press space
    public TextMeshProUGUI pauseText;   // paused

    [Header("Economy")]
    public int money = 10;
    public int grassSeeds = 0;
    public int flowerSeeds = 0;
    public int grassSeedCost = 2;
    public int flowerSeedCost = 4;
    public int grassGrowReward = 1;
    public int flowerGrowReward = 2;

    [Header("UI Economy")]
    public TMPro.TextMeshProUGUI moneyText;


    private int grownPlants = 0;
    private float timer;

    public bool GameStarted { get; private set; }
    public bool IsPaused { get; private set; }
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

        GameStarted = false;
        IsPaused = false;
        IsGameOver = false;

        Time.timeScale = 1f;

        if (messageText != null) messageText.text = "";
        if (pauseText != null) pauseText.gameObject.SetActive(false);

        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            startText.text = "Press SPACE to start";
        }

        UpdateInfoText();
    }

    void Update()
    {
        if (!GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            return;
        }

        if (IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (SpendMoney(grassSeedCost))
            {
                grassSeeds++;
                UpdateMoneyUI();
                ShowMessage("Bought 1 grass seed.");
                
            }
            else
            {
                ShowMessage("Not enough money for grass seed.");
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (SpendMoney(flowerSeedCost))
            {
                flowerSeeds++;
                UpdateMoneyUI();
                ShowMessage("Bought 1 flower seed.");
            }
            else
            {
                ShowMessage("Not enough money for flower seed.");
            }
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (IsPaused) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            LoseGame();
        }

        UpdateInfoText();
    }

    void StartGame()
    {
        GameStarted = true;
        if (startText != null) startText.gameObject.SetActive(false);
    }

    void TogglePause()
    {
        IsPaused = !IsPaused;

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(IsPaused);
            if (IsPaused)
            {
                pauseText.text = "Paused\nPress P or ESC to resume";
            }
        }

        Time.timeScale = IsPaused ? 0f : 1f;
    }

    void UpdateInfoText()
    {
        if (infoText == null) return;

        infoText.text =
            "Plants grown: " + grownPlants + "/" + targetPlants +
            "\nTime left: " + timer.ToString("F1") + "s" +
            "\nSelected: " + SelectedPlantType + "  (Press 1=Grass, 2=Flower)";
    }

    public void SpawnPlantAt(Plot plot, PlantType type)
    {
        if (!GameStarted || IsPaused || IsGameOver) return;

        GameObject prefabToSpawn =
            (type == PlantType.Flower) ? flowerPlantPrefab : grassPlantPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("Missing plant prefab in GameManager (grass or flower).");
            return;
        }

        Vector3 spawnPos = plot.transform.position;
        GameObject plantObj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        Plant plant = plantObj.GetComponent<Plant>();
        if (plant != null)
        {
            plant.Init(plot, type);
        }
    }

    public void ShowMessage(string msg)
    {
        if (messageText == null) return;
        messageText.text = msg;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), 1.5f);
    }

    void ClearMessage()
    {
        if (IsGameOver) return;   // keep win/lose text
        if (messageText != null) messageText.text = "";
    }

    public void PlantGrew()
    {
        if (IsGameOver) return;

        grownPlants++;

        if (grownPlants >= targetPlants)
        {
            WinGame();
        }
    }

    public bool CanAfford(int cost)
    {
        return money >= cost;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        money -= amount;
        UpdateMoneyUI();
        return true;
    }

    public void UpdateMoneyUI()
    {
        if (moneyText == null) return;

        moneyText.text =
            $"Money: ${money}\n" +
            $"Grass Seeds: {grassSeeds} (Cost ${grassSeedCost})\n" +
            $"Flower Seeds: {flowerSeeds} (Cost ${flowerSeedCost})\n" +
            $"Selected: {SelectedPlantType}  (1=Grass, 2=Flower)";
    }


    void WinGame()
    {
        IsGameOver = true;
        Time.timeScale = 1f;

        if (pauseText != null) pauseText.gameObject.SetActive(false);

        if (messageText != null)
        {
            messageText.text = "You win! The farm looks great.\nPress R to restart.";
        }
    }

    void LoseGame()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Time.timeScale = 1f;

        if (pauseText != null) pauseText.gameObject.SetActive(false);

        if (messageText != null)
        {
            messageText.text = "You lose. Time ran out.\nPress R to restart.";
        }
    }
}
