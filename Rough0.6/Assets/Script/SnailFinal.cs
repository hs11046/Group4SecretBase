using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SnailFinal : MonoBehaviour
{
    public float initialSpeed = 70.0f; // ���ó�ʼ�ٶ�
    public float rotationSpeed = 360.0f; // ������ת�ٶȣ���/�룩
    public Rigidbody2D rb;
    private float speedMagnitude;
    private CircleCollider2D circleCollider;
    private Vector3 colliderOffset;
    private SpriteRenderer spriteRenderer; // ���ڿ�����ɫ
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private Transform mtransform;
    private float radius; // С��İ뾶
    public GameObject popupWindow; // ���õ������ڵ���Ϸ����
    public Sprite newSnailSprite; // �µ���ţͼƬ
    public int count = 0;
    public float bounceHeight = 30f; // ���ϵ����ĸ߶�
    public float bounceSpeed = 200f;
    public float fallSpeed = 500f; // ����׹����ٶ�

    public float centripetalSpeed = 150f; // �̶��������ٶ�
    private Transform stormEyeTransform;
    private bool isInStormEye = false;
    private Vector2 originalVelocity;
    public int isSound = 1;
    void Start()
    {
        count = 0;
        mtransform = GetComponent<Transform>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            audioSource1 = audioSources[0];
            audioSource2 = audioSources[1];
        }
        

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        colliderOffset = circleCollider.offset;
        radius = circleCollider.radius;
        // ������ĳ�ʼ�ٶ�
        Vector2 initialVelocity = new Vector2(0, 1).normalized * initialSpeed;
        rb.velocity = initialVelocity;
        speedMagnitude = initialSpeed; // �洢��ʼ�ٶȴ�С

        
    }

    void Update()
    {
        //��ͣ��ת
        float angle = rotationSpeed * Time.deltaTime;
        transform.RotateAround(transform.TransformPoint(colliderOffset), Vector3.forward, angle);

        //�����߽���
        Vector3 position = transform.position;
        Vector3 cameraBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        if (position.x - radius < cameraBottomLeft.x || position.x + radius > cameraTopRight.x ||
            position.y - radius < cameraBottomLeft.y || position.y + radius > cameraTopRight.y)
        {
            StartCoroutine(Blink());
        }

        //�����
        if (isInStormEye && stormEyeTransform != null)
        {
            originalVelocity = rb.velocity;
            // ��������������
            Vector2 directionToStormEye = (stormEyeTransform.position - transform.position).normalized;
            Vector2 centripetalVelocity = directionToStormEye * centripetalSpeed;

            // �ϳ��µ��ٶ�
            rb.velocity = originalVelocity + centripetalVelocity;
        }
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
            if(isSound == 1)
            {
                audioSource1.Play();
            }
            // �����߶�
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("WNH"))
        {
            if (isSound == 1)
            {
                audioSource1.Play();
            }
            Vector2 normal3;
            Vector2 incomingVector3;
            Vector2 reflectVector3;
            if (collision.GetType() == typeof(BoxCollider2D))
            {
                normal3 = collision.GetComponent<Collider2D>().transform.right;
                incomingVector3 = rb.velocity;
                reflectVector3 = Vector2.Reflect(incomingVector3, normal3);
                //Debug.LogWarning("1");
                // ����������ٶȣ�ȷ���ٶȴ�С���ֲ���
                rb.velocity = reflectVector3.normalized * speedMagnitude;
            }
            else
            {
                normal3 = collision.GetComponent<Collider2D>().transform.up;
                incomingVector3 = rb.velocity;
                reflectVector3 = Vector2.Reflect(incomingVector3, normal3);
                //Debug.LogWarning("2");
                // ����������ٶȣ�ȷ���ٶȴ�С���ֲ���
                rb.velocity = reflectVector3.normalized * speedMagnitude;
            }
        }

        if (collision.gameObject.CompareTag("WNHSHU"))
        {
            Vector2 normal3;
            if (isSound == 1)
            {
                audioSource1.Play();
            }
            Vector2 incomingVector3;
            Vector2 reflectVector3;
            if (collision.GetType() == typeof(BoxCollider2D))
            {
                normal3 = collision.GetComponent<Collider2D>().transform.right;
                incomingVector3 = rb.velocity;
                reflectVector3 = Vector2.Reflect(incomingVector3, normal3);
                Debug.LogWarning("1");
                // ����������ٶȣ�ȷ���ٶȴ�С���ֲ���
                rb.velocity = reflectVector3.normalized * speedMagnitude;
            }
            else
            {
                normal3 = collision.GetComponent<Collider2D>().transform.up;
                incomingVector3 = rb.velocity;
                reflectVector3 = Vector2.Reflect(incomingVector3, normal3);
                Debug.LogWarning("2");
                // ����������ٶȣ�ȷ���ٶȴ�С���ֲ���
                rb.velocity = reflectVector3.normalized * speedMagnitude;
            }
        }

        if (collision.gameObject.CompareTag("WH"))
        {

            StartCoroutine(Blink());
            if (isSound == 1)
            {
                audioSource2.Play();
            }

        }

        if (collision.gameObject.CompareTag("HOLE"))
        {
            Debug.Log("hole");
            this.gameObject.SetActive(false);
            if (isSound == 1)
            {
                audioSource2.Play();
            }
        }

        if (collision.gameObject.CompareTag("Trans"))
        {
            //����ţ�ŵ���Դ����ŵ�λ�ò�����һ�����ʵĳ��ٶ�

        }

        if (collision.gameObject.CompareTag("Pendulum"))
        {
            StartCoroutine(Blink());
            if (isSound == 1)
            {
                audioSource2.Play();
            }
        }

        if (collision.gameObject.CompareTag("Egg"))
        {
            StartCoroutine(Blink());
            if (isSound == 1)
            {
                audioSource2.Play();
            }
        }

        if (collision.gameObject.CompareTag("blackhole"))
        {
            stormEyeTransform = collision.transform;
            originalVelocity = rb.velocity;
            isInStormEye = true;
            if (isSound == 1)
            {
                audioSource2.Play();
            }
        }
    }

    public IEnumerator Blink()
    {
        float blinkDuration = 0.1f; // ��˸��ʱ����
        int blinkTimes = 2; // ��˸�Ĵ���
        rb.velocity = new Vector2(0, 0);
        for (int i = 0; i < blinkTimes; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(blinkDuration);
        }

        spriteRenderer.sprite = newSnailSprite; // �л���ţͼƬ
        transform.localScale = new Vector3(50f, 50f, 50f);
        if (count == 0)
        {
            Debug.Log("3");
            StartCoroutine(BounceAndFall());
            count++;
        }
       
        
    }

    private void ShowPopupWindow()
    {
        if (popupWindow != null)
        {
            popupWindow.SetActive(true); // ��ʾ��������
        }
    }

    private IEnumerator BounceAndFall()
    {
        // �����ƶ�һ�ξ���
        Vector3 targetPosition = transform.position + Vector3.up * bounceHeight;

        // ȷ������Ŀ��λ��
        transform.position = targetPosition;

        float startTime = Time.time;
        float fallDuration = 0.7f;
        // ����׹��
        while (Time.time - startTime < fallDuration)
        {
            //transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            Vector2 newPosition = rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
            yield return null;
        }
        this.gameObject.SetActive(false);
        ShowPopupWindow(); // ��ʾ��������
    }
}
