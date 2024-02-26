using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.VFX;

public class FiveCutscene : MonoBehaviour
{
    public Dialog[] wording;
    public float delay = 10;
    public Transform door;
    public Enemy enemy;
    bool checkDis = false;
    public Volume bloom;
    public Transform pass;
    public CinemachineVirtualCamera target;
    public Material arrowMat;
    public GameObject fake;
    public GameObject real;
    public GameObject firstTrigger;
    public VisualEffect[] arrows;
    public GameObject god;

    public Rigidbody endCheck;

    private AudioSource aud;
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, ResetScene);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, ResetScene);
    }
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Animation2"))
        {
            arrowMat.color = new Color(1, 0, 0, 1);
            fake.SetActive(true);
            real.SetActive(false);
        }
        else
        {
            OffArrow();
            fake.SetActive(false);
            real.SetActive(true);
        }
        aud = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (checkDis)
        {
            if(Vector3.Distance(enemy.transform.position, PlayerController.Instance.transform.position) <= 5)
            {
                checkDis = false;
                StartCoroutine(GoGod());
            }
        }
        if(endCheck.isKinematic == true && GameManager.Instance.clear)
        {
            firstTrigger.SetActive(false);
        }
    }
    public void Block()
    {
        StartCoroutine(Confine());
    }
    IEnumerator Confine()//가둠
    {
        enemy.move = false;
        enemy.transform.DOLocalMoveY(7.25f, 1f);
        door.DOLocalMoveY(10f, 0.6f);
        aud.Play();
        yield return new WaitForSeconds(1);
        enemy.transform.localPosition = new Vector3(126.25f, enemy.transform.localPosition.y, 30f);
        enemy.transform.DOLocalMoveY(12.5f, 1f);
        yield return new WaitForSeconds(1);
        enemy.move = true;

        if (!PlayerPrefs.HasKey("Animation2"))
        {
            checkDis = true;
        }
    }
    IEnumerator GoGod()//신 보러감
    {
        god.SetActive(true);
        CinemachineBrain brain = GameManager.Instance.cBrain;
        DOTween.To(() => bloom.weight, x => bloom.weight = x, 1f, 1f).SetEase(Ease.InExpo);
        CutsceneAudio.Instance.Play(CutsceneAudio.Instance.stage5_fade);
        enemy.move = false;
        yield return new WaitForSeconds(3);
        PlayerController.Instance.transform.position = pass.transform.position;
        GravityControl.Instance.canGravityControl = false;
        EventBus.Publish(State.Normal);
        DOTween.To(() => bloom.weight, x => bloom.weight = x, 0f, 1f).SetEase(Ease.OutExpo);
        float fog = RenderSettings.fogEndDistance;
        float start = RenderSettings.fogStartDistance;
        RenderSettings.fogEndDistance = 25;
        RenderSettings.fogStartDistance = 0;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < wording.Length; i++)
        {
            UIManager.Instance.AppendDialog(wording[i]);
        }
        brain.m_DefaultBlend.m_Time = 5;
        yield return new WaitForSeconds(delay);
        target.Priority = 15;
        GameManager.Instance.canControl = false;
        yield return new WaitForSeconds(5);
        RenderSettings.fogStartDistance = 6;
        GameManager.Instance.audioSource.volume = 0f;
        EventBus.Publish(State.PlayerDie);
        yield return new WaitForSeconds(8);
        DOTween.To(() => bloom.weight, x => bloom.weight = x, 1f, 1f).SetEase(Ease.InExpo);
        CutsceneAudio.Instance.Play(CutsceneAudio.Instance.stage5_fade);
        brain.m_DefaultBlend.m_Time = 1;
        enemy.move = false;
        yield return new WaitForSeconds(2);
        enemy.move = false;
        RenderSettings.fogEndDistance = fog;
        RenderSettings.fogStartDistance = start;
        target.Priority = 5;
        DOTween.To(() => bloom.weight, x => bloom.weight = x, 0f, 2f).SetEase(Ease.OutExpo);
        GravityControl.Instance.canGravityControl = true;
        PlayerPrefs.SetInt("Animation2", 1);
        fake.SetActive(false);
        real.SetActive(true);
        yield return new WaitForSeconds(2);
        enemy.move = false;
        GameManager.Instance.audioSource.volume = 1f;
        god.SetActive(false);
        GameManager.Instance.canControl = true;
        yield return new WaitForSeconds(1);
        enemy.move = true;
    }
    public void ResetScene()
    {
        door.localPosition = new Vector3(door.localPosition.x, 16, door.localPosition.z);

        bloom.weight = 0;
        firstTrigger.SetActive(true);
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetFloat("Lerp", 0);
        }
        OffArrow();
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        enemy.move = true;
    }

    private void OffArrow()
    {
        arrowMat.color = new Color(1, 0, 0, 0);
    }
}
