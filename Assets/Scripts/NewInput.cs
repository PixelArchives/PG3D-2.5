using UnityEngine;

public static class NewInput
{
    public static bool mouseLocked
    {
        get
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }
        set
        {
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    public static Vector2 mouseDelta
    {
        get
        {
            return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
    }

    public static Vector3 movement
    {
        get
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
    }

    public static Vector2 movementAbs
    {
        get
        {
            return new Vector2(Mathf.Abs(Input.GetAxisRaw("Horizontal")), Mathf.Abs(Input.GetAxisRaw("Vertical")));
        }
    }

    public static bool pause
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.F1); // esc acts weird in editor, doent lock mouse properly
#endif
            return Input.GetKeyDown(KeyCode.Escape);
        }
    }

    public static bool Shooting(int curAmmo)
    {
        if (curAmmo > 0) 
            return Input.GetMouseButton(0); // to prevent empty animation and sound playing every frame

        else return Input.GetMouseButtonDown(0);
    }

    public static bool reload
    {
        get
        {
            return Input.GetKeyDown(KeyCode.R);
        }
    }

    public static bool jump
    {
        get
        {
            return Input.GetKey(KeyCode.Space);
        }
    }
}
