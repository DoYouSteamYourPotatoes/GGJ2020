using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
    public void Pickup(Action callBack)
    {
        StartCoroutine(GoToPlayer(done =>
        {
            if(done)
            {
                callBack();
                Destroy(gameObject);
            }
        }));
    }

    private IEnumerator GoToPlayer(Action<bool> done)
    {
        float timer = 0.0f;
        Vector3 startPos = transform.position;
        GameObject player = FindObjectOfType<Player>().gameObject;
        Vector3 targetPos = Vector3.zero;
        while (timer < 1.0f)
        {
            timer += Time.deltaTime;
            float t = timer / 1.0f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            targetPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        done(true);
        yield return null;
    }
}
