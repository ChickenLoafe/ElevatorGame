using UnityEngine;

public class WallOpacity : MonoBehaviour
{
    public float opacity;
    public bool toggled;
    MeshRenderer mr;
    Color c;
    Transform player;
    public float viewAngle = 90f;
    float angle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetToggle(bool state)
    {
        toggled = state;
    }

    void Update()
    {
        c = mr.material.color;
        if (!Angle())
        {
            c.a = 1f;
            // Debug.Log(c.a);
            mr.material.color = c;
            return;
        }
        c.a = toggled ? opacity:1f;
        // Debug.Log(c.a);
        mr.material.color = c;
    }

    public bool Angle()
    {
        if (player == null) return false;
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        Vector3 forward = -Vector3.forward;
        angle = Vector3.Angle(forward, toPlayer);
        return angle >= viewAngle;
    }

    private void OnDrawGizmos()
    {
        // Draw debug cone limits
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, viewAngle, 0) * -Vector3.forward * 3);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -viewAngle, 0) * -Vector3.forward * 3);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle, 0) * -Vector3.forward * 3);

    }
}
