using UnityEngine;

public class DirectionalIndicator : MonoBehaviour {

    public GameObject TargetObject { get; set; }

    public float TitleSafeFactor = 0.1f;
    public GameObject Label;

    enum State {
        Unknown,
        GoForward,
        TurnLeft,
        TurnRight,
        TurnAround
    };

    State state = State.Unknown;

    AudioSource audioSource = null;
    AudioClip goForwardClip = null;
    AudioClip turnLeftClip = null;
    AudioClip turnRightClip = null;
    AudioClip turnAroundClip = null;
    AudioClip arrivalClip = null;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        goForwardClip = Resources.Load<AudioClip>("GoForward");
        turnLeftClip = Resources.Load<AudioClip>("TurnLeft");
        turnRightClip = Resources.Load<AudioClip>("TurnRight");
        turnAroundClip = Resources.Load<AudioClip>("TurnAround");
        arrivalClip = Resources.Load<AudioClip>("YouHaveArrived");
    }

    public void Update()
    {
        if (SceneStateManager.State != SceneState.Navigate)
        {
            var textMesh = Label.GetComponent<TextMesh>();
            textMesh.text = "";

            return;
        }

        if (TargetObject == null)
        {
            if (state != State.Unknown)
            {
                audioSource.clip = arrivalClip;
                audioSource.Play();

                state = State.Unknown;
            }

            return;
        }
        else
        {
            Vector3 camToObjectDirection = TargetObject.transform.position - Camera.main.transform.position;

            Vector3 planarCamToObject = Vector3.ProjectOnPlane(camToObjectDirection, Vector3.up).normalized;
            Vector3 planarCamForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;

            float angle = Vector3.Angle(planarCamForward, planarCamToObject);

            var textMesh = Label.GetComponent<TextMesh>();

            State previousState = state;

            if (angle < 20)
            {
                state = State.GoForward;
                textMesh.text = "";
            }
            else if (angle > 135)
            {
                state = State.TurnAround;
                textMesh.text = "v";
            }
            else
            {
                float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(planarCamForward, planarCamToObject)));
                
                if (sign < 0)
                {
                    state = State.TurnLeft;
                    textMesh.text = "<";
                }
                else
                {
                    state = State.TurnRight;
                    textMesh.text = ">";
                }
            }

            if (state != previousState)
            {
                switch (state)
                {
                    case State.GoForward:
                        audioSource.clip = goForwardClip;
                        audioSource.Play();
                        break;
                    case State.TurnAround:
                        audioSource.clip = turnAroundClip;
                        audioSource.Play();
                        break;
                    case State.TurnLeft:
                        audioSource.clip = turnLeftClip;
                        audioSource.Play();
                        break;
                    case State.TurnRight:
                        audioSource.clip = turnRightClip;
                        audioSource.Play();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
