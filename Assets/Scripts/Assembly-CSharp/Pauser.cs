using UnityEngine;

public class Pauser : MonoBehaviour
{
	public bool pausedVar;

	private GameObject _fpc;

	private GameObject _leftJoystick;

	private GameObject _rightJoystick;

	public bool paused
	{
		get
		{
			return pausedVar;
		}
		set
		{
			pausedVar = value;
			if (pausedVar)
			{
				_leftJoystick.SendMessage("Disable");
				_rightJoystick.SendMessage("Disable");
			}
			else
			{
				_leftJoystick.active = true;
				_rightJoystick.active = true;
			}
		}
	}

	private void Start()
	{
		_fpc = GameObject.FindGameObjectWithTag("FirstPersonControl");
		GameObject[] array = GameObject.FindGameObjectsWithTag("Joystick");
		_leftJoystick = array[0];
		_rightJoystick = array[1];
    }

    private void Update()
	{
	}
}
