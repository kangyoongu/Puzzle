using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpenWindow : MonoBehaviour
{
    public void OnClickMenu(Transform window)
    {
        window.SetAsLastSibling();
    }
}
