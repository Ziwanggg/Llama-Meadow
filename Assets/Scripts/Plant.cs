using UnityEngine;

public class Plant : MonoBehaviour
{
    public float growTime = 5f;
    private float timer = 0f;
    private bool isGrown = false;

    private Plot parentPlot;

    public void SetParentPlot(Plot plot)
    {
        parentPlot = plot;
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
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
        transform.localScale = Vector3.one * 1.5f;

        GameManager.Instance.PlantGrew();
        if (parentPlot != null)
        {
            parentPlot.PlantFinished();
        }
    }
}
