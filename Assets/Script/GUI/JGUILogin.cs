using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JGUILogin : JGUIBase {

	public void onClick_Btn_Start(GameObject sender)
	{
		JTaskManager.Instance.StartTask();
	}
}
