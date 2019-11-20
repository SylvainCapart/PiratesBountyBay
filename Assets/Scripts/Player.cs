using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerStats stats;

    private AudioManager m_AudioManager;
    private Animator m_Anim;

    [SerializeField] private float m_FallBoundary = -20f;
    private const float m_DamageAnimShutOnDelay = 0.9f;
    private bool m_Damagable = true;

    public string deathSoundName = "";
    public string damageSoundName = "";

    public StatusIndicator statusIndicator;

    private void Start()
    {
        stats = PlayerStats.Instance;
        stats.CurrentHealth = stats.m_MaxHealth;

        if (statusIndicator == null)
        {
            StatusIndicator ind = GameObject.Find("PlayerHP").GetComponentInChildren<StatusIndicator>();
            if (ind != null)
                statusIndicator = ind;
        }

        statusIndicator.SetHealth(stats.CurrentHealth, stats.m_MaxHealth);

        if (m_Anim == null)
            m_Anim = GetComponent<Animator>();

        m_AudioManager = AudioManager.instance;
        if (m_AudioManager == null)
        {
            Debug.LogError("No audioManager found in " + this.name);
        }
    }

    private void OnEnable()
    {
        GameMaster.OnPlayerRespawn += OnReset;
    }

    private void OnDisable()
    {
        GameMaster.OnPlayerRespawn -= OnReset;
    }

    public void DamagePlayer(int damageReceived)
    {
        if (m_Damagable == true)
        {
            stats.CurrentHealth -= damageReceived;
            if (stats.CurrentHealth <= 0)
            {
                m_AudioManager.PlaySound(deathSoundName);
                GameMaster.KillPlayer(this);
            }
            else
            {
                m_AudioManager.PlaySound(damageSoundName);
            }

            statusIndicator.SetHealth(stats.CurrentHealth, stats.m_MaxHealth);
            StartCoroutine(TriggerDamageAnim());
            StartCoroutine(DamageInShutOff(m_DamageAnimShutOnDelay));
        }

    }

    private void Update()
    {
        statusIndicator.SetHealth(stats.CurrentHealth, stats.m_MaxHealth);
    }

    void RegenHealth()
    {
        stats.CurrentHealth += 1;
        statusIndicator.SetHealth(stats.CurrentHealth, stats.m_MaxHealth);
    }

    private IEnumerator TriggerDamageAnim()
    {
        m_Anim.SetBool("Hurt", true);
        yield return (new WaitForSeconds(m_DamageAnimShutOnDelay));
        m_Anim.SetBool("Hurt", false);
    }

    public IEnumerator DamageInShutOff(float delay)
    {
        m_Damagable = false;

        yield return new WaitForSeconds(delay);

        m_Damagable = true;
    }

    private void OnReset()
    {
        if (statusIndicator == null)
        {
            StatusIndicator ind = GameObject.Find("UIOverlay").GetComponentInChildren<StatusIndicator>();
            if (ind != null)
                statusIndicator = ind;
        }
    }

}
