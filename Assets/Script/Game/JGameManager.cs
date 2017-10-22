using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JGameManager : MonoBehaviour
{
	static private JGameManager s_instance = null;
	static public JGameManager Instance { get { return s_instance; } }

	public JTableGameConfig game_config;
	public JTablePanel2Script panel_2_script_config;
	public JTableLocalSave local_save;

	public string gameName { get { return game_config.game_name; } }
	public string novelPath = "Assets/NovelData/";
	public string panelPath = "Prefabs/Panel/";

	public struct PeopleData
	{
		string idname;
		string fullname;
		int gender;
	}
	public Dictionary<string, PeopleData> data_people = new Dictionary<string, PeopleData>();

	void Awake()
	{
		s_instance = this;

		LoadGameData();

		gameObject.AddComponent<JGUIManager>();
		gameObject.AddComponent<JTaskManager>();
	}
	
	// 加载游戏表格等数据
	void LoadGameData ()
	{
		data_people["M"] = new PeopleData();
		data_people["S"] = new PeopleData();
	}
}
