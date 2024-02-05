using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
public class StartAnimation : MonoBehaviour
{
    public GameObject floor;
    public GameObject[] rigidFloor;
    public GameObject[] downFloor;
    public Enemy enemy;
    public CinemachineVirtualCamera view;
    public GameObject resetter;
    public void SetStart()
    {
        PlayerPrefs.DeleteKey("Animation1");
        if (!PlayerPrefs.HasKey("Animation1"))
        {
            PlayerPrefs.SetInt("Animation1", -1);
            resetter.SetActive(false);
            enemy.move = false;
        }
        if (PlayerPrefs.GetInt("Animation1") == 1)
        {
            enemy.move = true;
            floor.SetActive(false);
            downFloor[0].SetActive(true);
            downFloor[1].SetActive(true);
            downFloor[2].SetActive(true);
            downFloor[3].SetActive(true);
            enemy.transform.localPosition = new Vector3(84.50336f, 5.511235f, 0f);
        }
    }
    IEnumerator StartAnim()
    {
        GameManager.Instance.canControl = false;
        view.Priority = 15;
        enemy.move = false;
        floor.SetActive(false);
        rigidFloor[0].SetActive(true);
        rigidFloor[1].SetActive(true);
        rigidFloor[2].SetActive(true);
        rigidFloor[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        enemy.transform.DOLocalMove(new Vector3(84.50336f, 5.511235f, 0f), 3).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(4);
        view.Priority = 5;
        enemy.move = true;
        resetter.SetActive(true);
        resetter.GetComponent<MapReset>().KinematicFalse();
        GameManager.Instance.canControl = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("Animation1") == -1)
            {
                PlayerPrefs.SetInt("Animation1", 1);
                StartCoroutine(StartAnim());
            }
        }
    }
}
