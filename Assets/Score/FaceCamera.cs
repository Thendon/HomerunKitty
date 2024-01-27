using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{
    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnRendered;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnRendered;
    }

    private void OnRendered(ScriptableRenderContext context, Camera cam)
    {
        if (cam == null)
            return;
        transform.LookAt(transform.position + cam.transform.forward, cam.transform.up);
    }
}