using System;
using System.Collections;
using UnityEngine;

public class BotHealth : MonoBehaviour
{
	public string myName = "Bot";

	private bool IsLife = true;

	public Texture hitTexture;

	private BotAI ai;

	private Player_move_c healthDown;

	private bool _flashing;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private Texture _skin;

	private SkinsManager _skinsManager;

	private void Start()
	{
		_skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManager>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				_modelChild = transform.gameObject;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		_soundClips = _modelChild.GetComponent<Sounds>();
		ai = GetComponent<BotAI>();
		healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		string text = _modelChild.name + "_Level" + GlobalGameController.currentLevel;
		if (!(_skin = _skinsManager.skins[text] as Texture))
		{
			_skin = Resources.Load(text) as Texture;
			_skinsManager.skins[text] = _skin;
		}
		SetTextureRecursivelyFrom(_modelChild, _skin);
	}

	private void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (Transform item in obj.transform)
		{
			if ((bool)item.gameObject.GetComponent<Renderer>() && (bool)item.gameObject.GetComponent<Renderer>().material)
			{
				if (item.gameObject.GetComponent<Renderer>().materials.Length > 1 && _modelChild.name.Equals("Enemy3"))
				{
					Material[] materials = item.gameObject.GetComponent<Renderer>().materials;
					foreach (Material material in materials)
					{
						if (material.name.Equals("raven_skin (Instance)"))
						{
							material.mainTexture = _skin;
							break;
						}
					}
				}
				else
				{
					item.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
			}
			SetTextureRecursivelyFrom(item.gameObject, txt);
		}
	}

	private IEnumerator Flash()
	{
		_flashing = true;
		SetTextureRecursivelyFrom(_modelChild, hitTexture);
		yield return new WaitForSeconds(0.125f);
		SetTextureRecursivelyFrom(_modelChild, _skin);
		_flashing = false;
	}

	public void adjustHealth(int _health, Transform target)
	{
		if (_health < 0 && !_flashing)
		{
			StartCoroutine(Flash());
		}
		_soundClips.health += _health;
		if (_soundClips.health < 0f)
		{
			_soundClips.health = 0f;
		}
		if (_soundClips.health == 0f)
		{
			IsLife = false;
		}
		else
		{
			GlobalGameController.Score += 5;
		}
		base.GetComponent<AudioSource>().PlayOneShot(_soundClips.hurt);
		ai.SetTarget(target, true);
	}

	public bool getIsLife()
	{
		return IsLife;
	}
}
