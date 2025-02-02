using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class EnemyBehavior : MonoBehaviour {

    // All instances of Enemy shares this one WayPoint and EnemySystem
    static private EnemySpawnSystem sEnemySystem = null;
    static public void InitializeEnemySystem(EnemySpawnSystem s) { sEnemySystem = s; }

    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private const float kEnemyEnergyLost = 0.8f;
    

    private float kSpeed = 20f;//敌人移动速度
    private int currentWaypointIndex = 0;//当前航点索引
    private bool sequentialWaypoints = true;//是否按序

    public Transform[] waypoints;//航电
    public float turnSpeed = 540f; //旋转速度（度/秒）
    public float waypointThreshold = 25.0f; //距离阈值
    #region Trigger into chase or die
    
    private void Start()
    {
        //获取全部航点
        waypoints = GameObject.Find("Waypoints").GetComponentsInChildren<Transform>();
        //排除本身
        List<Transform> waypointList = new List<Transform>(waypoints);
        waypointList.RemoveAt(0);
        waypoints = waypointList.ToArray();

        //随机选一个作为起始
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        //对齐下一航点
        AlignToNextWaypoint();
    }

    private void Update()
    {
        Patrol();//巡逻
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Emeny OnTriggerEnter");
        TriggerCheck(collision.gameObject);
    }
    private void TriggerCheck(GameObject g)
    {
        if (g.name == "Hero")
        {
            ThisEnemyIsHit();

        } else if (g.name == "Egg(Clone)")
        {
            mNumHit++;
            if (mNumHit < kHitsToDestroy)
            {
                Color c = GetComponent<Renderer>().material.color;
                c.a = c.a * kEnemyEnergyLost;
                GetComponent<Renderer>().material.color = c;
            } else
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

    private void Patrol()
    {
        if (waypoints.Length == 0) return;//没有航点

        Transform targetWaypoint = waypoints[currentWaypointIndex];//获取当前目标航点

        TurnTowardsWaypoint(targetWaypoint);//转向目标航点
        MoveForward();
        Debug.Log("present" + Vector3.Distance(transform.position, targetWaypoint.position));
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            Debug.Log("rotation" + Vector3.Distance(transform.position, targetWaypoint.position));
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
        float rotationStep = Mathf.Min(turnSpeed * Time.deltaTime, Vector3.Angle(transform.up, direction) * Mathf.Deg2Rad);
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
    #endregion


