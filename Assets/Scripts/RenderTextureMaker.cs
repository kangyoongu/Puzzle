using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class RenderTextureMaker : MonoBehaviour
{
    public Material targetMaterial;
    Camera cam;
    RenderTexture renderTexture;
    void Start()
    {
        targetMaterial = Instantiate(targetMaterial);
        cam = GetComponentInChildren<Camera>();
        cam.orthographicSize = transform.root.localScale.y * 0.5f;
        renderTexture = new RenderTexture((int)(transform.root.localScale.x * 100), (int)(transform.root.localScale.y * 100), 0, GraphicsFormat.R16G16B16A16_SFloat);
        renderTexture.name = "CustomRenderTexture";
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.Create();
        cam.targetTexture = renderTexture;
        targetMaterial.SetTexture("_Texture2D", renderTexture);
        GetComponent<MeshRenderer>().material = targetMaterial;
    }

    void OnDestroy()
    {
        // ��ũ��Ʈ�� �ı��� �� ���� �ؽ��� �� ī�޶� �ı�
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}
