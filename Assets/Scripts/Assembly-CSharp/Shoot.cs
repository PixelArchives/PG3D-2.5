using UnityEngine;

public class Shoot : MonoBehaviour
{
	public float Range = 1000f;

	public Transform _transform;

	public GameObject bullet;

	private GameObject _bulletSpawnPoint;

	private Player_move_c _playerMoveC;

	private void Start()
	{
		_bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
		_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
	}

	public void shootS()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(bullet, _bulletSpawnPoint.transform.position, Quaternion.LookRotation(_transform.TransformDirection(Vector3.forward)));
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, -5) && (bool)hitInfo.collider.gameObject.transform.parent && hitInfo.collider.gameObject.transform.parent.CompareTag("Enemy"))
		{
			BotHealth component = hitInfo.collider.gameObject.transform.parent.GetComponent<BotHealth>();
			component.adjustHealth(-(int)WeaponsManager.DamageFromWeapon(_playerMoveC.CurrentWeaponIndex), _transform);
		}
	}
}
