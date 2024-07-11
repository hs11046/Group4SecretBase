using UnityEngine;
using System.Collections;

public class RectangleShrink : MonoBehaviour
{
    public float shrinkDuration = 1.0f; // ��������ʱ�䣨�룩
    public float waitDuration = 2.0f; // �ȴ�ʱ�䣨�룩
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private float originalBottomY;

    void Start()
    {
        originalScale = transform.localScale; // ��¼ԭʼ�ߴ�
        originalPosition = transform.localPosition; // ��¼ԭʼλ��
        // ��¼Collider�ױ�λ��
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
            float progress = elapsedTime / shrinkDuration;
            float newScaleY = Mathf.Lerp(originalScale.y, 0, progress);
            transform.localScale = new Vector3(originalScale.x, newScaleY, originalScale.z);

            // ����λ����ȷ���ױ߲���
            float newBottomY = transform.position.y + GetComponent<BoxCollider2D>().offset.y - GetComponent<BoxCollider2D>().size.y / 2 * newScaleY;
            float offset = originalBottomY - newBottomY;
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // ȷ��������СΪ0
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

            // ����λ����ȷ���ױ߲���
            float newBottomY = transform.position.y + GetComponent<BoxCollider2D>().offset.y - GetComponent<BoxCollider2D>().size.y / 2 * newScaleY;
            float offset = originalBottomY - newBottomY;
            transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; // ȷ�����ջָ���ԭʼ��С
        transform.position = new Vector3(transform.position.x, originalBottomY + GetComponent<BoxCollider2D>().size.y / 2 * targetScale.y - GetComponent<BoxCollider2D>().offset.y, transform.position.z);
    }
}
