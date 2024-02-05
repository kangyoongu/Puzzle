using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class ButtonClickSensor : MonoBehaviour
{
    bool first = true;
    public Dialog[] wording;
    bool playerIsIn = false;
    public Sprite Ekey;

    public GameObject[] afterObj;
    public GameObject[] beforeObj;
    public VisualEffect[] effects;

    public MeshRenderer[] meshs;
    public Material afterMaterial;
    public Transform button;
    public Transform pillar;

    public DoorOpen door;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerIsIn == true)
            {
                StartCoroutine(StartAnim());
                UIManager.Instance.OutImage();
                playerIsIn = false;
            }
        }
    }
    IEnumerator StartAnim()
    {
        PlayerController.Instance.Down();
        PlayerController.Instance.camTransform.DOLocalRotate(new Vector3(0, -PlayerController.Instance.transform.eulerAngles.y + 90, 0), 1f);
        button.DOLocalMoveY(1.62f, 0.5f);
        yield return new WaitForSeconds(1);
        pillar.DOLocalMoveY(-28f, 2f);
        for(int i = 0; i < effects.Length; i++)
        {
            yield return new WaitForSeconds(1);
            int z = i;
            effects[z].gameObject.SetActive(true);
            DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 3, 2)//슬슬 보이게
               .SetEase(Ease.Linear).OnComplete(() =>
               {
                   beforeObj[z].SetActive(false);//이동시작
                   DOTween.To(() => effects[z].GetFloat("Lerp"), x => effects[z].SetFloat("Lerp", x), 1, 7).SetEase(Ease.Linear).OnComplete(() =>
                   {
                       afterObj[z].SetActive(true);
                       DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 0, 3).OnComplete(() =>
                       {
                           effects[z].gameObject.SetActive(false);
                       });//슬슬 안보이게
                   });
               });
            GameManager.Instance.currentInfo.PlayerDie();
        }
        yield return new WaitForSeconds(3);
        for(int i = 0; i < meshs.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            meshs[i].material = afterMaterial;
        }
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider>().enabled = false;
        door.OpenDoor();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(first == true)
            {
                for (int i = 0; i < wording.Length; i++)
                {
                    UIManager.Instance.AppendDialog(wording[i]);
                    first = false;
                }
            }
            UIManager.Instance.AppendImage(Ekey);
            playerIsIn = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = false;
            UIManager.Instance.OutImage();
        }
    }
}
