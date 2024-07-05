using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager sTheGlobalBehavior = null;
    public string state0 = "Sequantial";
    public string state1 = "Random";
    public int flag = 0;
    public Text mGameStateEcho = null;  // Defined in UnityEngine.UI
    public HeroBehavior mHero = null;
    private EnemySpawnSystem mEnemySystem = null;
    public BarBehaviour barbehave = null;
    private CameraSupport mMainCamera;

    private void Start()
    {
        GameManager.sTheGlobalBehavior = this;  // Singleton pattern
        Debug.Assert(mHero != null);

        mMainCamera = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(mMainCamera != null);

        Bounds b = mMainCamera.GetWorldBound();
        mEnemySystem = new EnemySpawnSystem(b.min, b.max);
    }

	void Update () {
        EchoGameState(); // always do this

        if (Input.GetKey(KeyCode.Q))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (flag == 0)
            {
                flag = 1;
            }
            else
            {
                flag = 0;
            }
            EnemyBehavior[] enemies = FindObjectsOfType<EnemyBehavior>();
            foreach (EnemyBehavior enemy in enemies)
            {
                enemy.ToggleWaypointMode();
                
            }
        }
    }


    #region Bound Support
    public CameraSupport.WorldBoundStatus CollideWorldBound(Bounds b) { return mMainCamera.CollideWorldBound(b); }
    #endregion 

    private void EchoGameState()
    {
        if(flag == 0)
            mGameStateEcho.text = mHero.GetHeroState() + "  " + mEnemySystem.GetEnemyState() + " WAYPOINTS:" + state0;
        else
            mGameStateEcho.text = mHero.GetHeroState() + "  " + mEnemySystem.GetEnemyState() + " WAYPOINTS:" + state1;
    }

    public void barreset()
    {
        Debug.LogError("No");
        //barbehave.shrink();
    } 
}