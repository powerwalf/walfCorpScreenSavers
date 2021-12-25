using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensaverManager : MonoBehaviour
{
	private void Start()
	{
		Cursor.visible = false;
	}

	private void Update()
    {
		if(Input.anyKeyDown)
		{
			Application.Quit();
		}
    }
}
