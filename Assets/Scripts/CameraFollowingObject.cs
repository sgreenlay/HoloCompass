using UnityEngine;

public class CameraFollowingObject : MonoBehaviour
{
    public float Distance = 1.4f;

    void Update()
    {
        Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward.normalized * Distance;
        transform.position = targetPos;
    }
}
