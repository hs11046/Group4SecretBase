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
    

    private float kSpeed = 20f;//�����ƶ��ٶ�
    private int currentWaypointIndex = 0;//��ǰ��������
    private bool sequentialWaypoints = true;//�Ƿ���

    public Transform[] waypoints;//����
    public float turnSpeed = 540f; //��ת�ٶȣ���/�룩
    public float waypointThreshold = 25.0f; //������ֵ
    #region Trigger into chase or die
    
    private void Start()
    {
        //��ȡȫ������
        waypoints = GameObject.Find("Waypoints").GetComponentsInChildren<Transform>();
        //�ų�����
        List<Transform> waypointList = new List<Transform>(waypoints);
        waypointList.RemoveAt(0);
        waypoints = waypointList.ToArray();

        //���ѡһ����Ϊ��ʼ
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        //������һ����
        AlignToNextWaypoint();
    }

    private void Update()
    {
        Patrol();//Ѳ��
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
        if (waypoints.Length == 0) return;//û�к���

        Transform targetWaypoint = waypoints[currentWaypointIndex];//��ȡ��ǰĿ�꺽��

        TurnTowardsWaypoint(targetWaypoint);//ת��Ŀ�꺽��
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


