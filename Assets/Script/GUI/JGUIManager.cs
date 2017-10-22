using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

//using ScriptData;

public class JGUIManager : MonoBehaviour {
	static private JGUIManager s_instance = null;
	static public JGUIManager Instance { get { return s_instance; } }

	private GameObject m_uiroot = null;
	Dictionary<string, System.Type> m_panel_2_script = new Dictionary<string, Type>();

	// Use this for initialization
	void Awake () {
		s_instance = this;
		m_uiroot = GameObject.Find("UI Root");

		LoadPanel2Script();
	}

	/// <summary>
	/// 加载界面和脚本之间的联系表
	/// </summary>
	void LoadPanel2Script()
	{
		Assembly asm = Assembly.GetExecutingAssembly();
		foreach(JTablePanel2Script.panel_2_script_item item in JGameManager.Instance.panel_2_script_config.panel_2_script)
		{
			m_panel_2_script.Add(item.panel_name, asm.GetType(item.script_name));
		}
	}

	void Start()
	{
		JGUIManager.Instance.CreatePanelSimple("panel_login");
	}

	public void CreatePanelSimple(string panel_name)
	{
		if (m_panel_2_script.ContainsKey(panel_name))
		{
			System.Type script_type = m_panel_2_script[panel_name];
			CreatePanel(panel_name, script_type);
		}
		else
		{
			Debug.LogError("Can't find panel(simple):" + panel_name);
		}
	}

	public void CreatePanel(string panel_name, System.Type script_type)
	{
		GameObject panel_obj = GameObject.Instantiate(Resources.Load(JGameManager.Instance.panelPath + panel_name) as GameObject, m_uiroot.transform);

		if(panel_obj)
		{
			panel_obj.AddComponent(script_type);
			panel_obj.name = panel_name;
			panel_obj.SetActive(true);
		}
		else
		{
			Debug.LogError("Can't find panel:" + panel_name);
		}
	}

}
