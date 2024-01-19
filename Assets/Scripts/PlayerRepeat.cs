using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepeat : MonoBehaviour
{
    public MapInfo[] mapInfo;
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (GameManager.Instance.camTrm.position.x >= mapInfo[GameManager.Instance.mapIndex].maxPos.x + 60)
        {
            GameManager.Instance.player.position -= new Vector3(mapInfo[GameManager.Instance.mapIndex].maxPos.x - mapInfo[GameManager.Instance.mapIndex].minPos.x + 120, 0, 0);
        }
        else if (GameManager.Instance.camTrm.position.x <= mapInfo[GameManager.Instance.mapIndex].minPos.x - 60)
        {
            GameManager.Instance.player.position += new Vector3(mapInfo[GameManager.Instance.mapIndex].maxPos.x - mapInfo[GameManager.Instance.mapIndex].minPos.x + 120, 0, 0);
        }
        if (GameManager.Instance.camTrm.position.y >= mapInfo[GameManager.Instance.mapIndex].maxPos.y + 60)
        {
            GameManager.Instance.player.position -= new Vector3(0, mapInfo[GameManager.Instance.mapIndex].maxPos.y - mapInfo[GameManager.Instance.mapIndex].minPos.y + 120, 0);
        }
        else if (GameManager.Instance.camTrm.position.y <= mapInfo[GameManager.Instance.mapIndex].minPos.y - 60)
        {
            GameManager.Instance.player.position += new Vector3(0, mapInfo[GameManager.Instance.mapIndex].maxPos.y - mapInfo[GameManager.Instance.mapIndex].minPos.y + 120, 0);
        }
        if (GameManager.Instance.camTrm.position.z >= mapInfo[GameManager.Instance.mapIndex].maxPos.z + 60)
        {
            GameManager.Instance.player.position -= new Vector3(0, 0, mapInfo[GameManager.Instance.mapIndex].maxPos.z - mapInfo[GameManager.Instance.mapIndex].minPos.z + 120);
        }
        else if (GameManager.Instance.camTrm.position.z <= mapInfo[GameManager.Instance.mapIndex].minPos.z - 60)
        {
            GameManager.Instance.player.position += new Vector3(0, 0, mapInfo[GameManager.Instance.mapIndex].maxPos.z - mapInfo[GameManager.Instance.mapIndex].minPos.z + 120);
        }
    }
}
