using UnityEngine;

public class Plant : MonoBehaviour
{
    public float growTime = 2f;

    private float timer = 0f;
    private bool isGrown = false;

    private Plot parentPlot;
    private PlantType plantType;

    public void Init(Plot plot, PlantType type)
    {
        parentPlot = plot;
        plantType = type;
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted ||
            GameManager.Instance.IsPaused ||
            GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (!isGrown)
        {
            timer += Time.deltaTime;
            if (timer >= growTime)
            {
                BecomeGrown();
            }
        }
    }

    void BecomeGrown()
    {
        isGrown = true;

        // reward money
        int reward = (plantType == PlantType.Flower)
            ? GameManager.Instance.flowerGrowReward
            : GameManager.Instance.grassGrowReward;

        GameManager.Instance.AddMoney(reward);

        GameManager.Instance.PlantGrew();

        if (parentPlot != null)
        {
            parentPlot.PlantFinished(plantType);
        }

        Destroy(gameObject);
    }

}
