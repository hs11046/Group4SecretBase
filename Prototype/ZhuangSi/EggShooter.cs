using UnityEngine;
using System.Collections;

public class EggShooter : MonoBehaviour
{
    public GameObject eggPrefab; // 引用egg预制体
    public float shootInterval = 3f; // 发射间隔
    public float eggSpeed = 4f; // egg的移动速度
    private bool canShoot = true;// 是否可以继续发射
    public Sprite newSprite; // 新的Sprite引用
    private SpriteRenderer spriteRenderer; // 用于更换Sprite
    private Sprite originalSprite; 
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }
        else
        {
            // 保存原始的Sprite
            originalSprite = spriteRenderer.sprite;
        }
        // 开始发射循环
        StartCoroutine(ShootEggs());
    }

    private IEnumerator ShootEggs()
    {
        while (true)
        {
            if(canShoot)
            {
                ShootEgg();
                
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }
    // public void ChangeBack(){
    //     if (spriteRenderer != null)
    //     {
    //         spriteRenderer.sprite = originalSprite;
    //     }
    // }
    // public void ChangeTexture(){
    //     if (spriteRenderer != null && newSprite != null)
    //     {
    //         spriteRenderer.sprite = newSprite;
    //     }
    // }

    private void ShootEgg()
    {
        StartCoroutine(ResetSpriteBeforeDelay());
        // 更换Sprite为新Sprite
        // if (spriteRenderer != null && newSprite != null)
        // {
        //     spriteRenderer.sprite = newSprite;
        // }

        // 实例化egg
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        
        // 设置egg的移动脚本
        EggMovement eggMovement = egg.GetComponent<EggMovement>();
        if (eggMovement != null)
        {
            eggMovement.speed = eggSpeed;
            eggMovement.eggShooter = this;
        }
        // 启动协程将Sprite换回原来的
        StartCoroutine(ResetSpriteAfterDelay());
    }

    private IEnumerator ResetSpriteAfterDelay()
    {
        // 等待一段时间后换回原来的Sprite
        yield return new WaitForSeconds(0.5f); 
        if (spriteRenderer != null )
        {
            spriteRenderer.sprite = originalSprite;
        }// 这里的0.1f可以调整，表示在发射后多长时间换回原来的Sprite
    }
    private IEnumerator ResetSpriteBeforeDelay()
    {
        if (spriteRenderer != null&&newSprite!=null)
        {
            spriteRenderer.sprite = newSprite;
        }
        yield return new WaitForSeconds(1f);
    }

    public void StopShooting(){
        canShoot=false;
    }
    public void StartShooting(){
        canShoot=true;
    }
}
