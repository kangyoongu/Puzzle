using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReset : MonoBehaviour
{
    List<Vector3> positions = new();
    List<Quaternion> angles = new();
    List<Transform> transforms = new();
    List<Rigidbody> rigids = new();
    Interactable[] objects;
    Quaternion camAngle;
    RotateCam rotateCam;
    void Start()
    {
        objects = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        rotateCam = FindObjectOfType<RotateCam>();
        for (int i = 0; i < objects.Length; i++)
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
        print(objects.Length);
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
        for (int i = 0; i < objects.Length; i++)
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
}
