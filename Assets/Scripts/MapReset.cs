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
    DissolvingWall[] walls;
    Quaternion camAngle;
    RotateCam rotateCam;
    List<Enemy> enemies = new();
    public string nextScene;
    public Transform spawnPoint;
    public GameObject closeDoor;
    [HideInInspector] public bool isDie = false;
    [HideInInspector] public bool firstFrame = true;
    private void Awake()
    {
        transform.root.position = GameManager.Instance.currentJointPos;
        GameManager.Instance.currentJointPos = transform.root.position;
        GameManager.Instance.currentSpawnPoint = spawnPoint;
        GameManager.Instance.currentInfo = this;
        FindFirstObjectByType<StartAnimation>()?.SetStart();
    }
    void Start()
    {
        Interactable[] inters = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        walls = FindObjectsByType<DissolvingWall>(FindObjectsSortMode.None);
        for (int i = 0; i < inters.Length; i++)
        {
            if(inters[i].gameObject.scene == gameObject.scene)
            {
                objects.Add(inters[i]);
            }
            Enemy enemy;
            if (inters[i].gameObject.TryGetComponent(out enemy))
            {
                if(enemy.gameObject.scene == gameObject.scene)
                    enemies.Add(enemy);
            }
        }
        rotateCam = FindFirstObjectByType<RotateCam>();
        for (int i = 0; i < objects.Count; i++)
        {
            positions.Add(objects[i].transform.localPosition);
            angles.Add(objects[i].transform.rotation);
            rigids.Add(objects[i].GetComponent<Rigidbody>());
            transforms.Add(objects[i].transform);
        }
        rigids.Add(PlayerController.Instance.gameObject.GetComponent<Rigidbody>());
        transforms.Add(PlayerController.Instance.transform);
        positions.Add(Vector3.zero);
        angles.Add(spawnPoint.rotation);
        camAngle = Quaternion.identity;
        StartCoroutine(NextFrame());
    }
    IEnumerator NextFrame()
    {
        yield return null;
        firstFrame = false;
    }
    private void OnEnable()
    {
        EventBus.Subscribe(State.PlayerDie, PlayerDie);
        EventBus.Subscribe(State.Clear, GameClear);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(State.PlayerDie, PlayerDie);
        EventBus.Unsubscribe(State.Clear, GameClear);
    }
    public void SetSpawnPoint(int index, Vector3 pos)
    {
        positions[index] = pos;
    }
    public void PlayerDie()
    {
        StartCoroutine(Delay());
    }
    public void OnlyPlayerDie()
    {
        StartCoroutine(OnlyPlayerDelay());
    }
    IEnumerator OnlyPlayerDelay()
    {
        isDie = true;

        EventBus.Publish(State.Normal);
        yield return new WaitForSeconds(4);

        GameManager.Instance.audioSource.Play();
        rigids[objects.Count].isKinematic = true;
        transforms[objects.Count].DOLocalMove(positions[objects.Count], 4).SetEase(Ease.OutCubic);
        transforms[objects.Count].DORotateQuaternion(angles[objects.Count], 4);
        PlayerController.Instance.camTransform.DOLocalRotateQuaternion(camAngle, 4);
        yield return new WaitForSeconds(4);
        GravityControl.Instance.changeState = State.Up;

        rigids[objects.Count].isKinematic = false;
        rotateCam.pitch = 0;
        rotateCam.yaw = 0;
        PlayerController.Instance.ObjectReset();
        GameManager.Instance.canControl = true;
        yield return null;
        isDie = false;
    }
    IEnumerator Delay()
    {
        isDie = true;
        EventBus.Publish(State.Normal);
        yield return new WaitForSeconds(4);
        GameManager.Instance.audioSource.Play();
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i])
            {
                enemies[i].gameObject.SetActive(false);
                enemies[i].transform.parent = GameManager.Instance.currentInfo.transform.root;
            }
        }
        for (int i = 0; i < positions.Count; i++)
        {
            if(rigids[i] != null)
                rigids[i].isKinematic = true;
            transforms[i].DOLocalMove(positions[i], 4).SetEase(Ease.OutCubic);
            transforms[i].DORotateQuaternion(angles[i], 4);
        }
        PlayerController.Instance.camTransform.DOLocalRotateQuaternion(camAngle, 4);
        yield return new WaitForSeconds(4);
        GravityControl.Instance.changeState = State.Up;
        if(GameManager.Instance.lover.gameObject.activeSelf == true)
            GameManager.Instance.lover.ObjectReset();
        for(int i = 0; i < rigids.Count; i++)
        {
            if (rigids[i] != null)
                rigids[i].isKinematic = false;
        }
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].gameObject.SetActive(true);
            objects[i].ObjectReset();
        }
        rotateCam.pitch = 0;
        rotateCam.yaw = 0;
        PlayerController.Instance.ObjectReset();
        GameManager.Instance.canControl = true;
        yield return null;
        isDie = false;
    }
    public void GameClear()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            if (rigids[i] != null)
                rigids[i].isKinematic = true;
        }
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].gameObject.activeSelf == true)
                enemies[i].DieEnemy();
        }
        for(int i = 0; i < walls.Length; i++)
        {
            walls[i].Off();
        }
        PlayerController.Instance.grabbing = false;
        GameManager.Instance.clear = true;
    }
    public void EnemyStartPos()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].startPos = enemies[i].transform.position;
        }
    }
}
