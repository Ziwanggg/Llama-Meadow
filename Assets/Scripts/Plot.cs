using UnityEngine;

public class Plot : MonoBehaviour
{
    public bool hasPlant = false;

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
        
    }
}
