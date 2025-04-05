using System.Collections;
using UnityEngine;

public class ZombieCreator : MonoBehaviour
{
	private GameObject[] _teleports;

	public Transform[] zombiePrefabs;

	private int _numOfLiveZombies;

	private int _numOfDeadZombies;

	private int _numOfDeadZombsSinceLastFast;

	public float curInterval = 10f;

	private GameObject[] _enemyCreationZones;

	public int NumOfLiveZombies
	{
		get
		{
			return _numOfLiveZombies;
		}
		set
		{
			_numOfLiveZombies = value;
		}
	}

	public int NumOfDeadZombies
	{
		get
		{
			return _numOfDeadZombies;
		}
		set
		{
			int num = value - _numOfDeadZombies;
			_numOfDeadZombies = value;
			NumOfLiveZombies -= num;
			_numOfDeadZombsSinceLastFast += num;
			if (_numOfDeadZombsSinceLastFast == 12)
			{
				if (curInterval > 5f)
				{
					curInterval -= 5f;
				}
				_numOfDeadZombsSinceLastFast = 0;
			}
			if (_numOfDeadZombies >= GlobalGameController.EnemiesToKill)
			{
				GameObject[] teleports = _teleports;
				foreach (GameObject gameObject in teleports)
				{
					gameObject.SetActive(true);
				}
			}
		}
	}

	private void Start()
	{
		_enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
		_teleports = GameObject.FindGameObjectsWithTag("Portal");
		GameObject[] teleports = _teleports;
		foreach (GameObject gameObject in teleports)
		{
			gameObject.SetActive(false);
		}
		curInterval = Mathf.Max(1f, curInterval - (float)GlobalGameController.AllLevelsCompleted);
		StartCoroutine(AddZombies());
	}

	private void Update()
	{
	}

	private IEnumerator AddZombies()
	{
		int numOfZombsToAdd = GlobalGameController.ZombiesInWave;
		float halfLLength = 17f;
		float radius = 2.5f;
		do
		{
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, GlobalGameController.EnemiesToKill - (NumOfDeadZombies + NumOfLiveZombies));
			for (int i = 0; i < numOfZombsToAdd; i++)
			{
				int typeOfZomb = Random.Range(0, 3);
				GameObject spawnZone = _enemyCreationZones[Random.Range(0, _enemyCreationZones.Length)];
				BoxCollider spawnZoneCollider = spawnZone.GetComponent<BoxCollider>();
				Rect zoneRect = new Rect(spawnZone.transform.position.x - spawnZoneCollider.size.x / 2f, spawnZone.transform.position.z - spawnZoneCollider.size.z / 2f, spawnZoneCollider.size.x, spawnZoneCollider.size.z);
				Object.Instantiate(position: new Vector3(zoneRect.x + Random.Range(0f, zoneRect.width), 0f, zoneRect.y + Random.Range(0f, zoneRect.height)), original: zombiePrefabs[typeOfZomb], rotation: Quaternion.identity);
			}
			yield return new WaitForSeconds(curInterval);
		}
		while (NumOfDeadZombies + NumOfLiveZombies < GlobalGameController.EnemiesToKill);
	}
}
