using UnityEngine;

public class EggMovement : MonoBehaviour
{
    public float speed = 1f;
    public EggShooter eggShooter;
    private void Update()
    {
        // 移动egg
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Snail")
        {
            CrushManagement crushManagement = collision.gameObject.GetComponent<CrushManagement>();
            if (crushManagement != null)
            {
                crushManagement.StartCoroutine(crushManagement.Blink());
            }
            eggShooter.StopShooting();
            Destroy(gameObject); // 销毁egg
        }
    }
    private void  OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Snail")
        {
            CrushManagement crushManagement = collision.gameObject.GetComponent<CrushManagement>();
            if (crushManagement != null)
            {
                crushManagement.StartCoroutine(crushManagement.Blink());
            }
            eggShooter.StopShooting();
            Destroy(gameObject); // 销毁egg
        }
    }
}
