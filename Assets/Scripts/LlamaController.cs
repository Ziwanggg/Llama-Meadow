using UnityEngine;

public class LlamaController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Plot currentPlot;

    void Update()
    {
        // ----- Game state checks -----
        if (!GameManager.Instance.GameStarted ||
            GameManager.Instance.IsGameOver ||
            GameManager.Instance.IsPaused)
        {
            return;
        }

        // ----- Select plant type -----
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.SelectedPlantType = PlantType.Grass;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.SelectedPlantType = PlantType.Flower;
        }

        // ----- Movement -----
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0f).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        // ----- Plant -----
        if (Input.GetKeyDown(KeyCode.Space) && currentPlot != null)
        {
            currentPlot.TryPlant(GameManager.Instance.SelectedPlantType);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Plot plot = other.GetComponent<Plot>();
        if (plot != null)
        {
            currentPlot = plot;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Plot plot = other.GetComponent<Plot>();
        if (plot != null && plot == currentPlot)
        {
            currentPlot = null;
        }
    }
}
