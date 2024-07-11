using UnityEngine;

public class BallReflection : MonoBehaviour
{
    public float initialSpeed = 5.0f; // 初始速度
    public float angle = 45.0f; // 初始角度（度）

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 将角度转换为弧度
        float angleRad = angle * Mathf.Deg2Rad;
        // 计算初始速度的方向
        Vector2 initialVelocity = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * initialSpeed;
        // 设置初始速度
        rb.velocity = initialVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 获取碰撞点的法向量
        Vector2 normal = collision.contacts[0].normal;
        // 获取当前速度向量
        Vector2 incomingVector = rb.velocity;
        // 计算反射向量
        Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);
        // 设置新的速度
        rb.velocity = reflectVector;
    }
}
