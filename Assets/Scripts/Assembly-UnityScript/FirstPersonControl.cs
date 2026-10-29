using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CharacterController))]
public class FirstPersonControl : MonoBehaviour
{
	public Joystick moveTouchPad;

	public Joystick rotateTouchPad;

	public Transform cameraPivot;

	public float forwardSpeed;

	public float backwardSpeed;

	public float sidestepSpeed;

	public float jumpSpeed;

	public float inAirMultiplier;

	public Vector2 rotationSpeed;

	public float tiltPositiveYAxis;

	public float tiltNegativeYAxis;

	public float tiltXAxisMinimum;

	private Transform thisTransform;

	private CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump;

	private Rect fireZone;

	private Rect jumpZone;

	private bool _jumping;

	private bool jump;

	private GameObject _playerGun;

	public FirstPersonControl()
	{
		forwardSpeed = 4f;
		backwardSpeed = 1f;
		sidestepSpeed = 1f;
		jumpSpeed = 4.5f;
		inAirMultiplier = 0.25f;
		rotationSpeed = new Vector2(2f, 1f);
		tiltPositiveYAxis = 0.6f;
		tiltNegativeYAxis = 0.4f;
		tiltXAxisMinimum = 0.1f;
		canJump = true;
	}

	public virtual void Start()
	{
		thisTransform = (Transform)GetComponent(typeof(Transform));
		character = (CharacterController)GetComponent(typeof(CharacterController));
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if ((bool)gameObject)
		{
			thisTransform.position = gameObject.transform.position;
		}
		_playerGun = GameObject.FindGameObjectWithTag("PlayerGun");
	}

	public virtual void OnEndGame()
	{
		moveTouchPad.Disable();
		if ((bool)rotateTouchPad)
		{
			rotateTouchPad.Disable();
		}
		enabled = false;
	}

	public virtual void Jumping()
	{
		if (jump)
		{
		}
	}

	public virtual void Update()
	{
		Vector3 motion;
		Vector2 vector;
		Vector2 vector2;

		if (NewInput.isMobile)
		{
			motion = thisTransform.TransformDirection(new Vector3(moveTouchPad.position.x, 0f, moveTouchPad.position.y));
			vector = new Vector2(Mathf.Abs(moveTouchPad.position.x), Mathf.Abs(moveTouchPad.position.y));
			vector2 = Vector2.zero;
		}
		else
		{
			motion = thisTransform.TransformDirection(NewInput.movement);
			vector = NewInput.movementAbs;
			vector2 = NewInput.mouseDelta;
			if (NewInput.jump) rotateTouchPad.jumpPressed = true;
		}

        motion.y = 0f;
		motion.Normalize();
		if (!(vector.y <= vector.x))
		{
			if (!(moveTouchPad.position.y <= 0f))
			{
				motion *= forwardSpeed * vector.y;
			}
			else
			{
				motion *= backwardSpeed * vector.y;
			}
		}
		else
		{
			motion *= sidestepSpeed * vector.x;
		}
		if (character.isGrounded)
		{
			canJump = true;
			jump = false;
			Joystick joystick = rotateTouchPad;
			if (canJump && joystick.jumpPressed)
			{
				joystick.jumpPressed = false;
				jump = true;
				canJump = false;
			}
			if (jump)
			{
				velocity = character.velocity;
				velocity.y = jumpSpeed;
			}
		}
		else
		{
			velocity.y += Physics.gravity.y * Time.deltaTime;
			motion.x *= inAirMultiplier;
			motion.z *= inAirMultiplier;
			if (rotateTouchPad.jumpPressed)
			{
				rotateTouchPad.jumpPressed = false;
			}
		}
		motion += velocity;
		motion += Physics.gravity;
		motion *= Time.deltaTime;
		if (!(new Vector2(motion.x, motion.z).magnitude <= 0f) && character.isGrounded)
		{
			_playerGun.SendMessage("WalkAnimation");
		}
		else
		{
			_playerGun.SendMessage("IdleAnimation");
		}
		character.Move(motion);
		if (character.isGrounded)
		{
			velocity = Vector3.zero;
		}
		if (!character.isGrounded)
		{
			return;
		}
		if (NewInput.isMobile)
		{
			if ((bool)rotateTouchPad)
			{
				vector2 = rotateTouchPad.position;
			}
			else
			{
				Vector3 acceleration = Input.acceleration;
				float num = Mathf.Abs(acceleration.x);
				if (!(acceleration.z >= 0f) && !(acceleration.x >= 0f))
				{
					if (!(num < tiltPositiveYAxis))
					{
						vector2.y = (num - tiltPositiveYAxis) / (1f - tiltPositiveYAxis);
					}
					else if (!(num > tiltNegativeYAxis))
					{
						vector2.y = (0f - (tiltNegativeYAxis - num)) / tiltNegativeYAxis;
					}
				}
				if (!(Mathf.Abs(acceleration.y) < tiltXAxisMinimum))
				{
					vector2.x = (0f - (acceleration.y - tiltXAxisMinimum)) / (1f - tiltXAxisMinimum);
				}
			}
			if (!(vector2.magnitude <= 1f))
			{
			}
			vector2 *= Time.deltaTime * 10f;
		}
        thisTransform.Rotate(0f, vector2.x, 0f, Space.World);
		cameraPivot.Rotate(0f - vector2.y, 0f, 0f);
	}

	public virtual void Main()
	{
	}
}
