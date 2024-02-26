using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
public class KillLover : MonoBehaviour
{
    [SerializeField] Dialog[] dialogs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Kill();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void Kill()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        for(int i = 0; i < dialogs.Length; i++)//대사 전달
        {
            UIManager.Instance.AppendDialog(dialogs[i]);
        }
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.lover.move = false;//못움직이게 하기
        PlayerController.Instance.canMove = false;
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0, PlayerController.Instance.GetComponent<Rigidbody>().velocity.y, 0);
        yield return new WaitForSeconds(1f);
        Vector3 target = PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 2.5f;//타겟위치
        GameManager.Instance.lover.transform.DOLookAt(target, 1f).SetEase(Ease.OutQuad);//가야할 곳 쳐다보기
        yield return new WaitForSeconds(1f);
        GameManager.Instance.lover.transform.DOMove(target, 2f).SetEase(Ease.OutQuad);//이동
        yield return new WaitForSeconds(2f);
        GameManager.Instance.lover.transform.DOLookAt(PlayerController.Instance.transform.position, 1f).SetEase(Ease.OutQuad);//플레이어 쳐다보기
        yield return new WaitForSeconds(3f);
        PlayerController.Instance.PlayDie();//빵
        GameManager.Instance.lover.GetComponent<VisualEffect>().Stop();
        GameManager.Instance.lover.GetComponent<VisualEffect>().SetBool("isDie", true);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.lover.gameObject.SetActive(false);//4초있다 active false
        PlayerController.Instance.canMove = true;
        gameObject.SetActive(false);
    }
}
