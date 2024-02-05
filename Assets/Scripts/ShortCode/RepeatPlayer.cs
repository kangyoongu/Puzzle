using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPlayer : MonoBehaviour
{
    int count = 0;
    public int Count { 
        get => count;
        set {
            count = value;
            if(count == 2)
            {
                UIManager.Instance.AppendDialog(new Dialog() { dialog = "ㅋㅋㅋ뭔가 이상하지 않니" });
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
