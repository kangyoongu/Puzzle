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
    private void OnEnable()
    {
        LaserManager.Instance.walls.Add(this);
    }
    private void OnDisable()
    {
        LaserManager.Instance.walls.RemoveAt(0);
    }
    private void Start()
    {
        mat = mesh.material;
        mat = Instantiate(mat);
        mesh.material = mat;
    }
    public void ColliderOn()
    {
        if (boxCollider.enabled == false)
        {
            boxCollider.enabled = true;
        }
    }
    public void ColliderOff()
    {
        if(boxCollider.enabled == true)
        {
            boxCollider.enabled = false; 
        }
    }
    private void LateUpdate()
    {
        if (boxCollider.enabled)
        {
            mat.SetFloat("_Lerp", Mathf.Clamp01(mat.GetFloat("_Lerp") - 2f * Time.deltaTime));
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            mat.SetFloat("_Lerp", Mathf.Clamp01(mat.GetFloat("_Lerp") + 2f * Time.deltaTime));
            gameObject.layer = LayerMask.NameToLayer("NotGrabLayer");
        }
    }
}
