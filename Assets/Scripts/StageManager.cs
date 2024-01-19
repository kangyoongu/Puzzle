using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public MapInfo[] mapInfo;
    [HideInInspector] public int mapIndex;
    void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (GameManager.Instance.camTrm.position.x >= mapInfo[mapIndex].maxPos.x + 60)
        {
            GameManager.Instance.player.position -= new Vector3(mapInfo[mapIndex].maxPos.x - mapInfo[mapIndex].minPos.x + 120, 0, 0);
        }
        else if (GameManager.Instance.camTrm.position.x <= mapInfo[mapIndex].minPos.x - 60)
        {
            GameManager.Instance.player.position += new Vector3(mapInfo[mapIndex].maxPos.x - mapInfo[mapIndex].minPos.x + 120, 0, 0);
        }
        if (GameManager.Instance.camTrm.position.y >= mapInfo[mapIndex].maxPos.y + 60)
        {
            GameManager.Instance.player.position -= new Vector3(0, mapInfo[mapIndex].maxPos.y - mapInfo[mapIndex].minPos.y + 120, 0);
        }
        else if (GameManager.Instance.camTrm.position.y <= mapInfo[mapIndex].minPos.y - 60)
        {
            GameManager.Instance.player.position += new Vector3(0, mapInfo[mapIndex].maxPos.y - mapInfo[mapIndex].minPos.y + 120, 0);
        }
        if (GameManager.Instance.camTrm.position.z >= mapInfo[mapIndex].maxPos.z + 60)
        {
            GameManager.Instance.player.position -= new Vector3(0, 0, mapInfo[mapIndex].maxPos.z - mapInfo[mapIndex].minPos.z + 120);
        }
        else if (GameManager.Instance.camTrm.position.z <= mapInfo[mapIndex].minPos.z - 60)
        {
            GameManager.Instance.player.position += new Vector3(0, 0, mapInfo[mapIndex].maxPos.z - mapInfo[mapIndex].minPos.z + 120);
        }
    }
}
