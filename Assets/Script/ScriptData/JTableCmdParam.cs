using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.Reflection;

public class JTableCmdParam : MonoBehaviour {
	#region simple command 2 param type
	public struct CmdPack
	{
		public CmdPack(string c, System.Type t)
		{
			cmd = c;
			param_type = t;
		}

		public string cmd;
		public System.Type param_type;
	}

	private List<CmdPack> list_cmd = new List<CmdPack>
	{
		new CmdPack("cinema", typeof(CinemaParam)),
		new CmdPack("sound", typeof(SoundParam)),
	};
	#endregion

	#region simple params
	public class TaskParam
	{
		public string wait = "";
	}
	public class CinemaParam : TaskParam
	{
		public string pic = "";
		public float fadein = 0.5f;
		public float fadeout = 0.5f;
		public float time = 1.0f;

		public string anim = "";
		public float anim_speed = 1.0f;

		public string txt = "";
		public string txt_type = "line";
		public float txt_speed = 1.0f;
	}
	public class SoundParam : TaskParam
	{
		public string path = "";
	}
	public class PanelCmdParam : TaskParam
	{
		public string panel = "";
		public string func = "";
	}
	public class SleepParam : TaskParam
	{
		public int dura = 0;
	}
	#endregion

	#region params about chat
	public static object FillStruct(string line, Type paramType)
	{
		Assembly asm = Assembly.GetExecutingAssembly();
		object result = asm.CreateInstance(paramType.FullName);
		if (result == null)
		{
			Debug.LogError("Can't create param: " + paramType.FullName);
			return result;
		}

		Type convertType = typeof(Convert);
		Type[] typelist = { typeof(String), };

		MatchCollection match_list = Regex.Matches(line, @"\b([a-z_\d]+)\s*=\s*([a-z_\d ""]*)(?:,|$)");
		foreach (Match match_one in match_list)
		{
			string name = match_one.Groups[1].Value;
			string value = match_one.Groups[2].Value;

			FieldInfo field = paramType.GetField(name);
			if (field != null)
			{
				MethodInfo method = convertType.GetMethod("To" + field.FieldType.Name, typelist);
				try
				{
					object v = method.Invoke(null, new object[] { value, });
					field.SetValue(result, v);
				}
				catch (TargetInvocationException)
				{
					Debug.LogError("Invalid type and value!!! type=" + field.FieldType.Name + ",value=" + value);
				}
			}
			else
			{
				Debug.LogError("No field(" + name + ") of type(" + paramType.Name + ")!!!");
			}
		}
		return result;
	}

	public class ChatTypeParam : TaskParam
	{
		public string focus = ""; // 高亮的控件
		public string refresh = ""; // 刷新事件
		public string arrowto = ""; // 显示箭头至该控件
		public bool screenshake = false; // 是否进行聊天抖动
	}

	// 一条聊天
	public class ChatOneParam : TaskParam
	{
		public ChatOneParam(string p, string t, string c, bool n)
		{
			people = p;
			is_normal = n;
			type = FillStruct(t, typeof(ChatTypeParam)) as ChatTypeParam;
			content = c;
		}
		public string people;
		public ChatTypeParam type;
		public string content;
		public List<ChatOneParam> next_chat;
		public bool is_normal;
	}
	// 一段聊天
	public class ChatOneSectParam : TaskParam
	{
		public List<ChatOneParam> chat;
		public List<ChatOneSectParam> chat_sect;
		public List<int> sect_insert_idx;
	}
	// 一次聊天
	public class ChatParam : TaskParam
	{
		public bool required = false;
		public List<ChatOneSectParam> chat;
	}
	#endregion

}
