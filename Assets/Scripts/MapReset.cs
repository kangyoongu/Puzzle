using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapReset : MonoBehaviour
{
    List<Vector3> positions = new();
    List<Quaternion> angles = new();
    List<Transform> transforms = new();
    List<Rigidbody> rigids = new();
    List<Interactable> objects = new();
    Quaternion camAngle;
    RotateCam rotateCam;
    List<Enemy> enemies = new();
    void Start()
    {
        Interactable[] inters = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        for(int i = 0; i < inters.Length; i++)
        {
            if(inters[i].gameObject.scene == gameObject.scene)
            {
                objects.Add(inters[i]);
            }
            Enemy enemy;
            if (inters[i].gameObject.TryGetComponent<Enemy>(out enemy))
            {
                enemies.Add(enemy);
            }
        }
        rotateCam = FindFirstObjectByType<RotateCam>();
        for (int i = 0; i < objects.Count; i++)
        {
            positions.Add(objects[i].transform.position);
            angles.Add(objects[i].transform.rotation);
            rigids.Add(objects[i].GetComponent<Rigidbody>());
            transforms.Add(objects[i].transform);
        }
        positions.Add(PlayerController.Instance.transform.position);
        angles.Add(PlayerController.Instance.transform.rotation);
        rigids.Add(PlayerController.Instance.gameObject.GetComponent<Rigidbody>());
        transforms.Add(PlayerController.Instance.transform);

        camAngle = GameManager.Instance.camTrm.rotation;
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, PlayerDie);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, PlayerDie);
    }

    void PlayerDie()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        EventBus.Publish(State.Normal);
        yield return new WaitForSeconds(4);
        for(int i = 0; i < positions.Count; i++)
        {
            rigids[i].isKinematic = true;
            transforms[i].DOMove(positions[i], 6).SetEase(Ease.OutCubic);
            transforms[i].DORotateQuaternion(angles[i], 6);
        }
        GameManager.Instance.camTrm.DORotateQuaternion(camAngle, 6);
        yield return new WaitForSeconds(6);
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].gameObject.SetActive(true);
            rigids[i].isKinematic = false;
            objects[i].ObjectReset();
        }
        rigids[rigids.Count-1].isKinematic = false;
        rotateCam.pitch = 0;
        rotateCam.yaw = 0;
        PlayerController.Instance.ObjectReset();
        GameManager.Instance.canControl = true;
    }
    public void GameClear()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            rigids[i].constraints = RigidbodyConstraints.FreezeAll;
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].DieEnemy();
        }
        GameManager.Instance.clear = true;
    }
}
