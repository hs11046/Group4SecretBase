using UnityEngine;  
  
public class WPController : MonoBehaviour
{
    // ������Ҫ���Ƶ���Ϸ����  
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

    // �л�ָ��waypoint�ļ���״̬  
    private void ToggleWaypoint(GameObject waypoint)
    {
        waypoint.SetActive(!waypoint.activeSelf);
        Debug.Log(waypoint.name + " is now " + (waypoint.activeSelf ? "active" : "inactive"));
    }

    
}