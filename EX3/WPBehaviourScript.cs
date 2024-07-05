using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPBehaviourScript : MonoBehaviour
{
    private Vector3 initialPosition;
    private int mNumHit = 0;
    private const int kHitsToDestroy = 4;
    private Color originalColor;

    //初始化
    void Start()
    {
        initialPosition = transform.position;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    //中间做的
    void Update()
    {
        
           /* if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleVisibility();
            }
           */
    }

    #region Trigger into chase or die
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerCheck(collision.gameObject);
    }

    private void TriggerCheck(GameObject g)
    {
        
        if (g.name == "Egg(Clone)")
        {
            mNumHit++;
            if (mNumHit < kHitsToDestroy)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Color currentColor = spriteRenderer.color;

                    float newAlpha = Mathf.Clamp01(currentColor.a - 0.25f); 

                    Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

                    spriteRenderer.color = newColor;

                }
            }
            else
            {
                Change();
            }
        }
    }

    private void Change()
    {
        Vector3 newPosition = initialPosition + new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), 0f);
        transform.position = newPosition;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;  
        }
    }
    #endregion

    /*private void ToggleVisibility()
    {
       
        gameObject.SetActive(!gameObject.activeSelf);
        Debug.Log("ToggleVisibility called. Game object is now " + (gameObject.activeSelf ? "active" : "inactive"));

       
    }*/


}
