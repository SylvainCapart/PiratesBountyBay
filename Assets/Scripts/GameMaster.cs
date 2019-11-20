using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    private GameObject m_SpawnPoint;
    private GameObject[] m_SpawnArray;

    [SerializeField] private Transform m_InitSpawnPoint;

    [SerializeField] private float m_SpawnDelay = 0.5f;
    [SerializeField] private float m_AppearDelay = 1f;

    public bool isRespawning = false;
    private CameraShake m_CameraShake;
    public string gameOverSoundName = "GameOver";

    private AudioManager m_AudioManager;
    [SerializeField] private string m_LastMainSoundStr = "Music";

    public bool m_DebugMode = true;

    public delegate void PlayerRespawnDelegate();
    public static event PlayerRespawnDelegate OnPlayerRespawn;
    public delegate void PlayerKillDelegate();
    public static event PlayerKillDelegate OnPlayerKill;

    void OnGUI()
    {
        if (m_DebugMode)
            GUI.Label(new Rect(0, 0, 100, 100), "" + (int)(1.0f / Time.smoothDeltaTime));
    }


    public GameObject SpawnPoint
    {
        get { return gm.m_SpawnPoint; }
        set { gm.m_SpawnPoint = value; }
    }

    private void Start()
    {
        m_CameraShake = CameraShake.instance;

        m_AudioManager = AudioManager.instance;
      
        if (OnPlayerRespawn != null)
            OnPlayerRespawn();
    }

    void Awake()
    {
        if (gm != null)
        {
            if (gm != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            gm = this;
        }

        GameObject clone = (GameObject)Instantiate(Resources.Load("Prefabs\\Player"));
        clone.transform.position = m_InitSpawnPoint.position + new Vector3(0f, 0.5f, 0f);
        clone.name = "Player";

    }

    public IEnumerator RespawnPlayer()
    {
        isRespawning = true;
        
        yield return new WaitForSeconds(m_SpawnDelay);
        /*
        GameObject cloneappear = (GameObject)Instantiate(Resources.Load("Prefabs\\AppearPlayer"));
        cloneappear.transform.position = m_SpawnPoint.transform.position + new Vector3(0f, 0.5f, 0f);
        cloneappear.name = "AppearPlayer";

        yield return new WaitForSeconds(m_AppearDelay);

        Destroy(cloneappear);

        GameObject clone = (GameObject)Instantiate(Resources.Load("Prefabs\\Player"));
        clone.transform.position = m_SpawnPoint.transform.position + new Vector3(0f, 0.5f, 0f);
        clone.name = "Player";

        if (OnPlayerRespawn != null)
            OnPlayerRespawn(); // called to relink the statusindicator in the new player instance*/
        isRespawning = false;

    }

    public static void KillPlayer(Player player)
    {


        if (player.gameObject != null)
        {
            if (OnPlayerKill != null)
                OnPlayerKill(); // called to deactivate the audiosource linked to the player's audiolistener that is to be destroyed

            AudioManager.instance.CrossFade(AudioManager.instance.MainSound.name, gm.m_LastMainSoundStr, 2f, 2f, AudioManager.instance.GetSound(gm.m_LastMainSoundStr).initVol);

            Destroy(player.gameObject);
        }
        else return;

        gm.StartCoroutine(gm.RespawnPlayer());
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //sound
        if (_enemy.m_DeathSoundName != "")
            m_AudioManager.PlaySound(_enemy.m_DeathSoundName);

        //camerashake
        m_CameraShake.Shake(_enemy.m_ShakeAmountAmt, _enemy.m_ShakeLength);

        Destroy(_enemy.gameObject);

    }
    public static void InitializePlayerRespawn(Player player)
    {
        gm.m_SpawnPoint = player.gameObject;
    }

    public void ResetFlagsExcept(GameObject gameObj)
    {
        for (int i = 0; i < m_SpawnArray.Length; i++)
        {
            if (m_SpawnArray[i].GetComponent<RespawnFlagMgt>().State == RespawnFlagMgt.FlagState.GREEN
            && m_SpawnArray[i] != gameObj)
                m_SpawnArray[i].GetComponent<RespawnFlagMgt>().State = RespawnFlagMgt.FlagState.RED;
        }
    }

    public string LastMainSoundStr
    {
        get
        {
            return m_LastMainSoundStr;
        }

        set
        {
            m_LastMainSoundStr = value;
        }
    }


}
