using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.

    [Range(0, .3f)] [SerializeField] private float m_movementSmoothing = .05f;  // How much to smooth out the movement

    [SerializeField] private LayerMask m_whatIsGround;                          // A mask determining what is ground to the character  
    [SerializeField] private bool m_Grounded;            // Whether or not the player is grounded.

    // event for the status above
    public delegate void OnMovementStatusChange(string statusName, bool state);
    public static event OnMovementStatusChange MovementStatusChange;

    private Rigidbody2D m_Rigidbody2D;

    private Vector2 m_velocity = Vector2.zero;
    private Animator m_Anim;

    private const float EPSILON = 0.01f;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No audioManager found in " + this.name);
        }
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        if (m_Rigidbody2D == null)
            Debug.LogError(this.name + " : RB not found");

        m_Anim = this.GetComponentInChildren<Animator>();
        if (m_Anim == null)
            Debug.LogError(this.name + " : Animator not found");

    }

    public void Move(Vector2 aSpeedInput)
    {
        m_Anim.SetFloat("SpeedX", aSpeedInput.x);
        m_Anim.SetFloat("SpeedY", aSpeedInput.y);
        Vector3 targetVelocity = aSpeedInput * Time.fixedDeltaTime;
        m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_velocity, m_movementSmoothing);
    }


}
