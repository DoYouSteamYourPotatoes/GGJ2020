using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private BoxCollider boxCol = null;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        DashModule.Dashing += HandleDashing;
    }

    private void HandleDashing(bool dashing)
    {
        boxCol.isTrigger = dashing;
    }

    private void OnDestroy()
    {
        DashModule.Dashing -= HandleDashing;
    }
}
