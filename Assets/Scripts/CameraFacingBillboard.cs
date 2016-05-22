using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{    
    public bool Reverse = false;

    void Update()
    {
        Vector3 targetPos = transform.position + Camera.main.transform.rotation * (Reverse ? Vector3.forward : Vector3.back);
        transform.LookAt(targetPos);
    }
}