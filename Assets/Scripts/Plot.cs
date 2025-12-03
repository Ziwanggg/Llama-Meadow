using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool hasPlant = false;

    public Sprite dirtSprite;
    public Sprite plantedSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // start as dirt
        if (spriteRenderer != null && dirtSprite != null)
        {
            spriteRenderer.sprite = dirtSprite;
        }
    }

    public void TryPlant()
    {
        if (hasPlant || GameManager.Instance.IsGameOver)
        {
            return;
        }

        GameManager.Instance.SpawnPlantAt(this);
        hasPlant = true;
    }

    public void PlantFinished()
    {
        if (spriteRenderer != null && plantedSprite != null)
        {
            spriteRenderer.sprite = plantedSprite;
        }
    }
}
