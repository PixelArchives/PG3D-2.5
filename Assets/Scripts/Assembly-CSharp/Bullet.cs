using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float LifeTime = 1f;

	private float RespawnTime;

	public float bulletSpeed = 200f;

	private void OnCollisionEnter(Collision collision)
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		base.transform.position += base.transform.forward * bulletSpeed * Time.deltaTime;
		RespawnTime += Time.deltaTime;
		if (RespawnTime > LifeTime)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
