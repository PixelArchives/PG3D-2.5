using UnityEngine;

public class WeaponBonus : MonoBehaviour
{
	public int index;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
	}

	private void Update()
	{
		float num = 120f;
		base.transform.Rotate(base.transform.InverseTransformDirection(Vector3.up), num * Time.deltaTime);
		if (Vector3.Distance(base.transform.position, _player.transform.position) < 1.5f)
		{
			_playerMoveC.ChangeWeapon(index, false);
			Object.Destroy(base.gameObject);
		}
	}
}
