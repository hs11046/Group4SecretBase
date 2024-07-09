using UnityEngine;
using System.Collections;


public class DrawLineOnMouse : MonoBehaviour
{
    public GameObject linePrefab; // Ԥ���壬�������ɴ���LineRenderer�����ֱ��
    public float lineWidth = 15.0f; // ����������ȣ����Դ���1
    public Material lineMaterial; // ���������Ĳ���
    public Material lineMaterial1;
    private LineRenderer lineRenderer;
    private LineRenderer previewLineRenderer;
    private bool isDrawing = false;
    public int Flag = 0; // ���ڿ����Ƿ���Ի���

    void Start()
    {
        // ��������Ԥ����LineRenderer
        GameObject previewLine = new GameObject("PreviewLine");
        previewLineRenderer = previewLine.AddComponent<LineRenderer>();
        previewLineRenderer.startWidth = lineWidth;
        previewLineRenderer.endWidth = lineWidth;
        previewLineRenderer.material = lineMaterial1; // ���ò���
        previewLineRenderer.startColor = new Color(1, 1, 1, 0.5f); // ��͸����ɫ
        previewLineRenderer.endColor = new Color(1, 1, 1, 0.5f);   // ��͸����ɫ
        previewLineRenderer.positionCount = 2;
        previewLineRenderer.enabled = false;
    }

    void Update()
    {
        if (Flag == 1)
        {
            // ��갴��
            if (Input.GetMouseButtonDown(0))
            {
                StartDrawing();
            }
            // ����ɿ�
            if (Input.GetMouseButtonUp(0) && isDrawing)
            {
                EndDrawing();
            }

            // ����Ԥ���߶�
            if (isDrawing)
            {
                UpdatePreviewLine();
            }
        }
    }

    void StartDrawing()
    {
        // ��¼��ʼλ��
        GameManager.instance.startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.instance.startMousePos.z = 0; // ȷ����ͬһƽ����

        // ����ɵ��߶�
        GameManager.instance.ClearLine();

        // ������ֱ��
        GameManager.instance.currentLine = Instantiate(linePrefab);
        lineRenderer = GameManager.instance.currentLine.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial; // ���ò���
        lineRenderer.SetPosition(0, GameManager.instance.startMousePos);
        lineRenderer.SetPosition(1, GameManager.instance.startMousePos);

        // ����Ԥ���߶�
        previewLineRenderer.SetPosition(0, GameManager.instance.startMousePos);
        previewLineRenderer.SetPosition(1, GameManager.instance.startMousePos);
        previewLineRenderer.enabled = true;

        isDrawing = true;
    }

    void EndDrawing()
    {
        // ��¼����λ��
        GameManager.instance.endMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.instance.endMousePos.z = 0; // ȷ����ͬһƽ����

        lineRenderer.SetPosition(1, GameManager.instance.endMousePos);

        // �����ײ���
        BoxCollider2D boxCollider = GameManager.instance.currentLine.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true; // ����Ϊ������

        // ����BoxCollider�Ĵ�С������λ��
        float lineLength = Vector3.Distance(GameManager.instance.startMousePos, GameManager.instance.endMousePos);
        boxCollider.size = new Vector2(lineLength, lineWidth);

        // ����BoxCollider������λ�ú���ת
        Vector3 midPoint = (GameManager.instance.startMousePos + GameManager.instance.endMousePos) / 2;
        boxCollider.transform.position = midPoint;

        float angle = Mathf.Atan2(GameManager.instance.endMousePos.y - GameManager.instance.startMousePos.y, GameManager.instance.endMousePos.x - GameManager.instance.startMousePos.x) * Mathf.Rad2Deg;
        boxCollider.transform.rotation = Quaternion.Euler(0, 0, angle);

        boxCollider.offset = Vector2.zero;

        GameManager.instance.currentLine.AddComponent<LineCollisionHandler>();

        // ����Ԥ���߶�
        previewLineRenderer.enabled = false;

        isDrawing = false;
    }

    void UpdatePreviewLine()
    {
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentMousePos.z = 0; // ȷ����ͬһƽ����

        previewLineRenderer.SetPosition(1, currentMousePos);
    }
}

public class LineCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // �ӳ�����ֱ��
        StartCoroutine(DestroyAfterDelay());
        GameManager.instance.currentLine = null;
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // �ӳ�0.1������
        Destroy(gameObject);
    }
}
