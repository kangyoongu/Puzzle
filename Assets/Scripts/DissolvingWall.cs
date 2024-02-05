using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DissolvingWall : MonoBehaviour
{
    private Collider boxCollider;
    private MeshRenderer mesh;
    Material mat;
    bool work = true;
    private void Awake()
    {
        boxCollider = GetComponent<Collider>();
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
        if (boxCollider.enabled == false && work)
        {
            boxCollider.enabled = true;
        }
    }
    public void ColliderOff()
    {
        if(boxCollider.enabled == true && work)
        {
            boxCollider.enabled = false; 
        }
    }
    private void LateUpdate()
    {
        if (work)
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
    public void Off()
    {
        if(boxCollider.enabled == true)
            boxCollider.enabled = false;
        work = false;
        mat.DOFloat(1, "_Lerp", 2);
    }
}
