using UnityEngine;

public class AmmoItem : MonoBehaviour
{
	private Player_move_c test;

	private GameObject player;

	public AudioClip AmmoItemUp;

	private void Start()
	{
		test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, player.transform.position) < 2f && test.NeedAmmo())
		{
			test.InventoryAmmoCount += WeaponsManager.ammoInClipForWeapon(test.CurrentWeaponIndex);
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(AmmoItemUp);
			Object.Destroy(base.gameObject);
		}
		if (test.InventoryAmmoCount > WeaponsManager.maxAmmoInClipForWeapon(test.CurrentWeaponIndex))
		{
			test.InventoryAmmoCount = WeaponsManager.maxAmmoInClipForWeapon(test.CurrentWeaponIndex);
		}
	}
}
