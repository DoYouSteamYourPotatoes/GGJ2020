using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TogglePlatformGraphics
{
    [MenuItem("Custom/Toggle Physical Platforms ON _F5")]
    static void TogglePhysicalPlatformsON()
    {
        GameObject goPhysical = GameObject.FindGameObjectWithTag("PlatformsPhysical");
        RecursiveEnableMeshRenderer(goPhysical, true);
        Debug.Log("Toggle Physical Platforms ON");
    }

    [MenuItem("Custom/Toggle Physical Platforms OFF _F6")]
    static void TogglePhysicalPlatformsOFF()
    {
        GameObject goPhysical = GameObject.FindGameObjectWithTag("PlatformsPhysical");
        RecursiveEnableMeshRenderer(goPhysical, false);
        Debug.Log("Toggle Physical Platforms OFF");
    }

    [MenuItem("Custom/Toggle Graphics Platforms ON _F7")]
    static void ToggleGraphicsPlatformsON()
    {
        GameObject goGraphics = GameObject.FindGameObjectWithTag("PlatformsGraphics");
        RecursiveEnableGraphics(goGraphics, true);
        Debug.Log("Toggle Graphics Platforms ON");
    }

    [MenuItem("Custom/Toggle Graphics Platforms OFF _F8")]
    static void ToggleGraphicsPlatformsOFF()
    {
        GameObject goGraphics = GameObject.FindGameObjectWithTag("PlatformsGraphics");
        RecursiveEnableGraphics(goGraphics, false);
        Debug.Log("Toggle Graphics Platforms OFF");
    }

    private static void RecursiveEnableMeshRenderer(GameObject go, bool enable)
    {
        MeshRenderer refMesh = go.GetComponent<MeshRenderer>();
        if (refMesh != null)
        {
            refMesh.enabled = enable;
        }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            RecursiveEnableMeshRenderer(go.transform.GetChild(i).gameObject, enable);
        }
    }

    private static void RecursiveEnableGraphics(GameObject go, bool enable)
    {
        TilemapRenderer spriteRendererRef = go.GetComponent<TilemapRenderer>();
        if (spriteRendererRef != null)
        {
            spriteRendererRef.enabled = enable;
        }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            RecursiveEnableGraphics(go.transform.GetChild(i).gameObject, enable);
        }
    }
}
