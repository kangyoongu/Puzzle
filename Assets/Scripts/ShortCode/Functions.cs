using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public Dialog[] dialogs;
    public void PlayerDie()
    {
        PlayerController.Instance.PlayDie();
        EventBus.Publish(State.PlayerDie);
    }
    public void Dialog()
    {
        for(int i = 0; i < dialogs.Length; i++)
        {
            UIManager.Instance.AppendDialog(dialogs[i]);
        }
    }
}
