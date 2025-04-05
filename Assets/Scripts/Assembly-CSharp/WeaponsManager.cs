using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
	public static int ammoInClipForWeapon(int idx)
	{
		int[] array = new int[4] { 12, 7, 30, 0 };
		if (idx >= array.Length)
		{
			return 0;
		}
		return array[idx];
	}

	public static int InitialAmmoInClipForWeapon(int idx)
	{
		int[] array = new int[4] { 24, 14, 30, 0 };
		if (idx >= array.Length)
		{
			return 0;
		}
		return array[idx];
	}

	public static int maxAmmoInClipForWeapon(int idx)
	{
		int[] array = new int[4] { 84, 49, 90, 0 };
		if (idx >= array.Length)
		{
			return 0;
		}
		return array[idx] * ((!PlayerPrefsX.GetBool(Defs.InAppBoughtSett, false)) ? 1 : 10);
	}

	public static float DamageFromWeapon(int idx)
	{
		float[] array = new float[4] { 50f, 100f, 25f, 50f };
		if (idx >= array.Length)
		{
			return 0f;
		}
		return array[idx];
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
