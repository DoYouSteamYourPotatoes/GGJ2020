using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float width = 32.0f;
    public float height = 32.0f;

    public Color color = Color.white;

    public GameObject echelle;
    public GameObject epine;
    public GameObject hookGrab;
    public GameObject mecanisme;
    public GameObject murFragile;
    public GameObject obstacle;
    public GameObject plateformeFragile;
    public GameObject plateformeMouvante;
    public GameObject plateformeTraversable;
    public GameObject trampoline;
    public GameObject[] layout;


    private void OnDrawGizmos()
    {
        if (width == 0 || height == 0) return;

        Vector3 cameraPos = Camera.current.transform.position;

        Gizmos.color = color;

        for (float y = cameraPos.y - 8000.0f; y < cameraPos.y + 800.0f; y += height)
        {
            Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / height) * height, 0.0f),
                            new Vector3(1000000.0f, Mathf.Floor(y / height) * height, 0.0f));
        }

        for(float x = cameraPos.x - 1200.0f; x < cameraPos.x + 1200.0f; x += width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -1000000.0f, 0.0f),
                            new Vector3(Mathf.Floor(x / width) * width, 1000000.0f, 0.0f));
        }
    }
}
