using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour, IDoor
{
    Animator[] anim;
    public int crystalNum = 1;
    private int currentCrystal = 0;
    public AudioClip openClip;
    public AudioClip closeClip;
    private AudioSource audioSource;
    private AudioSource crystalClip;
    public int CurrentCrystal
    {
        get => currentCrystal;
        set
        {
            currentCrystal = value;
            centerMaterial.SetFloat("_Lerp", 1f - ((float)currentCrystal / crystalNum));
        }
    }
    MeshRenderer center;
    Material centerMaterial;
    GameObject block;
    bool open = false;
    void Start()
    {
        anim = GetComponentsInChildren<Animator>();
        center = transform.GetChild(5).GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        crystalClip = transform.GetChild(7).GetComponent<AudioSource>();
        block = transform.GetChild(6).gameObject;
        centerMaterial = center.material;
        centerMaterial = Instantiate(centerMaterial);
        center.material = centerMaterial;
        centerMaterial.SetFloat("_Lerp", 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            if (CurrentCrystal >= crystalNum && !open)
            {
                open = true;
                OpenDoor();
            }
        }
    }
    public void ColorFull()
    {
        centerMaterial.SetFloat("_Lerp", 0);
    }
    public void OpenDoor()
    {
        ColorFull();
        audioSource.clip = openClip;
        audioSource.Play();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Open");
        }
        SceneManager.LoadScene(GameManager.Instance.currentInfo.nextScene, LoadSceneMode.Additive);
        StartCoroutine(MoveY(-2.4f, 1.5f, false));
    }
    public void CloseDoor()
    {
        audioSource.clip = closeClip;
        audioSource.Play();
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play("Close");
        }
        block.SetActive(true);
        StartCoroutine(MoveY(0, 1.5f, true));
    }
    private IEnumerator MoveY(float targetY, float duration, bool active)
    {
        Vector3 startPosition = center.transform.localPosition;
        Vector3 targetPosition = new Vector3(center.transform.localPosition.x, targetY, center.transform.localPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            center.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막에 정확한 위치로 보정
        center.transform.localPosition = targetPosition;
        if (active == false)
        {
            block.SetActive(false);
        }
    }
    public void PlayAud(AudioClip clip)
    {
        crystalClip.clip = clip;
        crystalClip.Play();
    }
}
