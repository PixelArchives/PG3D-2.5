using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Boo.Lang;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(GUITexture))]
public class Joystick : MonoBehaviour
{
	[Serializable]
	[CompilerGenerated]
	internal sealed class _0024BlinkReload_002426 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal Joystick _0024self__002427;

			public _0024(Joystick self_)
			{
				_0024self__002427 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					result = (Yield(2, new WaitForSeconds(0.5f)) ? 1 : 0);
					break;
				case 2:
					_0024self__002427.blink = !_0024self__002427.blink;
					goto default;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal Joystick _0024self__002428;

		public _0024BlinkReload_002426(Joystick self_)
		{
			_0024self__002428 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024self__002428);
		}
	}

	[NonSerialized]
	private static Joystick[] joysticks;

	[NonSerialized]
	private static bool enumeratedJoysticks;

	[NonSerialized]
	private static float tapTimeDelta = 0.3f;

	public bool touchPad;

	public Rect touchZone;

	public Vector2 deadZone;

	public bool normalize;

	public Vector2 position;

	public int tapCount;

	public bool halfScreenZone;

	private int lastFingerId;

	private float tapTimeWindow;

	public Vector2 fingerDownPos;

	private float fingerDownTime;

	private float firstDeltaTime;

	private GUITexture gui;

	private Rect defaultRect;

	private Boundary guiBoundary;

	private Vector2 guiTouchOffset;

	private Vector2 guiCenter;

	public GUITexture jumpTexture;

	public bool jumpPressed;

	public Texture fireTexture;

	public Texture reloadTexture;

	public Texture reloadTextureNoAmmo;

	private Rect fireZone;

	private GameObject _playerGun;

	private Rect reloadZone;

	private Rect joystickZone;

	private Vector2 _lastFingerPosition;

	private bool blink;

	private bool NormalReloadMode;

	private bool isSerialShooting;

	public Joystick()
	{
		deadZone = Vector2.zero;
		lastFingerId = -1;
		firstDeltaTime = 0.5f;
		guiBoundary = new Boundary();
		NormalReloadMode = true;
	}

	public virtual void NoAmmo()
	{
		if (NormalReloadMode)
		{
			NormalReloadMode = false;
			StartCoroutine("BlinkReload");
		}
	}

	public virtual void HasAmmo()
	{
		if (!NormalReloadMode)
		{
			NormalReloadMode = true;
			StopCoroutine("BlinkReload");
			blink = false;
		}
	}

	public virtual IEnumerator BlinkReload()
	{
		return new _0024BlinkReload_002426(this).GetEnumerator();
	}

	public virtual void Start()
	{
		gui = (GUITexture)GetComponent(typeof(GUITexture));
		gui.pixelInset = new Rect(gui.pixelInset.x * (float)Screen.height / 640f, gui.pixelInset.y * (float)Screen.height / 640f, gui.pixelInset.width * (float)Screen.height / 640f, gui.pixelInset.height * (float)Screen.height / 640f);
		defaultRect = gui.pixelInset;
		Vector3 vector = transform.position;
		defaultRect.x += transform.position.x * (float)Screen.width;
		defaultRect.y += transform.position.y * (float)Screen.height;
		float num = 1.2f;
		if (halfScreenZone)
		{
			defaultRect.y = 0f;
			defaultRect.x = (float)Screen.width / 2f;
			defaultRect.width = (float)Screen.width / 2f;
			defaultRect.height = (float)Screen.height * 0.8f;
			jumpTexture = gui;
			float num2 = (num - 1f) * 0.5f;
			jumpTexture.pixelInset = new Rect((float)Screen.width - (float)jumpTexture.texture.width * (num2 + 1f) * (float)Screen.height / 640f, (float)(jumpTexture.texture.height * Screen.height / 640) * num2 / 2f, jumpTexture.texture.width * Screen.height / 640, jumpTexture.texture.height * Screen.height / 640);
			fireZone = new Rect((float)Screen.width - (float)Screen.height * 0.4f, (float)Screen.height * 0.15f, fireTexture.width * Screen.height / 640, fireTexture.height * Screen.height / 640);
			if ((bool)reloadTexture)
			{
				reloadZone = new Rect((float)Screen.width - (float)reloadTexture.width * 1.1f * (float)Screen.height / 640f, (float)Screen.height * 0.32f, reloadTexture.width * Screen.height / 640, reloadTexture.height * Screen.height / 640);
			}
		}
		else if ((bool)reloadTexture)
		{
			reloadZone = new Rect((float)Screen.width - (float)reloadTexture.width * 1.1f * (float)Screen.height / 640f, (float)Screen.height * 0.32f, reloadTexture.width * Screen.height / 640, reloadTexture.height * Screen.height / 640);
		}
		float x = 0f;
		Vector3 vector2 = transform.position;
		float num3 = (vector2.x = x);
		Vector3 vector4 = (transform.position = vector2);
		float y = 0f;
		Vector3 vector5 = transform.position;
		float num4 = (vector5.y = y);
		Vector3 vector7 = (transform.position = vector5);
		if (touchPad)
		{
			if ((bool)gui.texture)
			{
				touchZone = defaultRect;
			}
		}
		else
		{
			joystickZone = new Rect(0f, 0f, (float)Screen.width / 2f, (float)Screen.height / 2f);
			defaultRect = gui.pixelInset;
			defaultRect.x = (float)Screen.height * 0.1f;
			defaultRect.y = (float)Screen.height * 0.1f;
			guiTouchOffset.x = defaultRect.width * 0.5f;
			guiTouchOffset.y = defaultRect.height * 0.5f;
			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;
			guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
			guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
			guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
			guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
		}
		_playerGun = GameObject.FindGameObjectWithTag("PlayerGun");

#if UNITY_STANDALONE || UNITY_EDITOR
		GetComponent<GUITexture>().enabled = false;
#endif
	}

    public virtual void Disable()
	{
		gameObject.active = false;
		enumeratedJoysticks = false;
	}

	public virtual void Enable()
	{
		gameObject.active = true;
	}

	public virtual void ResetJoystick()
	{
		if ((!halfScreenZone || !touchPad || !touchPad) && (bool)gui)
		{
			gui.pixelInset = defaultRect;
		}
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
	}

	public virtual bool IsFingerDown()
	{
		return lastFingerId != -1;
	}

	public virtual void LatchedFinger(int fingerId)
	{
		if (lastFingerId == fingerId)
		{
			ResetJoystick();
		}
	}

	public virtual void Update()
	{
#if !UNITY_STANDALONE
		if (!enumeratedJoysticks)
		{
			joysticks = ((Joystick[])UnityEngine.Object.FindObjectsOfType(typeof(Joystick))) as Joystick[];
			enumeratedJoysticks = true;
		}
		int touchCount = Input.touchCount;
		if (!(tapTimeWindow <= 0f))
		{
			tapTimeWindow -= Time.deltaTime;
		}
		else
		{
			tapCount = 0;
		}
		if (touchCount == 0)
		{
			ResetJoystick();
		}
		else
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Vector2 vector = touch.position - guiTouchOffset;
				bool flag = false;
				if (touchPad)
				{
					if (touchZone.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (gui.HitTest(touch.position))
				{
					flag = true;
				}
				if (isSerialShooting && (bool)fireTexture && fireZone.Contains(touch.position))
				{
					_playerGun.SendMessage("ShotPressed");
				}
				bool num = flag;
				if (num)
				{
					num = lastFingerId == -1;
					if (!num)
					{
						num = lastFingerId != touch.fingerId;
					}
				}
				bool flag2 = num;
				if (flag2)
				{
					if (touchPad)
					{
						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
						fingerDownTime = Time.time;
					}
					lastFingerId = touch.fingerId;
					if (!(tapTimeWindow <= 0f))
					{
						tapCount++;
					}
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					int j = 0;
					Joystick[] array = joysticks;
					for (int length = array.Length; j < length; j++)
					{
						if (array[j] != this)
						{
							array[j].LatchedFinger(touch.fingerId);
						}
					}
					if ((bool)fireTexture && fireZone.Contains(touch.position) && !isSerialShooting)
					{
						_playerGun.SendMessage("ShotPressed");
						continue;
					}
					if ((bool)jumpTexture && jumpTexture.pixelInset.Contains(touch.position))
					{
						jumpPressed = true;
					}
					if (touchPad && reloadZone.Contains(touch.position))
					{
						_playerGun.SendMessage("ReloadPressed");
					}
					if (touchPad)
					{
						_lastFingerPosition = touch.position;
					}
				}
				if (lastFingerId == touch.fingerId)
				{
					if (touch.tapCount > tapCount)
					{
						tapCount = touch.tapCount;
					}
					if (touchPad)
					{
						float num2 = 25f;
						position.x = Mathf.Clamp((touch.position.x - fingerDownPos.x) * 1f / 1f, 0f - num2, num2);
						position.y = Mathf.Clamp((touch.position.y - fingerDownPos.y) * 1f / 1f, 0f - num2, num2);
						fingerDownPos = touch.position;
					}
					else
					{
						float num3 = Mathf.Clamp(vector.x, guiBoundary.min.x, guiBoundary.max.x);
						Rect pixelInset = gui.pixelInset;
						float num5 = (pixelInset.x = num3);
						Rect rect2 = (gui.pixelInset = pixelInset);
						float num6 = Mathf.Clamp(vector.y, guiBoundary.min.y, guiBoundary.max.y);
						Rect pixelInset2 = gui.pixelInset;
						float num8 = (pixelInset2.y = num6);
						Rect rect4 = (gui.pixelInset = pixelInset2);
					}
					if (!flag2 && touchPad && touchZone.Contains(touch.position))
					{
						_lastFingerPosition = touch.position;
					}
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						ResetJoystick();
					}
				}
			}
		}
		if (!touchPad)
		{
			position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;
			position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
		}
		float num9 = Mathf.Abs(position.x);
		float num10 = Mathf.Abs(position.y);
		if (!(num9 >= deadZone.x))
		{
			position.x = 0f;
		}
		else if (normalize)
		{
			position.x = Mathf.Sign(position.x) * (num9 - deadZone.x) / (1f - deadZone.x);
		}
		if (!(num10 >= deadZone.y))
		{
			position.y = 0f;
		}
		else if (normalize)
		{
			position.y = Mathf.Sign(position.y) * (num10 - deadZone.y) / (1f - deadZone.y);
		}
#endif
	}

	public virtual void OnGUI()
	{
#if !UNITY_STANDALONE
		if ((bool)fireTexture)
		{
			GUI.DrawTexture(new Rect(fireZone.x, (float)Screen.height - fireZone.height - fireZone.y, fireZone.width, fireZone.height), fireTexture);
		}
		if ((bool)reloadTexture)
		{
			GUI.DrawTexture(new Rect(reloadZone.x, (float)Screen.height - reloadZone.height - reloadZone.y, reloadTexture.width * Screen.height / 640, reloadTexture.height * Screen.height / 640), NormalReloadMode ? reloadTexture : ((!blink) ? reloadTexture : reloadTextureNoAmmo));
		}
#endif
	}

	public virtual void setSeriya(bool isSeriya)
	{
		isSerialShooting = isSeriya;
	}

	public virtual void Main()
	{
	}
}
