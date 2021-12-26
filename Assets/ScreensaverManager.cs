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
		if(Input.anyKeyDown)
		{
			Application.Quit();
		}
    }
}
