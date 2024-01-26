using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
public class UIManager : SingleTon<UIManager>
{
    public UI[] gameOverUI;
    public UI[] mainUI;
    public UI[] playUI;
    public UI[] settingUI;
    public UI[] pauseUI;
    public GameObject[] block;

    private Queue<string> dialog = new();
    public TextMeshProUGUI dialogText;
    public RectTransform dialogWindow;
    bool playingText = false;
    bool setting = false;
    public GameObject pause;
    private void Update()
    {
        if(dialog.Count > 0 && !playingText)
        {
            StartCoroutine(DisplayText());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
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

    IEnumerator DisplayText()
    {
        playingText = true;
        dialogText.text = "";
        dialogWindow.DOAnchorPosY(228f, 1.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1.5f);
        while (dialog.Count > 0)
        {
            dialogText.text = "";
            string text = dialog.Dequeue();
            for (int i = 0; i < text.Length; i++)
            {
                dialogText.text += text[i];
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(Mathf.Min(1f, text.Length * 0.2f));
        }
        dialogWindow.DOAnchorPosY(-320f, 1.5f).SetEase(Ease.InBack);
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.canControl = false;
        In(pauseUI);
    }
    public void PauseUIOut()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.canControl = true;
        Out(pauseUI);
    }
    private void In(UI[] lst)
    {
        block[0].SetActive(true);
        block[1].SetActive(true);
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
        block[1].SetActive(true);
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
        block[1].SetActive(false);
    }
    public void AppendDialog(string text)
    {
        dialog.Enqueue(text);
    }
    public void OnClickStart()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.canControl = true;
        GameManager.Instance.playing = true;
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
}
