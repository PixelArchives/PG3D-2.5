using UnityEngine;

public class FireButton : MonoBehaviour
{
	public Texture2D btnTexture;

	private Shoot _shootScript;

	private float width;

	private float height;

	private GameObject _player;

	private void Awake()
	{
		_shootScript = GetComponent<Shoot>();
		width = (height = 150f);
		_player = GameObject.FindGameObjectWithTag("Player");
	}
}
