using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensaverManager : MonoBehaviour
{
	private void Start()
	{
#if !UNITY_EDITOR
		Cursor.visible = false;
#endif
	}

	private void Update()
    {
		if(IsWhitelistedInput())
		{
			return;
		}

		if(Input.anyKeyDown)
		{
			Application.Quit();
		}
    }

	protected bool IsWhitelistedInput()
	{
		return Input.GetMouseButtonDown(0) || Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.S);
	}
}
