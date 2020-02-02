using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    [SerializeField] private BoxCollider targetArea = null;
    [SerializeField] private GameObject player = null;

    [Header("settings")]
    [SerializeField] private float triggerToFast = 10.0f;
    [SerializeField] private float triggerToSlow = 2.0f;
    [SerializeField] private float triggerPlayerDistance = 15.0f;
    [SerializeField] private float timeToTarget_Default = 2.0f;
    [SerializeField] private float timeToTarget_Fast = 0.75f;
    [SerializeField] private float timeToTarget_Slow = 5.0f;

    private Vector3 oldPlayerPos = Vector3.zero;
    private float oldDistanceToPlayer = 0.0f;
    private float distanceToPlayer = 0.0f;
    private Coroutine behaviourCoroutine = null;

    private void Start()
    {
        oldPlayerPos = player.transform.position;
        //behaviourCoroutine = StartCoroutine(StandardBehaviourLoop());
    }

    private void Update()
    {
        
    }


    /*
    public IEnumerator StandardBehaviourLoop()
    {
        while (true)
        {
            float timer = 0.0f;
            bool recalculate = false;
            Vector3 targetPoint = RandomPointInBounds(targetArea.bounds);
            targetPoint.y = targetArea.transform.position.y;
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            oldDistanceToPlayer = distanceToPlayer;
            float transitionDuration = timeToTarget_Default;
            if (distanceToPlayer < triggerToSlow)
            {
                transitionDuration = timeToTarget_Slow;
            }
            else if (distanceToPlayer > triggerToFast)
            {
                transitionDuration = timeToTarget_Fast;
            }

            while (timer < transitionDuration && !recalculate)
            {
                Vector3 startPos = transform.position;
                timer += Time.deltaTime;
                float t = timer / transitionDuration;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                transform.position = Vector3.Lerp(startPos, targetPoint, t);
                if (Mathf.Abs(distanceToPlayer - oldDistanceToPlayer) > triggerPlayerDistance)
                {
                    oldDistanceToPlayer = 1.0f;
                    recalculate = true;
                }
                yield return null;
            }
            yield return null;
        }
    }
    
    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    */
}
