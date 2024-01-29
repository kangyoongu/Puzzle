using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSensor : MonoBehaviour
{
    public List<string> messages;
    public Sprite image;
    public bool upImage;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for(int i = 0; i < messages.Count; i++)
            {
                UIManager.Instance.AppendDialog(messages[i]);
            }
            if (upImage)
            {
                UIManager.Instance.AppendImage(image);
            }
            else
            {
                UIManager.Instance.OutImage();
            }
        }
    }
}
