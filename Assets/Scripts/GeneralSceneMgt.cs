using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralSceneMgt : MonoBehaviour
{
    public enum SceneIndex {MAIN};
    public enum GameMode { NORMAL, HARD };

    public static GeneralSceneMgt instance;

    private const float EPSILON = 0.01f;

    public GameMode m_GameMode;

    private bool m_IsMenuPlayedOnce;

    public bool IsMenuPlayedOnce
    {
        get
        {
            return m_IsMenuPlayedOnce;
        }

        set
        {
            m_IsMenuPlayedOnce = value;
        }
    }

    public delegate void GenScnMgtDlg();
    public static event GenScnMgtDlg OnMenuInit;
    public static event GenScnMgtDlg OnMenuAlreadyInit;

    private void Awake()
    {
        //Screen.fullScreen = false;

        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        if (!IsMenuPlayedOnce)
        {
            if (OnMenuInit != null)
                OnMenuInit();
            IsMenuPlayedOnce = true;
        }
        else
        {
            if (OnMenuAlreadyInit != null)
                OnMenuAlreadyInit();
        }
    }


    public void GoToScene(SceneIndex scene_index)
    {
        SceneManager.LoadScene((int)scene_index);
    }

    public void GoToMenu()
    {
        if (System.Math.Abs(Time.timeScale - 1f) > EPSILON)
            Time.timeScale = 1f;

        AudioManager.instance.StopSound(AudioManager.instance.MainSound.name);
        //AudioManager.instance.PlaySound("Guitar");
        //AudioManager.instance.MainSound = AudioManager.instance.GetSound("Guitar");
        //SceneManager.LoadScene((int)SceneIndex.MENU);
    }

    public void GoToGame(GameMode aGameMode)
    {
        m_GameMode = aGameMode;

        if (System.Math.Abs(Time.timeScale - 1f) > EPSILON)
            Time.timeScale = 1f;

        //AudioManager.instance.MainSound = AudioManager.instance.GetSound("Music");
        //SceneManager.LoadScene((int)SceneIndex.GAME);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

}
