using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Scriptable/Panel2Script")]
public class JTablePanel2Script : ScriptableObject {

	[System.Serializable]
	public struct panel_2_script_item
	{
		public string panel_name;
		public string script_name;
	}

	public List<panel_2_script_item> panel_2_script = new List<panel_2_script_item>();
}
