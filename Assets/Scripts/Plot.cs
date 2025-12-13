using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool hasPlant = false;

    public Sprite dirtSprite;
    public Sprite grassSprite;
    public Sprite flowerSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && dirtSprite != null)
        {
            spriteRenderer.sprite = dirtSprite;
        }
    }

    public void TryPlant(PlantType type)
    {
        if (hasPlant || GameManager.Instance.IsGameOver || !GameManager.Instance.GameStarted || GameManager.Instance.IsPaused)
            return;

        // Must have seeds first
        if (type == PlantType.Grass)
        {
            if (GameManager.Instance.grassSeeds <= 0)
            {
                GameManager.Instance.ShowMessage("No grass seeds. Press B to buy.");
                return;
            }
            GameManager.Instance.grassSeeds--;
        }
        else // Flower
        {
            if (GameManager.Instance.flowerSeeds <= 0)
            {
                GameManager.Instance.ShowMessage("No flower seeds. Press N to buy.");
                return;
            }
            GameManager.Instance.flowerSeeds--;
        }

        GameManager.Instance.UpdateMoneyUI();
        GameManager.Instance.SpawnPlantAt(this, type);
        hasPlant = true;
    }


    public void PlantFinished(PlantType type)
    {
        if (spriteRenderer == null) return;

        if (type == PlantType.Grass && grassSprite != null)
        {
            spriteRenderer.sprite = grassSprite;
        }
        else if (type == PlantType.Flower && flowerSprite != null)
        {
            spriteRenderer.sprite = flowerSprite;
        }
    }
}
