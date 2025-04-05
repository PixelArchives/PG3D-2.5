using UnityEngine;

public class GotToNextLevel : MonoBehaviour
{
	private GameObject _player;

	private GameObject _fpc;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, _player.transform.position) < 1.5f)
		{
			AutoFade.LoadLevel("Loading", 0.5f, 0.5f, Color.white);
		}
	}
}
