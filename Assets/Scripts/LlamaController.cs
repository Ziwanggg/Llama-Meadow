using UnityEngine;

public class LlamaController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Plot currentPlot;

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, y, 0f).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Plant seed on current plot
        if (Input.GetKeyDown(KeyCode.Space) && currentPlot != null)
        {
            currentPlot.TryPlant();
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
