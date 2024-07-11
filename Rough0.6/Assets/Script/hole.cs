using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class hole : MonoBehaviour
{
    public GameObject popupPanel;
    public float collisionTime;
    public float finalscore;
    private float secondstart;
    private float end;
    private float hard = 10000;

    public GameObject animatedObject; // 在Inspector中指定包含动画的物体  
    public string animationTriggerName = "New Int"; // 动画触发器的名称  
    public float waitTime = 2.0f;
    void Start()
    {
        secondstart = RoundToDecimals(Time.time, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "snail4")
        {
            Debug.Log("Collision");
            end = RoundToDecimals(Time.time, 2);
            finalscore = RoundToDecimals(hard / end, 2);
            if (animatedObject != null)
            {

                animatedObject.SetActive(true);
                Animator animator = animatedObject.GetComponent<Animator>();
                Debug.Log("find animator");
                if (animator != null)
                {
                    Debug.Log("start");
                    //animator.SetBool(animationTriggerName, true);

                    animator.SetInteger("New Int", 1);
                    Debug.Log("done");
                }
            }
            StartCoroutine(WaitForAnimation());
        }


    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(waitTime);
        animatedObject.SetActive(false);
        collisionTime = RoundToDecimals(end - secondstart, 2);
        ShowPopup();
    }


    void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    public static float RoundToDecimals(float value, int decimals)
    {
        float multiplier = Mathf.Pow(10f, decimals);
        return Mathf.Round(value * multiplier) / multiplier;
    }




}
