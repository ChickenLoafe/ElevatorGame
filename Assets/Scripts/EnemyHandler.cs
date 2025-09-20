using System.Threading;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

/*
state
wander
chase


shoot a ray to player
	distance calc
		calc view
			chase
		not
			radius
				chase


chase
get player point nodemap every second walk to
 shoot a ray to player
if chasing and obs
	start timer for 4
	if never see again return to wander
	if found reset

wander
stand idly
*/

public enum EnemyState
{
    Chase,
    Idle
}
public class EnemyHandler : MonoBehaviour
{
    [Header("Nav")]
    private NavMeshAgent nav;
    public EnemyState state;
    private float timer;
    public float updateRate;

    [Header("Values")]

    public float radius;
    public float maxDistance;
    public float hearRadius;
    public bool seesPlayer = false;
    float aggroTimer;
    public float maxAggroTimer;

    [Header("Stuff")]
    public Transform player;
    public LayerMask layerMask;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.Idle;
        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Idle)
        {
            // get all Collider in a radius, should just be the player
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);
            if (hits.Length > 0) // we have a player
            {
                player = hits[0].transform;
                //distance raycast to find distance from player to enemy 
                if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, maxDistance, layerMask))
                {
                    //seeing if there is a clear line of sight to the player
                    RaycastHit viewHit;
                    if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out viewHit, maxDistance))
                    {
                        if (viewHit.transform != null)
                        {
                            Debug.Log(viewHit.transform.gameObject.name);
                        }
                        if (viewHit.transform == player)
                        {
                            state = EnemyState.Chase;
                        }
                        else
                        {
                            Collider[] hearHits = Physics.OverlapSphere(transform.position, hearRadius, layerMask);
                            if (hearHits.Length > 0) // TOO DAMN CLOSE
                            {
                                state = EnemyState.Chase;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("first if fails");
                    }
                }
                else
                {
                    Debug.Log("second if fails");
                }
            }
        }

        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chase:
                Chase();
                break;
        }
    }

    private void Chase()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            RaycastHit stillSeesPlayer;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out stillSeesPlayer, Mathf.Infinity))
            {
                if (stillSeesPlayer.transform != player)
                {
                    seesPlayer = false;
                }
                else
                {
                    seesPlayer = true;
                    aggroTimer = maxAggroTimer;
                    nav.SetDestination(player.position);
                }
            }
            if (seesPlayer == false)
            {
                aggroTimer -= Time.deltaTime * 5;
                if (aggroTimer <= 0)
                {
                    state = EnemyState.Idle;
                    Debug.Log("lost the dude");
                }
            }


        }
        
    }

    private void Idle()
    {
        //
    }


    
    void OnDrawGizmos()
    {
        // detection sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        // hearing sphere
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hearRadius);

        // ray to player
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 end = transform.position + dir * maxDistance;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, end);
        }
    }
}
