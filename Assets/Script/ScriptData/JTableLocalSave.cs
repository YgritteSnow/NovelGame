using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Local Save")]
public class JTableLocalSave : ScriptableObject {
	//[System.Serializable]
	public struct TaskNodeChoice
	{
		int task_id;
		string task_name;
		List<int> choice;
	}

	public List<TaskNodeChoice> history;
}
