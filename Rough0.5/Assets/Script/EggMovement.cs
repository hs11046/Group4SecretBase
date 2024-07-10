using UnityEngine;

public class EggMovement : MonoBehaviour
{
    public float speed = 100f;
    public EggShooter eggShooter;
    private void Update()
    {
        // 移动egg
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    //private void  OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "snail4")
    //    {
    //        SnailFinal snailcollision = collision.gameObject.GetComponent<SnailFinal>();
    //        if (snailcollision != null)
    //        {
    //            snailcollision.StartCoroutine(snailcollision.Blink());
    //        }
    //        eggShooter.StopShooting();
    //        Destroy(gameObject); // 销毁egg
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "snail4")
    //    {
    //        SnailFinal snailcollision = collision.gameObject.GetComponent<SnailFinal>();
    //        if (snailcollision != null)
    //        {
    //            snailcollision.StartCoroutine(snailcollision.Blink());
    //        }
    //        eggShooter.StopShooting();
    //        Destroy(gameObject); // 销毁egg
    //    }
    //}
}
