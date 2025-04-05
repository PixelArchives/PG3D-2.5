using System.Collections;
using UnityEngine;

public class BonusCreator : MonoBehaviour
{
	public GameObject[] bonusPrefabs;

	public float creationInterval = 10f;

	public float weaponCreationInterval = 30f;

	public GameObject[] weaponPrefabs;

	private int _lastWeapon = -1;

	private ArrayList bonuses = new ArrayList();

	private ArrayList _weapons = new ArrayList();

	private GameObject[] _bonusCreationZones;

	private void Start()
	{
		_bonusCreationZones = GameObject.FindGameObjectsWithTag("BonusCreationZone");
		StartCoroutine(AddBonus());
		StartCoroutine(AddWeapon());
	}

	private IEnumerator AddBonus()
	{
		while (true)
		{
			yield return new WaitForSeconds(creationInterval);
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Rect zoneRect = new Rect(spawnZone.transform.position.x - spawnZoneCollider.size.x / 2f, spawnZone.transform.position.z - spawnZoneCollider.size.z / 2f, spawnZoneCollider.size.x, spawnZoneCollider.size.z);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), 0.24f, zoneRect.y + Random.Range(0f, zoneRect.height));
			int type = Random.Range(0, 11);
			Object newBonus = (GameObject)Object.Instantiate(bonusPrefabs[(type >= 9) ? 1 : 0], pos, Quaternion.identity);
			bonuses.Add(newBonus);
			if (bonuses.Count > 5)
			{
				Object.Destroy((GameObject)bonuses[0]);
				bonuses.RemoveAt(0);
			}
		}
	}

	private IEnumerator AddWeapon()
	{
		while (true)
		{
			yield return new WaitForSeconds(weaponCreationInterval);
			GameObject spawnZone = _bonusCreationZones[Random.Range(0, _bonusCreationZones.Length)];
			BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
			Rect zoneRect = new Rect(spawnZone.transform.position.x - spawnZoneCollider.size.x / 2f, spawnZone.transform.position.z - spawnZoneCollider.size.z / 2f, spawnZoneCollider.size.x, spawnZoneCollider.size.z);
			Vector3 pos = new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), 0.24f, zoneRect.y + Random.Range(0f, zoneRect.height));
			int weaponNumber;
			do
			{
				int type = Random.Range(0, 11);
				weaponNumber = ((type >= 5) ? ((type < 8) ? 1 : 2) : 0);
			}
			while (weaponNumber == _lastWeapon);
			Object newBonus = (GameObject)Object.Instantiate(weaponPrefabs[weaponNumber], pos, (weaponNumber != 0) ? Quaternion.identity : Quaternion.Euler(270f, 90f, 0f));
			_weapons.Add(newBonus);
			if (_weapons.Count > 5)
			{
				Object.Destroy((GameObject)_weapons[0]);
				_weapons.RemoveAt(0);
			}
		}
	}
}
