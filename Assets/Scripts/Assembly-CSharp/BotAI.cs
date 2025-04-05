using UnityEngine;

[RequireComponent(typeof(BotMovement))]
public class BotAI : MonoBehaviour
{
	private bool Agression;

	private bool deaded;

	private Transform Target;

	private Transform myTransform;

	private BotMovement _motor;

	private BotHealth _eh;

	private BotTrigger _botTrigger;

	public Transform homePoint;

	private void Start()
	{
		Target = null;
		_motor = GetComponent<BotMovement>();
		_eh = GetComponent<BotHealth>();
		myTransform = base.transform;
		_botTrigger = GetComponent<BotTrigger>();
	}

	private void Update()
	{
		if (!_eh.getIsLife() && !deaded)
		{
			SendMessage("Death");
			_botTrigger.shouldDetectPlayer = false;
			deaded = true;
		}
	}

	public void SetTarget(Transform _tgt, bool agression)
	{
		Agression = agression;
		Target = _tgt;
		_motor.SetTarget(Target, agression);
	}
}
