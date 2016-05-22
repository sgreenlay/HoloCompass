using UnityEngine;
using System.Collections;

public enum SceneState
{
    Waiting,
    Path,
    Navigate,
};

public class SceneStateManager : MonoBehaviour
{
    public static SceneState State = SceneState.Waiting;
}
