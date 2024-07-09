using UnityEngine;

public class BallReflection : MonoBehaviour
{
    public float initialSpeed = 50.0f; // ���ó�ʼ�ٶ�
    public float rotationSpeed = 360.0f; // ������ת�ٶȣ���/�룩
    private Rigidbody2D rb;
    private float speedMagnitude;
    private CircleCollider2D circleCollider;
    private Vector3 colliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        colliderOffset = circleCollider.offset;

        // ������ĳ�ʼ�ٶ�������45�Ƚ�
        Vector2 initialVelocity = new Vector2(1, -1).normalized * initialSpeed;
        rb.velocity = initialVelocity;
        speedMagnitude = initialSpeed; // �洢��ʼ�ٶȴ�С
    }

    void Update()
    {
        // �ֶ�����Χ��CircleCollider���ĵ���ת
        float angle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(transform.TransformPoint(colliderOffset), Vector3.forward, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            // ��ȡ��ײ��ķ�����
            Vector2 normal = collision.GetComponent<Collider2D>().transform.up;
            Vector2 incomingVector = rb.velocity;
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normal);

            // ����������ٶȣ�ȷ���ٶȴ�С���ֲ���
            rb.velocity = reflectVector.normalized * speedMagnitude;

            // �����߶�
            Destroy(collision.gameObject);
        }
    }
}
