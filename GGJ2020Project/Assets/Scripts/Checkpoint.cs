using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CheckpointManager checkpointManager = default;
    private bool active = false;
    private SpriteRenderer spriteRend;
    private Color defaultEmissive;
    private Color activeEmissive;
    private const float ActiveMutiplier = 10;
    private Transform respawnPoint;

    public Transform RespawnPoint
    {
        set => respawnPoint = value;
        get
        {
            if (respawnPoint == null)
            {
                respawnPoint = transform.GetChild(0).transform;
            }
            return respawnPoint;
        }
    }

    public bool Active
    {
        get => active;
        set
        {
            checkpointManager.CurrentActiveCheckpoint = this;
            SpriteRend.material.SetColor("_EmissionColor", value ? activeEmissive : defaultEmissive);
            active = value;
        }
    }

    private SpriteRenderer SpriteRend
    {
        set => spriteRend = value;
        get
        {
            if(spriteRend == null)
            {
                spriteRend = GetComponent<SpriteRenderer>();
            }
            return spriteRend;
        }
    }

    private void Awake()
    {
        defaultEmissive = SpriteRend.material.GetColor("_EmissionColor");
        activeEmissive = defaultEmissive * ActiveMutiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Active = true;
        }
    }
}
