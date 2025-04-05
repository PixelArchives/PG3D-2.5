using UnityEngine;

public class CoinBonus : MonoBehaviour
{
	private GameObject player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, player.transform.position) < 2f)
		{
			GlobalGameController.Score += 1000;
			Object.Destroy(base.gameObject);
		}
	}
}
