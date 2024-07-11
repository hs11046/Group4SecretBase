using UnityEngine;
using System.Collections;

public class RectangleShrink : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // 收缩持续时间（秒）
    public float waitDuration = 2.0f; // 等待时间（秒）
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private float originalBottomY;

    void Start()
    {
        originalScale = transform.localScale; // 记录原始尺寸
        originalPosition = transform.localPosition; // 记录原始位置
        // 记录Collider底边位置
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            originalBottomY = transform.position.y + boxCollider.offset.y - boxCollider.size.y / 2 * transform.localScale.y;
        }
        StartCoroutine(ScaleRoutine());
    }

    IEnumerator ScaleRoutine()
    {
        while (true)
        {
            // 收缩阶段
            yield return StartCoroutine(Shrink());

            // 等待阶段
            yield return new WaitForSeconds(waitDuration);

            // 恢复阶段
            yield return StartCoroutine(Expand());

            // 等待阶段
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator Shrink()
    {
        float elapsedTime = 0;
        Vector3 targetScale = new Vector3(originalScale.x, 0, originalScale.z);

        while (elapsedTime < shrinkDuration)
        {
            float progress = elapsedTime / shrinkDuration;
            float newScaleY = Mathf.Lerp(originalScale.y, 0, progress);
            transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);

            // 调整位置以确保底边不变
            float newBottomY = transform.position.y + GetComponent<BoxCollider2D>().offset.y - GetComponent<BoxCollider2D>().size.y / 2 * newScaleY;
            float offset = originalBottomY - newBottomY;
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // 确保最终缩小为0
        transform.position = new Vector3(transform.position.x, originalBottomY + GetComponent<BoxCollider2D>().size.y / 2 * targetScale.y - GetComponent<BoxCollider2D>().offset.y, transform.position.z);
    }

    IEnumerator Expand()
    {
        float elapsedTime = 0;
        Vector3 targetScale = originalScale;

        while (elapsedTime < shrinkDuration)
        {
            float progress = elapsedTime / shrinkDuration;
            float newScaleY = Mathf.Lerp(0, originalScale.y, progress);
            transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);

            // 调整位置以确保底边不变
            float newBottomY = transform.position.y + GetComponent<BoxCollider2D>().offset.y - GetComponent<BoxCollider2D>().size.y / 2 * newScaleY;
            float offset = originalBottomY - newBottomY;
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // 确保最终恢复到原始大小
        transform.position = new Vector3(transform.position.x, originalBottomY + GetComponent<BoxCollider2D>().size.y / 2 * targetScale.y - GetComponent<BoxCollider2D>().offset.y, transform.position.z);
    }
}
