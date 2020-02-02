using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCompanion_visual : MonoBehaviour
{
    [SerializeField] private Transform followObj = null;
    float step = 0.0f; //A variable we increment
    [SerializeField] float offset = 0.0f; //How far to offset the object upwards
    [SerializeField] float amplitude = 0.0f;
    [SerializeField] float followSpeed = 0.0f;
    [SerializeField] float wanderRangeMin = 0.0f;
    [SerializeField] float wanderRangeMax = 0.0f;
    [SerializeField] float wanderDurationY = 0.0f;
    [SerializeField] float wanderDurationX = 0.0f;
    public Vector3 targetPos;

    private float yOffset = 0.0f;
    private float xOffset = 0.0f;

    private bool yRunning = false;
    private bool xRunning = false;

    private void Update()
    {
        step += 0.01f;
        //Make sure Steps value never gets too out of hand 
        if (step > 999999f)
        {
            step = 1;
        }

        //Float up and down along the y axis, 
        targetPos = Vector3.Lerp(transform.localPosition, new Vector3(followObj.transform.position.x + xOffset, followObj.transform.position.y + yOffset, transform.localPosition.z), Time.deltaTime * followSpeed);
        transform.localPosition = targetPos;

        if (!yRunning)
        {
            yRunning = true;
            StartCoroutine(CalculateYOffset());
        }
        if (!xRunning)
        {
            xRunning = true;
            StartCoroutine(CalculateXOffset());
        }
    }

    private IEnumerator CalculateYOffset()
    {
        while (yRunning)
        {
            float timer = 0.0f;
            float startValue = yOffset;
            float targetValue = Random.Range(wanderRangeMin, wanderRangeMax);
            Vector3 startPos = transform.position;
            while (timer < wanderDurationY)
            {
                timer += Time.deltaTime;
                float t = timer / wanderDurationY;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                yOffset = Mathf.Lerp(startValue, targetValue, t);
                yield return null;
            }
            yRunning = false;
        }
        yield return null;
    }

    private IEnumerator CalculateXOffset()
    {
        while (xRunning)
        {
            float timer = 0.0f;
            float startValue = xOffset;
            float targetValue = Random.Range(wanderRangeMin, wanderRangeMax);
            Vector3 startPos = transform.position;
            while (timer < wanderDurationX)
            {
                timer += Time.deltaTime;
                float t = timer / wanderDurationX;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                xOffset = Mathf.Lerp(startValue, targetValue, t);
                yield return null;
            }
            xRunning = false;
        }
        yield return null;
    }
}
