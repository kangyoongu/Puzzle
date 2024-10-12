using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Dir : short
{
    x,
    y
}

[Serializable]
public struct UI
{
    public RectTransform changeUI;
    public Image fadeUI;
    public TextMeshProUGUI fadeText;
    public Dir dir;
    public Vector2 inAndOut;
    public float time;
    public bool setActive;
    public float fadeFloat;
}

[Serializable]
public struct Dialog
{
    public string speaker;
    [TextArea]
    public string line;
}

[Serializable]
public struct ImagePack
{
    public Image[] images;
    public TextMeshProUGUI[] texts;
    public GameObject root;
}
public class UIManager : SingleTon<UIManager>
{
    public UI[] gameOverUI;
    public UI[] mainUI;
    public UI[] playUI;
    public UI[] settingUI;
    public UI[] pauseUI;
    public GameObject[] block;

    private Queue<Dialog> dialog = new();
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerText;
    public RectTransform dialogWindow;
    bool playingText = false;
    bool setting = false;
    //bool imageOn = false;
    public GameObject pause;
    //Coroutine co;
    [Header("중심점")]
    public Image bigCircle;
    public Image donut;
    public Image point;
    [SerializeField] Color upColor;
    [SerializeField] Color downColor;
    private AudioSource[] sources;

    bool currentControl;
    Tweener tween;
    Tweener tween2;

    [SerializeField] private ImagePack[] images;
    private void Start()
    {
        UpPoint();
    }
    private void Update()
    {
        if(dialog.Count > 0 && !playingText)
        {
            StartCoroutine(DisplayText());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CheckManager.Instance.check.activeSelf == true)
            {
                CheckManager.Instance.OnClickNo();
            }
            else if (block[0].activeSelf == false)
            {
                if (setting)
                {
                    SettingUIOut();
                }
                else
                {
                    if (GameManager.Instance.playing && Time.timeScale != 0)
                    {
                        PauseUIIn();
                    }
                    else if (Time.timeScale == 0)
                    {
                        PauseUIOut();
                    }
                }
            }
        }
    }

    IEnumerator DisplayText()
    {
        playingText = true;
        while (dialog.Count > 0)
        {
            Dialog text = dialog.Dequeue();
            dialogText.text = "";
            speakerText.text = text.speaker;
            if (dialogWindow.anchoredPosition.y != 228f)
            {
                dialogWindow.DOAnchorPosY(110f, 0.8f).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(0.8f);
            }
            if (text.line != "")
            {
                for (int i = 0; i < text.line.Length; i++)
                {
                    dialogText.text += text.line[i];
                    if(text.line[i] == '<' && text.line[i+1] == 'c')
                    {
                        for (int j = 0; j < 14; j++)
                        {
                            i++;
                            dialogText.text += text.line[i];
                        }
                    }
                    else if (text.line[i] == '<' && text.line[i + 1] == '/')
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            i++;
                            dialogText.text += text.line[i];
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(Mathf.Min(1f, text.line.Length * 0.2f));
            }
        }
        dialogWindow.DOAnchorPosY(-191.2f, 0.8f).SetEase(Ease.InBack);
        playingText = false;
    }
    public void GameOverUIIn()
    {
        In(gameOverUI);
    }
    public void GameOverUIOut()
    {
        Out(gameOverUI);
    }
    public void MainUIIn()
    {
        In(mainUI);
    }

    public void MainUIOut()
    {
        Out(mainUI);
    }
    public void PlayUIIn()
    {
        In(playUI);
    }
    public void PlayUIOut()
    {
        Out(playUI);
    }
    public void SettingUIIn()
    {
        In(settingUI);
        setting = true;
    }
    public void SettingUIOut()
    {
        Out(settingUI);
        setting = false;
    }
    public void PauseUIIn()
    {
        Time.timeScale = 0;
        sources = FindObjectsOfType<AudioSource>();
        foreach(AudioSource a in sources)
        {
            if (a.isPlaying) a.Pause();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        currentControl = GameManager.Instance.canControl;
        GameManager.Instance.canControl = false;
        In(pauseUI);
    }
    public void PauseUIOut()
    {
        Time.timeScale = 1;
        foreach (AudioSource a in sources)
        {
            a.UnPause();
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.canControl = currentControl;
        Out(pauseUI);
    }
    private void In(UI[] lst)
    {
        block[0].SetActive(true);
        float max = 0;
        for (int i = 0; i < lst.Length; i++)
        {
            if (max < lst[i].time) max = lst[i].time;
            if (lst[i].changeUI != null)
            {
                if (lst[i].setActive) lst[i].changeUI.gameObject.SetActive(true);
                if (lst[i].dir == Dir.y) lst[i].changeUI.DOAnchorPosY(lst[i].inAndOut.x, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
                else lst[i].changeUI.DOAnchorPosX(lst[i].inAndOut.x, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
            else if (lst[i].fadeUI != null)
            {
                if (lst[i].setActive) lst[i].fadeUI.gameObject.SetActive(true);
                lst[i].fadeUI.DOFade(lst[i].fadeFloat / 255f, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
            else
            {
                if (lst[i].setActive) lst[i].changeUI.gameObject.SetActive(true);
                lst[i].fadeText.DOFade(lst[i].fadeFloat / 255f, lst[i].time).SetEase(Ease.Linear).SetUpdate(true);
            }
        }
        StartCoroutine(BlockTime(max));
    }

    private void Out(UI[] lst)
    {
        block[0].SetActive(true);
        float max = 0;
        for (int i = 0; i < lst.Length; i++)
        {
            if (max < lst[i].time) max = lst[i].time;
            int index = i;
            if (lst[i].changeUI != null)
            {
                if (lst[i].dir == Dir.y)
                {
                    lst[i].changeUI.DOAnchorPosY(lst[i].inAndOut.y, lst[i].time).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                    });
                }
                else
                {
                    lst[i].changeUI.DOAnchorPosX(lst[i].inAndOut.y, lst[i].time).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                    });
                }
            }
            else if (lst[i].fadeUI != null)
            {
                lst[i].fadeUI.DOFade(0, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (lst[index].setActive) lst[index].fadeUI.gameObject.SetActive(false);
                });
            }
            else
            {
                lst[i].fadeText.DOFade(0, lst[i].time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    if (lst[index].setActive) lst[index].changeUI.gameObject.SetActive(false);
                });
            }
        }
        StartCoroutine(BlockTime(max));
    }
    IEnumerator BlockTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        block[0].SetActive(false);
    }
    public void AppendDialog(Dialog dialog)
    {
        this.dialog.Enqueue(dialog);
    }
    public int DialogCount()
    {
        return dialog.Count;
    }
    /*public void ShowImage(Sprite sprite, float time) 단일이미지 코드
    {
        if (imageOn)
        {
            StopCoroutine(co);
            co = StartCoroutine(Swap(time, sprite));
        }
        else
        {
            image.sprite = sprite;
            co = StartCoroutine(Show(time));
        }
    }
    IEnumerator Swap(float time, Sprite sprite)
    {

        image.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        image.sprite = sprite;
        image.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        imageOn = true;
        yield return new WaitForSeconds(time);
        image.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        imageOn = false;
    }
    IEnumerator Show(float time)
    {
        image.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        imageOn = true;
        yield return new WaitForSeconds(time);
        image.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        imageOn = false;
    }*/
    public void ShowImage(int index, float time)
    {
        StartCoroutine(Show(time, index));
    }
    IEnumerator Show(float time, int index)
    {
        images[index].root.SetActive(true);
        for (int i = 0; i < images[index].images.Length; i++)
        {
            images[index].images[i].DOFade(1f, 1f);
        }
        for (int i = 0; i < images[index].texts.Length; i++)
        {
            images[index].texts[i].DOFade(1f, 1f);
        }
        yield return new WaitForSeconds(time + 1);
        images[index].root.SetActive(true);
        for (int i = 0; i < images[index].images.Length; i++)
        {
            images[index].images[i].DOFade(0f, 1f);
        }
        for (int i = 0; i < images[index].texts.Length; i++)
        {
            images[index].texts[i].DOFade(0f, 1f);
        }
        yield return new WaitForSeconds(1);
        images[index].root.SetActive(false);
    }
    public void UpPoint()
    {
        if (point.color != new Color(upColor.r, upColor.g, upColor.b, point.color.a))
        {
            point.DOColor(new Color(upColor.r, upColor.g, upColor.b, point.color.a), 0.5f);
            donut.DOColor(new Color(upColor.r, upColor.g, upColor.b, donut.color.a), 0.5f);
            bigCircle.DOColor(new Color(upColor.r, upColor.g, upColor.b, bigCircle.color.a), 0.5f);
        }
    }
    public void DownPoint()
    {
        if (point.color != new Color(downColor.r, downColor.g, downColor.b, point.color.a))
        {
            point.DOColor(new Color(downColor.r, downColor.g, downColor.b, point.color.a), 0.5f);
            donut.DOColor(new Color(downColor.r, downColor.g, downColor.b, donut.color.a), 0.5f);
            bigCircle.DOColor(new Color(downColor.r, downColor.g, downColor.b, bigCircle.color.a), 0.5f);
        }
    }
    public void Normal()
    {
        if (bigCircle.rectTransform.localScale.x != 0f || donut.rectTransform.localScale.x != 0f)
        {
            if (tween != null && tween.IsPlaying())
            {
                tween.Kill();
                tween2.Kill();
            }
            tween = donut.rectTransform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
            tween2 = bigCircle.rectTransform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
        }
    }
    public void OnGrabable()
    {
        if (bigCircle.rectTransform.localScale.x != 1f || donut.rectTransform.localScale.x != 0f)
        {

            if (tween != null && tween.IsPlaying())
            {
                tween.Kill();
                tween2.Kill();
            }
            tween = donut.rectTransform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
            tween2 = bigCircle.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }
    public void Grab()
    {
        if (bigCircle.rectTransform.localScale.x != 0f || donut.rectTransform.localScale.x != 1f)
        {
            if(tween != null && tween.IsPlaying())
            {
                tween.Kill();
                tween2.Kill();
            }
            tween = donut.rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
            tween2 = bigCircle.rectTransform.DOScale(0f, 0.5f).SetEase(Ease.OutBack);
        }
    }
    
    public void OnClickStart()
    {
        GameManager.Instance.GameStart();
    }
    public void OnClickRestart()
    {
        CheckManager.Instance.Check("정말 처음부터\n다시하시겠습니까?", Restart);
    }
    private void Restart()
    {
        JsonManager.Instance.Stage = 1;
        PlayerPrefs.DeleteAll();
        GameManager.Instance.GameStart();
        PlayUIIn();
        MainUIOut();
    }
    public void OnClickQuit()
    {
        CheckManager.Instance.Check("정말 게임을\n종료하시겠습니까?", Quit);
    }
    private void Quit()
    {
        Application.Quit();
    }
    public void OnClickGoMain()
    {
        CheckManager.Instance.Check("나가면 진행중이던 맵이 초기화됩니다.\n정말 나가시겠습니까?", GameManager.Instance.GoMain);
    }
}
