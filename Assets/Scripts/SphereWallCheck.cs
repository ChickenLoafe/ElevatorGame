using Unity.VisualScripting;
using UnityEngine;

public class SphereWallCheck : MonoBehaviour
{
    public WallCheck wc;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<WallOpacity>() != null)
        {
            wc.In(collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<WallOpacity>() != null)
        {
            wc.Out(collider);
        }
    }
}
