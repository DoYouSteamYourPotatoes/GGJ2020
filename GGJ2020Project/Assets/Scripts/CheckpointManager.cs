using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint currentActiveCheckpoint;

    public Checkpoint CurrentActiveCheckpoint
    {
        get => currentActiveCheckpoint;
        set
        {
            if (currentActiveCheckpoint != null && value != currentActiveCheckpoint)
            {
                currentActiveCheckpoint.Active = false;
            }
            currentActiveCheckpoint = value;
        }
    }

    public void Respawn()
    {

    }
}
