using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;

    public static PlayerStats Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }

    }

    [SerializeField] private int m_CurrentHealth;

    public int m_MaxHealth = 100;
    public float m_StartPcHealh = 1f;
    public float m_HealthRegenRate = 2f;
    public int CurrentHealth
    {
        get { return m_CurrentHealth; }
        set { m_CurrentHealth = Mathf.Clamp(value, 0, m_MaxHealth); }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CurrentHealth = (int)(m_StartPcHealh * m_MaxHealth);
    }
}

