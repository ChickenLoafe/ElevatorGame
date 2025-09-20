using System.Net;
using UnityEditor.UI;
using UnityEngine;

public class WallCheck : MonoBehaviour
{

    public float radius;
    GameObject col;
    SphereCollider sc;

    void Start()
    {
        col = new GameObject();
        col.name = "WallHideRadius";
        col.layer = 2;
        sc = col.AddComponent<SphereCollider>();
        Rigidbody rb = col.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        sc.radius = radius;
        sc.isTrigger = true;

        SphereWallCheck swc = col.AddComponent<SphereWallCheck>();

        swc.wc = this;

        col.transform.parent = transform;
        col.transform.localPosition = Vector3.zero;
    }

    public void In(Collider col)
    {
        col.gameObject.GetComponent<WallOpacity>().SetToggle(true);
    }

    public void Out(Collider col)
    {
        col.gameObject.GetComponent<WallOpacity>().SetToggle(false);

    }
}
