using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }
    
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;
    
    void Start()
    {
        Instance = this;
        
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            if (SceneStateManager.State == SceneState.Path)
            {
                var headPosition = Camera.main.transform.position;
                var gazeDirection = Camera.main.transform.forward;

                RaycastHit hitInfo;

                if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
                {
                    WaypointPinManager.Instance.CreatePin(hitInfo.point);
                }
            }
        };
        recognizer.StartCapturingGestures();
    }

    void Update()
    {
        GameObject oldFocusObject = FocusedObject;

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            FocusedObject = hitInfo.collider.gameObject;
        }
        else if (FocusedObject != null)
        {
            FocusedObject = null;
        }
        
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}