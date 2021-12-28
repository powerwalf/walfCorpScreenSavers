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
		string[] args = System.Environment.GetCommandLineArgs();
		string input = "";
		for (int i = 0; i < args.Length; i++)
		{
			//Debug.Log("ARG " + i + ": " + args[i]);
			// /s must be in the command line args or we should bail out (/p "preview" and /c "configuration" not currently supported)
			if (args[i] == "/s")
			{
				return;
			}
		}
		Application.Quit();
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
		return Input.GetMouseButtonDown(0) || Input.GetMouseButton(1)
#if UNITY_EDITOR
			|| Input.GetKeyDown(KeyCode.S);
#else
			;
#endif
	}
}
