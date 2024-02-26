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
        for(int i = 0; i < dialogs.Length; i++)//��� ����
        {
            UIManager.Instance.AppendDialog(dialogs[i]);
        }
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.lover.move = false;//�������̰� �ϱ�
        PlayerController.Instance.canMove = false;
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0, PlayerController.Instance.GetComponent<Rigidbody>().velocity.y, 0);
        yield return new WaitForSeconds(1f);
        Vector3 target = PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 2.5f;//Ÿ����ġ
        GameManager.Instance.lover.transform.DOLookAt(target, 1f).SetEase(Ease.OutQuad);//������ �� �Ĵٺ���
        yield return new WaitForSeconds(1f);
        GameManager.Instance.lover.transform.DOMove(target, 2f).SetEase(Ease.OutQuad);//�̵�
        yield return new WaitForSeconds(2f);
        GameManager.Instance.lover.transform.DOLookAt(PlayerController.Instance.transform.position, 1f).SetEase(Ease.OutQuad);//�÷��̾� �Ĵٺ���
        yield return new WaitForSeconds(3f);
        PlayerController.Instance.PlayDie();//��
        GameManager.Instance.lover.GetComponent<VisualEffect>().Stop();
        GameManager.Instance.lover.GetComponent<VisualEffect>().SetBool("isDie", true);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.lover.gameObject.SetActive(false);//4���ִ� active false
        PlayerController.Instance.canMove = true;
        gameObject.SetActive(false);
    }
}
