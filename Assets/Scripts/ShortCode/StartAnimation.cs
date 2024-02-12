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
    public Transform view;
    public GameObject resetter;
    public Rigidbody[] particle;
    public void SetStart()
    {
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
            downFloor[4].SetActive(true);
            downFloor[5].SetActive(true);
            downFloor[6].SetActive(true);
            downFloor[7].SetActive(true);
            enemy.transform.localPosition = new Vector3(84.50336f, 5.511235f, 0f);
        }
    }
    IEnumerator StartAnim()
    {
        GameManager.Instance.canControl = false;
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Quaternion q = PlayerController.Instance.camTransform.rotation;
        PlayerController.Instance.camTransform.DOLookAt(view.position, 1);
        enemy.move = false;
        yield return new WaitForSeconds(1.5f);
        floor.SetActive(false);
        rigidFloor[0].SetActive(true);
        rigidFloor[1].SetActive(true);
        rigidFloor[2].SetActive(true);
        rigidFloor[3].SetActive(true);
        rigidFloor[4].SetActive(true);
        rigidFloor[5].SetActive(true);
        rigidFloor[6].SetActive(true);
        rigidFloor[7].SetActive(true);
        for(int i = 0; i < particle.Length; i++)
        {
            particle[i].AddForce((particle[i].transform.position - enemy.transform.position) * 10, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(1f);
        CameraShake.Instance.Shake(10, 1);
        enemy.transform.DOLocalMove(new Vector3(84.50336f, 5.511235f, 0f), 3).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(3);
        PlayerController.Instance.camTransform.DORotateQuaternion(q, 1);
        yield return new WaitForSeconds(1);
        enemy.move = true;
        resetter.SetActive(true);
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
