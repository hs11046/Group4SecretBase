using UnityEngine;

public class BallReflection : MonoBehaviour
{
    public float initialSpeed = 50.0f; // 设置初始速度
    public float rotationSpeed = 360.0f; // 设置自转速度（度/秒）
    private Rigidbody2D rb;
    private float speedMagnitude;
    private CircleCollider2D circleCollider;
    private Vector3 colliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        colliderOffset = circleCollider.offset;

        // 设置球的初始速度向右下45度角
        Vector2 initialVelocity = new Vector2(1, -1).normalized * initialSpeed;
        rb.velocity = initialVelocity;
        speedMagnitude = initialSpeed; // 存储初始速度大小
    }

    void Update()
    {
        // 手动更新围绕CircleCollider中心的旋转
        float angle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(transform.TransformPoint(colliderOffset), Vector3.forward, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            // 获取碰撞点的法向量
            Vector2 normal = collision.GetComponent<Collider2D>().transform.up;
            Vector2 incomingVector = rb.velocity;
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

            // 设置球的新速度，确保速度大小保持不变
            rb.velocity = reflectVector.normalized * speedMagnitude;

            // 销毁线段
            Destroy(collision.gameObject);
        }
    }
}
