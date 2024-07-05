using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class EnemyBehavior111 : MonoBehaviour
{
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s) { sEnemySystem = s; }

    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private const float kEnemyEnergyLost = 0.8f;
    private float kTurnRate = 0.03f / 60f;
    private float kSpeed = 20f;
    private int currentWaypointIndex = 0;
    private bool sequentialWaypoints = true;

    public Transform[] waypoints;
    public float turnSpeed = 50.0f; // Degrees per second for turning
    public float waypointThreshold = 10f; // Distance to start turning
    private bool isTurning = false;
    public Transform HostXform = null;

    private void Start()
    {
        waypoints = GameObject.Find("Waypoints").GetComponentsInChildren<Transform>();
        List<Transform> waypointList = new List<Transform>(waypoints);
        waypointList.RemoveAt(0);
        waypoints = waypointList.ToArray();

        currentWaypointIndex = Random.Range(0, waypoints.Length);

        AlignToNextWaypoint();
    }

    private void Update()
    {
        Patrol();
    }

    #region 触发进入追逐或死亡
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerCheck(collision.gameObject);
    }

    private void TriggerCheck(GameObject g)
    {
        if (g.name == "Hero")
        {
            ThisEnemyIsHit();
        }
        else if (g.name == "Egg(Clone)")
        {
            mNumHit++;
            if (mNumHit < kHitsToDestroy)
            {
                Color c = GetComponent<Renderer>().material.color;
                c.a *= kEnemyEnergyLost;
                GetComponent<Renderer>().material.color = c;
            }
            else
            {
                ThisEnemyIsHit();
            }
        }
    }

    private void ThisEnemyIsHit()
    {
        sEnemySystem.OneEnemyDestroyed();
        Destroy(gameObject);
    }
    #endregion

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        TurnTowardsWaypoint(targetWaypoint);
        MoveForward();
        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            AlignToNextWaypoint();
            if (sequentialWaypoints)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {
                currentWaypointIndex = Random.Range(0, waypoints.Length);
            }
        }
    }

    private void TurnTowardsWaypoint(Transform targetWaypoint)
    {
        Vector3 direction = targetWaypoint.position - transform.position;
        float rotationStep = turnSpeed * Time.deltaTime;
        Vector3 newUp = Vector3.RotateTowards(transform.up, direction, rotationStep * Mathf.Deg2Rad, 0.0f);
        transform.up = newUp;
    }

    private void MoveForward()
    {
        transform.position += transform.up * kSpeed * Time.deltaTime;
    }

    private void AlignToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform nextWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToNextWaypoint = (nextWaypoint.position - transform.position).normalized;
        transform.up = directionToNextWaypoint;
    }

    public void ToggleWaypointMode()
    {
        sequentialWaypoints = !sequentialWaypoints;
    }
}
