using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    private bool m_IsMovementAllowed = true;
    [SerializeField] private float mySpeedMovement = 40f;
    Vector2 myMoveInput = Vector2.zero;
    private bool m_FacingRight = true;

    public bool IsMovementAllowed
    {
        get
        {
            return m_IsMovementAllowed;
        }

        set
        {
            if (m_IsMovementAllowed == value) return;
            if (value == false)
            {
                myMoveInput = Vector2.zero;
            }
            m_IsMovementAllowed = value;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMovementAllowed) return;

        myMoveInput.x = Input.GetAxisRaw("Horizontal");
        myMoveInput.y = Input.GetAxisRaw("Vertical");

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(myMoveInput * mySpeedMovement);
    }

    private void FlipScale()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}
