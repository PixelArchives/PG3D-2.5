using UnityEngine;

public class HealthItem : MonoBehaviour
{
	private Player_move_c test;

	private GameObject player;

	public AudioClip HealthItemUp;

	private void Start()
	{
		test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, player.transform.position) < 2f)
		{
			test.CurHealth++;
			test.gameObject.GetComponent<AudioSource>().PlayOneShot(HealthItemUp);
			Object.Destroy(base.gameObject);
			if (test.CurHealth > test.MaxHealth)
			{
				test.CurHealth = test.MaxHealth;
				GlobalGameController.Score += 100;
			}
		}
	}
}
