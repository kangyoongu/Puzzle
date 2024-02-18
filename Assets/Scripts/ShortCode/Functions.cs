using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    public void PlayerDie()
    {
        EventBus.Publish(State.PlayerDie);
    }
}
