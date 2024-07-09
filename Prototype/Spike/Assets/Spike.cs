using UnityEngine;
using System.Collections;

public class RectTransformScaler : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // 收缩持续时间（秒）
    public float waitDuration = 2.0f; // 等待时间（秒）
    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale; // 记录原始尺寸
        originalPosition = transform.position; // 记录原始位置
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
            float newScaleY = Mathf.Lerp(originalScale.y, 0, elapsedTime / shrinkDuration);
            transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);
            // 调整位置以确保一侧固定不变
            transform.position = new Vector3(originalPosition.x, originalPosition.y - (originalScale.y - newScaleY) / 2, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // 确保最终缩小为0
        transform.position = new Vector3(originalPosition.x, originalPosition.y - originalScale.y / 2, originalPosition.z);
    }

    IEnumerator Expand()
    {
        float elapsedTime = 0;
        Vector3 targetScale = originalScale;

        while (elapsedTime < shrinkDuration)
        {
            float newScaleY = Mathf.Lerp(0, originalScale.y, elapsedTime / shrinkDuration);
            transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);
            // 调整位置以确保一侧固定不变
            transform.position = new Vector3(originalPosition.x, originalPosition.y - (originalScale.y - newScaleY) / 2, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // 确保最终恢复到原始大小
        transform.position = originalPosition;
    }
}
