using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 startMousePos;
    public Vector3 endMousePos;
    public GameObject currentLine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLine(GameObject line, Vector3 start, Vector3 end)
    {
        currentLine = line;
        startMousePos = start;
        endMousePos = end;
    }

    public void ClearLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine);
            currentLine = null;
        }
    }
}
