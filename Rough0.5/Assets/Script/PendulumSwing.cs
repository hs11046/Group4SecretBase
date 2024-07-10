using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float speed = 1.0f;  // Ðý×ªËÙ¶È

    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            transform.RotateAround(transform.position, Vector3.forward, speed * Time.deltaTime * 360);
        }
    }

    public void StopPendulum()
    {
        isMoving = false;
    }
}
