using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraMove : MonoBehaviour
{
    public Material graphicMat;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, graphicMat);
    }
}
