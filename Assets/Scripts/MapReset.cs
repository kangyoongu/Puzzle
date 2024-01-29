using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public string nextScene;
    public Transform spawnPoint;
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
            if (inters[i].gameObject.TryGetComponent(out enemy))
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
        rigids.Add(PlayerController.Instance.gameObject.GetComponent<Rigidbody>());
        transforms.Add(PlayerController.Instance.transform);
        positions.Add(spawnPoint.position);
        angles.Add(spawnPoint.rotation);
        if (GameManager.Instance.lover)
        {
            positions.Add(spawnPoint.position + (spawnPoint.right * 2));
            angles.Add(spawnPoint.rotation);
            rigids.Add(GameManager.Instance.lover.gameObject.GetComponent<Rigidbody>());
            transforms.Add(GameManager.Instance.lover.transform);
        }
        camAngle = Quaternion.identity;
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, PlayerDie);
        EventBus.Subscribe(State.Clear, GameClear);
        transform.root.position = GameManager.Instance.currentJointPos;
        GameManager.Instance.currentJointPos = transform.root.position;
        GameManager.Instance.currentSpawnPoint = spawnPoint.position;
        GameManager.Instance.currentInfo = this;
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, PlayerDie);
        EventBus.Unsubscribe(State.Clear, GameClear);
    }

    void PlayerDie()
    {
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        EventBus.Publish(State.Normal);
        yield return new WaitForSeconds(4);
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
            enemies[i].transform.parent = null;
        }
        for(int i = 0; i < positions.Count; i++)
        {
            rigids[i].isKinematic = true;
            transforms[i].DOMove(positions[i], 6).SetEase(Ease.OutCubic);
            transforms[i].DORotateQuaternion(angles[i], 6);
        }
        GameManager.Instance.camTrm.DORotateQuaternion(camAngle, 6);
        yield return new WaitForSeconds(6);
        GravityControl.Instance.changeState = State.Up;
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].gameObject.SetActive(true);
            objects[i].ObjectReset();
        }
        for(int i = 0; i < rigids.Count; i++)
        {
            rigids[i].isKinematic = false;
        }
        rotateCam.pitch = 0;
        rotateCam.yaw = 0;
        PlayerController.Instance.ObjectReset();
        GameManager.Instance.canControl = true;
    }
    void GameClear()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            rigids[i].constraints = RigidbodyConstraints.FreezeAll;
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].gameObject.activeSelf == true)
                enemies[i].DieEnemy();
        }
        GameManager.Instance.clear = true;
    }
}
