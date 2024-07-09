using UnityEngine;
using System.Collections;

public class RectTransformScaler : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // ��������ʱ�䣨�룩
    public float waitDuration = 2.0f; // �ȴ�ʱ�䣨�룩
    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale; // ��¼ԭʼ�ߴ�
        originalPosition = transform.position; // ��¼ԭʼλ��
        StartCoroutine(ScaleRoutine());
    }

    IEnumerator ScaleRoutine()
    {
        while (true)
        {
            // �����׶�
            yield return StartCoroutine(Shrink());

            // �ȴ��׶�
            yield return new WaitForSeconds(waitDuration);

            // �ָ��׶�
            yield return StartCoroutine(Expand());

            // �ȴ��׶�
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
            // ����λ����ȷ��һ��̶�����
            transform.position = new Vector3(originalPosition.x, originalPosition.y - (originalScale.y - newScaleY) / 2, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // ȷ��������СΪ0
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
            // ����λ����ȷ��һ��̶�����
            transform.position = new Vector3(originalPosition.x, originalPosition.y - (originalScale.y - newScaleY) / 2, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // ȷ�����ջָ���ԭʼ��С
        transform.position = originalPosition;
    }
}
