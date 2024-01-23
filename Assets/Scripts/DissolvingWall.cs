using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvingWall : MonoBehaviour
{
    private BoxCollider boxCollider;
    private MeshRenderer mesh;
    Material mat;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        mat = mesh.material;
        mat = Instantiate(mat);
        mesh.material = mat;
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.BeforeLaserWork, Enable);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.BeforeLaserWork, Enable);
    }
    private void Enable()
    {
        boxCollider.enabled = true;
    }
    public void Disable()
    {
        boxCollider.enabled = false;
    }
    private void LateUpdate()
    {
        if (boxCollider.enabled)
        {
            mat.SetFloat("_Lerp", Mathf.Clamp01(mat.GetFloat("_Lerp") - 2f * Time.deltaTime));
        }
        else
        {
            mat.SetFloat("_Lerp", Mathf.Clamp01(mat.GetFloat("_Lerp") + 2f * Time.deltaTime));
        }
    }
}
