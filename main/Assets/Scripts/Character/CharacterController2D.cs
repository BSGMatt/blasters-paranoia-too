using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This code is used to control the player's movements. It's based on Brackey's Character Controller,
/// but modified to be used for top-down games instead of platformers. 
/// 
/// https://www.youtube.com/watch?v=dwcT-Dch0bA&list=PLPV2KyIb3jR6TFcFuzI2bB7TMNIIBpKMQ&index=2
/// 
/// </summary>
public class CharacterController2D : MonoBehaviour {
	[Range(0, .5f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] private Character m_character;                             // The character who will be listening to these events. 


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private const float ACTUAL_MAXIMUM_SPEED = 14;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake() {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

    private void Start() {

	}

    private void FixedUpdate() {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
				if (!wasGrounded) {
					OnLandEvent.Invoke();
					//Debug.Log("OnLandEvent.Invoke()");
				}
			}

		}

	}


	public void Move(Vector2 move, float speed) {

		// Move the character by finding the target velocity
		Vector3 targetVelocity = Vector3.zero;

		targetVelocity = new Vector2(move.x * speed, move.y * speed);

		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		// If the input is moving the player right and the player is facing left...
		if (move.x > 0 && !m_FacingRight) {
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move.x < 0 && m_FacingRight) {
			// ... flip the player.
			Flip();
		}
	}

	public void StopMoving() {
		m_Rigidbody2D.velocity = Vector3.zero;

	}


	private void Flip() {
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public float GetSmoothing() {
		return m_MovementSmoothing;
    }

	public void SetSmoothing(float smoothing) {
		m_MovementSmoothing = smoothing;
    }
}