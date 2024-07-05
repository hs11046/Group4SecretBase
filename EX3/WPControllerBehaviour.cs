using UnityEngine;  
  
public class WPController : MonoBehaviour
{
    // 引用需要控制的游戏对象  
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public GameObject E;
    public GameObject F;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            
            ToggleWaypoint(A);
            ToggleWaypoint(B);
            ToggleWaypoint(C);
            ToggleWaypoint(D);
            ToggleWaypoint(E);
            ToggleWaypoint(F);
        }
    }

    // 切换指定waypoint的激活状态  
    private void ToggleWaypoint(GameObject waypoint)
    {
        waypoint.SetActive(!waypoint.activeSelf);
        Debug.Log(waypoint.name + " is now " + (waypoint.activeSelf ? "active" : "inactive"));
    }

    
}