
using System.Collections;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class WaypointPinManager : MonoBehaviour
{
    public GameObject Pin;

    public static WaypointPinManager Instance { get; private set; }

    ArrayList pins;

    void Start ()
    {
        Instance = this;

        pins = new ArrayList();
        
        Restore();
    }

    void Restore()
    {
        WorldAnchorStore.GetAsync((store) =>
        {
            var iids = store.GetAllIds();

            foreach (var iid in iids)
            {
                var gameObject = (GameObject)Instantiate(Pin, Camera.main.transform.position, Quaternion.identity);
                pins.Add(gameObject);

                Pathfinder.Instance.AddNodeToPath(gameObject);

                var pin = gameObject.GetComponent<WaypointPin>();
                pin.Name = iid;
                pin.Anchor = store.Load(iid, gameObject);
            }
        });
    }

    public void StoreAnchor(string name, WorldAnchor anchor)
    {
        WorldAnchorStore.GetAsync((store) =>
        {
            store.Save(name, anchor);
        });
    }

    public void CreatePin(Vector3 position)
    {
        var gameObject = (GameObject)Instantiate(Pin, position, Quaternion.identity);
        pins.Add(gameObject);

        Pathfinder.Instance.AddNodeToPath(gameObject);

        var pin = gameObject.GetComponent<WaypointPin>();
        pin.Name = pin.GetInstanceID().ToString();
        pin.Anchor = gameObject.AddComponent<WorldAnchor>();

        //StoreAnchor(pin.Name, pin.Anchor);
    }

    void Reset()
    {
        WorldAnchorStore.GetAsync((store) =>
        {
            store.Clear();
        });
    }
}