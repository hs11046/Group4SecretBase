using UnityEngine;
using System.Collections;


public class DrawLineOnMouse : MonoBehaviour
{
    public GameObject linePrefab; // 预制体，用于生成带有LineRenderer组件的直线
    public float lineWidth = 15.0f; // 设置线条宽度，可以大于1
    public Material lineMaterial; // 设置线条的材质
    public Material lineMaterial1;
    private LineRenderer lineRenderer;
    private LineRenderer previewLineRenderer;
    private bool isDrawing = false;
    public int Flag = 0; // 用于控制是否可以绘制

    void Start()
    {
        // 创建用于预览的LineRenderer
        GameObject previewLine = new GameObject("PreviewLine");
        previewLineRenderer = previewLine.AddComponent<LineRenderer>();
        previewLineRenderer.startWidth = lineWidth;
        previewLineRenderer.endWidth = lineWidth;
        previewLineRenderer.material = lineMaterial1; // 设置材质
        previewLineRenderer.startColor = new Color(1, 1, 1, 0.5f); // 半透明颜色
        previewLineRenderer.endColor = new Color(1, 1, 1, 0.5f);   // 半透明颜色
        previewLineRenderer.positionCount = 2;
        previewLineRenderer.enabled = false;
    }

    void Update()
    {
        if (Flag == 1)
        {
            // 鼠标按下
            if (Input.GetMouseButtonDown(0))
            {
                StartDrawing();
            }
            // 鼠标松开
            if (Input.GetMouseButtonUp(0) && isDrawing)
            {
                EndDrawing();
            }

            // 更新预览线段
            if (isDrawing)
            {
                UpdatePreviewLine();
            }
        }
    }

    void StartDrawing()
    {
        // 记录开始位置
        GameManager.instance.startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.instance.startMousePos.z = 0; // 确保在同一平面上

        // 清除旧的线段
        GameManager.instance.ClearLine();

        // 创建新直线
        GameManager.instance.currentLine = Instantiate(linePrefab);
        lineRenderer = GameManager.instance.currentLine.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial; // 设置材质
        lineRenderer.SetPosition(0, GameManager.instance.startMousePos);
        lineRenderer.SetPosition(1, GameManager.instance.startMousePos);

        // 启用预览线段
        previewLineRenderer.SetPosition(0, GameManager.instance.startMousePos);
        previewLineRenderer.SetPosition(1, GameManager.instance.startMousePos);
        previewLineRenderer.enabled = true;

        isDrawing = true;
    }

    void EndDrawing()
    {
        // 记录结束位置
        GameManager.instance.endMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameManager.instance.endMousePos.z = 0; // 确保在同一平面上

        lineRenderer.SetPosition(1, GameManager.instance.endMousePos);

        // 添加碰撞检测
        BoxCollider2D boxCollider = GameManager.instance.currentLine.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true; // 设置为触发器

        // 设置BoxCollider的大小和中心位置
        float lineLength = Vector3.Distance(GameManager.instance.startMousePos, GameManager.instance.endMousePos);
        boxCollider.size = new Vector2(lineLength, lineWidth);

        // 设置BoxCollider的中心位置和旋转
        Vector3 midPoint = (GameManager.instance.startMousePos + GameManager.instance.endMousePos) / 2;
        boxCollider.transform.position = midPoint;

        float angle = Mathf.Atan2(GameManager.instance.endMousePos.y - GameManager.instance.startMousePos.y, GameManager.instance.endMousePos.x - GameManager.instance.startMousePos.x) * Mathf.Rad2Deg;
        boxCollider.transform.rotation = Quaternion.Euler(0, 0, angle);

        boxCollider.offset = Vector2.zero;

        GameManager.instance.currentLine.AddComponent<LineCollisionHandler>();

        // 禁用预览线段
        previewLineRenderer.enabled = false;

        isDrawing = false;
    }

    void UpdatePreviewLine()
    {
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentMousePos.z = 0; // 确保在同一平面上

        previewLineRenderer.SetPosition(1, currentMousePos);
    }
}

public class LineCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 延迟销毁直线
        StartCoroutine(DestroyAfterDelay());
        GameManager.instance.currentLine = null;
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 延迟0.1秒销毁
        Destroy(gameObject);
    }
}
