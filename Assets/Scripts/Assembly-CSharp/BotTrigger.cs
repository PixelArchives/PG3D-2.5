using System;
using System.Collections;
using UnityEngine;

public class BotTrigger : MonoBehaviour
{
	public bool shouldDetectPlayer = true;

	private bool _entered;

	private BotAI _eai;

	private GameObject _player;

	private GameObject _modelChild;

	private Sounds _soundClips;

	private void Start()
	{
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
		_eai = GetComponent<BotAI>();
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (shouldDetectPlayer)
		{
			if (!_entered && Vector3.Distance(base.transform.position, _player.transform.position) <= _soundClips.detectRadius)
			{
				_eai.SetTarget(_player.transform, true);
				_entered = true;
			}
			else if (_entered && Vector3.Distance(base.transform.position, _player.transform.position) > _soundClips.detectRadius)
			{
				_eai.SetTarget(null, false);
				_entered = false;
			}
		}
	}
}
