using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPlayer : MonoBehaviour
{
    int count = 0;
    public Dialog[] repeatDialog;
    public SixEvent sixEvent;
    public int Count { 
        get => count;
        set {
            count = value;
            if(count == 5 && !PlayerPrefs.HasKey("SixEvent"))
            {
                for (int i = 0; i < repeatDialog.Length; i++)
                {
                    UIManager.Instance.AppendDialog(repeatDialog[i]);
                }
                if (sixEvent.isFind == false)
                {
                    UIManager.Instance.AppendDialog(new Dialog { speaker = "찐신", line = "내가 진짜 큐브의 위치를 알려줄게"});
                    sixEvent.GuideParticle();
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float offset = transform.root.position.z;
            if(other.transform.position.z > offset + 74.5f)
            {
                other.transform.position -= new Vector3(0, 0, 25f);
                Count++;
            }
            else if (other.transform.position.z < offset + 49.5f)
            {
                other.transform.position += new Vector3(0, 0, 25f);
                Count++;
            }
        }
    }
}
