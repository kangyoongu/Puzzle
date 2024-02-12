using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class ButtonClickSensor : OnOffImage
{
    bool first = true;
    public Dialog[] wording;
    public GameObject quad;
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
    public GameObject doorOpener;

    public GameObject beforeProbe;
    public GameObject afterProbe;

    public Enemy enemy;
    private void OnEnable()
    {
        exit += Exit;
        enter += Enter;
    }
    private void Start()
    {
        enemy.move = false;
    }
    private void OnDisable()
    {
        exit -= Exit;
        enter -= Enter;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerIsIn == true)
            {
                OffImage();
                StartCoroutine(StartAnim());
                playerIsIn = false;
            }
        }
    }
    IEnumerator StartAnim()
    {
        PlayerController.Instance.Down();
        PlayerController.Instance.camTransform.DOLocalRotate(new Vector3(0, -PlayerController.Instance.transform.eulerAngles.y + 90, 0), 1f);
        button.DOLocalMoveY(1.62f, 0.5f);
        yield return new WaitForSeconds(1.2f);
        RenderSettings.fogColor = new Color32(51, 19, 19, 255);
        pillar.DOLocalMoveY(-28f, 2f);
        for(int i = 0; i < effects.Length; i++)
        {
            if (i == 8) yield return new WaitForSeconds(2);
            int z = i;
            effects[z].gameObject.SetActive(true);
            DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 3, 2)//슬슬 보이게
               .SetEase(Ease.Linear).OnComplete(() =>
               {
                   beforeObj[z].SetActive(false);//이동시작
                   DOTween.To(() => effects[z].GetFloat("Lerp"), x => effects[z].SetFloat("Lerp", x), 1, 6).SetEase(Ease.Linear).OnComplete(() =>
                   {
                       afterObj[z].SetActive(true);
                       DOTween.To(() => effects[z].GetFloat("AlphaLerp"), x => effects[z].SetFloat("AlphaLerp", x), 0, 3).OnComplete(() =>
                       {
                           effects[z].gameObject.SetActive(false);
                       });//슬슬 안보이게
                   });
               });
            GameManager.Instance.currentInfo.OnlyPlayerDie();
        }
        yield return new WaitForSeconds(3);
        for (int i = 0; i < meshs.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            meshs[i].material = afterMaterial;
        }
        beforeProbe.SetActive(false);
        afterProbe.SetActive(true);
        enemy.transform.DOLocalMoveY(-25.5f, 1f);
        yield return new WaitForSeconds(1);
        enemy.move = true;
        enemy.startPos = enemy.transform.position;
        GameManager.Instance.currentInfo.SetSpawnPoint(0, enemy.transform.position);
        enemy.rigid.isKinematic = false;
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider>().enabled = false;
        door.ColorFull();
        doorOpener.SetActive(true);
    }
    public void Enter()
    {
        if (first == true)
        {
            for (int i = 0; i < wording.Length; i++)
            {
                UIManager.Instance.AppendDialog(wording[i]);
                first = false;
            }
        }
        playerIsIn = true;
    }
    public void Exit()
    {
        playerIsIn = false;
    }

    public override void OffImage()
    {
        quad.SetActive(false);
    }
}
