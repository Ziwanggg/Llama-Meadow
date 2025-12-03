using UnityEngine;

public class Plant : MonoBehaviour
{
    public float growTime = 2f;   // how long until fully grown
    private float timer = 0f;

    private bool isGrown = false;
    private Plot parentPlot;

    // Called when Plant is spawned
    public void SetParentPlot(Plot plot)
    {
        parentPlot = plot;
    }

    void Update()
    {
        // Do nothing if pause/start/game over
        if (!GameManager.Instance.GameStarted ||
            GameManager.Instance.IsPaused ||
            GameManager.Instance.IsGameOver)
        {
            return;
        }

        // Growing
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

        // Optional: grow visual effect before disappearing
        transform.localScale = Vector3.one * 1.5f;

        // Tell GameManager a plant finished growing
        GameManager.Instance.PlantGrew();

        // Tell the plot to change dirt → grass
        if (parentPlot != null)
        {
            parentPlot.PlantFinished();
        }

        // Remove the green circle after growth
        Destroy(gameObject);
    }
}
