using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrushManagement : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public KeyCode startKey = KeyCode.M; // 开始移动的按键
    private bool isMoving = false; 
    private Transform mtransform; // 判断是否在移动
    private SpriteRenderer spriteRenderer; // 用于控制颜色
    private float radius; // 小球的半径
    private Vector3 initialPosition; // 初始位置

    public GameObject popupWindow; // 引用弹出窗口的游戏对象
    public Button resetButton; // 引用重置按钮
    public EggShooter eggShooter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mtransform = GetComponent<Transform>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }
        // 获取小球的半径
        radius = mtransform.localScale.x; // 假设小球是圆形的，这里使用x轴上的半径

        // 记录初始位置
        initialPosition = transform.position;

        // 确保弹出窗口开始时是隐藏的
        if (popupWindow != null)
        {
            popupWindow.SetActive(false);
        }

        // 为重置按钮添加点击事件监听器
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetGame);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(startKey))
        {
            isMoving = true;
        }

        if (isMoving)
        {
            // 移动
            transform.Translate(Vector3.right * speed * Time.deltaTime * -1);

            // 检查是否碰到摄像机边界
            Vector3 position = transform.position;
            Vector3 cameraBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

            if (position.x - radius < cameraBottomLeft.x || position.x + radius > cameraTopRight.x ||
                position.y - radius < cameraBottomLeft.y || position.y + radius > cameraTopRight.y)
            {
                // 碰到边界后闪烁
                StartCoroutine(Blink());
                if(eggShooter!=null){
            eggShooter.StopShooting();
        }
            }
            
        }
    }

    public IEnumerator Blink()
    {
        isMoving = false; // 停止移动
        float blinkDuration = 0.1f; // 闪烁的时间间隔
        int blinkTimes = 5; // 闪烁的次数

        for (int i = 0; i < blinkTimes; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(blinkDuration);
        }

        ShowPopupWindow(); // 显示弹出窗口
    }

    private void ShowPopupWindow()
    {
        if (popupWindow != null)
        {
            popupWindow.SetActive(true); // 显示弹出窗口
        }
    }

    private void ResetGame()
    {
        // 隐藏弹出窗口
        if (popupWindow != null)
        {
            popupWindow.SetActive(false);
        }

        // 恢复初始位置
        transform.position = initialPosition;

        // 恢复初始状态
        isMoving = false;
       
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        if(eggShooter!=null){
            eggShooter.StartShooting();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Plane: OnTriggerEnter2D");
        StartCoroutine(Blink());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Plane: OnTriggerStay2D");
        StartCoroutine(Blink());
    }
}
