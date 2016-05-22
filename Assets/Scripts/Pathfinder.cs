using UnityEngine;
using System.Collections;

public class Pathfinder : MonoBehaviour
{
    public static Pathfinder Instance { get; private set; }

    public DirectionalIndicator Indicator;
    public Material LineMaterial;

    ArrayList path;
    LineRenderer lineRenderer;
    
    void Start()
    {
        Instance = this;

        path = new ArrayList();
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetWidth(0.02f, 0.02f);
        lineRenderer.material = LineMaterial;
    }

    public void AddNodeToPath(GameObject node)
    {
        path.Add(node);

        FindNextNode();
    }

    void FindNextNode()
    {
        if (Indicator != null)
        {
            if (path.Count > 0)
            {
                Indicator.TargetObject = (GameObject)path[0];
            }
            else
            {
                Indicator.TargetObject = null;
            }
        }

        if (path.Count > 1)
        {
            lineRenderer.SetVertexCount(path.Count);

            for (int i = 0; i < path.Count; i++)
            {
                GameObject node = (GameObject)path[i];
                lineRenderer.SetPosition(i, node.transform.position);
            }

            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void Update ()
    {
        if (SceneStateManager.State != SceneState.Navigate)
        {
            return;
        }

        if (path.Count > 0)
        {
            GameObject node = (GameObject)path[0];

            Vector3 camToObjectDirection = node.transform.position - Camera.main.transform.position;
            Vector3 planarCamToObject = Vector3.ProjectOnPlane(camToObjectDirection, Vector3.up);

            if (planarCamToObject.magnitude < Camera.main.nearClipPlane)
            {
                path.RemoveAt(0);
                node.SetActive(false);

                FindNextNode();
            }
        }
    }
}
