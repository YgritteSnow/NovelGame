using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System;

public class JTaskManager : MonoBehaviour
{
	static private JTaskManager s_instance = null;
	static public JTaskManager Instance { get { return s_instance; } }

	static private Dictionary<string, Type> s_cmd_param;
	
	private struct TaskPack
	{
		public string command;
		public JTableCmdParam.TaskParam open_param;
	}

	private List<TaskPack> m_tasks = new List<TaskPack>();
	private int m_cur_stage = 0;

	// Use this for initialization
	void Awake ()
	{
		s_instance = this;

		InitCmdParam();
		LoadTaskData();
	}

	#region load data
	void InitCmdParam()
	{
		s_cmd_param = new Dictionary<string, Type>();
		s_cmd_param["cinema"] = typeof(JTableCmdParam.CinemaParam);
		s_cmd_param["sound"] = typeof(JTableCmdParam.SoundParam);
		s_cmd_param["panelcmd"] = typeof(JTableCmdParam.PanelCmdParam);
		s_cmd_param["sleep"] = typeof(JTableCmdParam.SleepParam);
	}

	void LoadTaskData()
	{
		m_tasks.Clear();
		m_cur_stage = 0;

		//using (FileStream file = new FileStream(JGameManager.Instance.novelPath + JGameManager.Instance.gameName, FileMode.Open))
		//using (StreamReader reader = new StreamReader(JGameManager.Instance.novelPath + JGameManager.Instance.gameName))
		StreamReader reader = new StreamReader(JGameManager.Instance.novelPath + JGameManager.Instance.gameName);
		{
			int line_idx = -1;
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();
				++line_idx;

				if (Regex.Match(line, "^%").Success) // 是命令
				{
					Match cmd_match = Regex.Match(line, @"^%([a-z]+)\b");
					if (cmd_match.Success)
					{
						bool ret = false;
						string cmd = cmd_match.Groups[1].Value;
						if (s_cmd_param.ContainsKey(cmd)) // 过场动画命令
						{
							ret = ReadSimple(line, cmd, s_cmd_param[cmd], ref line_idx, ref reader);
						}
						else if (cmd == "chat") // 对话命令
						{
							ret = ReadChat(line, cmd, ref line_idx, ref reader);
						}

						if(!ret)
						{
							Debug.LogError("Read context failed(" + line_idx +"): "+line);
						}
					}
				}
			}
		}

	}
	#endregion

	#region read simple params
	object ReadParam(string line, Type paramType, ref int line_idx, ref StreamReader reader)
	{
		object result = JTableCmdParam.FillStruct(line, paramType);
		if (result == null)
			return result;

		foreach (FieldInfo field in paramType.GetFields())
		{
			if (field.FieldType == typeof(System.String) && field.GetValue(result) as string == "\"")
			{
				string value = "";
				while (!reader.EndOfStream)
				{
					string new_line = reader.ReadLine();
					++line_idx;

					if (new_line == "\"")
						break;

					if (value == "")
					{
						value = new_line;
					}
					else
					{
						value = value + "\n" + new_line;
					}
				}
				field.SetValue(result, value);
			}
		}
		return result;
	}

	bool ReadSimple(string line, string cmd, Type simple_type, ref int line_idx, ref StreamReader reader)
	{
		JTableCmdParam.TaskParam param = ReadParam(line, simple_type, ref line_idx, ref reader) as JTableCmdParam.TaskParam;
		if(param == null)
		{
			Debug.LogError(string.Format("Read cinema param failed!!! line={0}, content={1}", line_idx, line));
			return false;
		}

		TaskPack pack = new TaskPack();
		pack.command = cmd;
		pack.open_param = param;
		
		m_tasks.Add(pack);
		return true;
	}
	#endregion

	#region read chat params
	JTableCmdParam.ChatOneParam ReadChatOne(string line, out string end_brackets, ref int line_idx, ref StreamReader reader)
	{
		end_brackets = "";
		
		foreach (string k in JGameManager.Instance.data_people.Keys)
		{
			Match match_people = Regex.Match(line, @"^\s*"+k+@"(N?)(?:\((.*)\))?:(.*?)(\]*)$");
			if (match_people.Success)
			{
				end_brackets = match_people.Groups[4].Value;
				return new JTableCmdParam.ChatOneParam(k, match_people.Groups[2].Value, match_people.Groups[3].Value, match_people.Groups[1].Value == "N");
			}
		}

		Debug.LogError("Can't find people(" + line_idx + "):" + line);
		return null;
	}

	JTableCmdParam.ChatOneSectParam ReadChatSect(out string end_brackets, out bool force_end, string line, ref int line_idx, ref StreamReader reader)
	{
		Debug.Log(line);
		JTableCmdParam.ChatOneSectParam result = new JTableCmdParam.ChatOneSectParam();
		result.chat = new List<JTableCmdParam.ChatOneParam>();
		result.sect_insert_idx = new List<int>();
		result.chat_sect = new List<JTableCmdParam.ChatOneSectParam>();

		end_brackets = "";
		force_end = false;
		while (!reader.EndOfStream)
		{
			if (Regex.Match(line, @"%end").Success)
			{
				force_end = true;
				return result;
			}

			Match start_bracket = Regex.Match(line, @"^\s*\[(.*)");
			if(start_bracket.Success)
			{
				string new_line = start_bracket.Groups[1].Value;
				string new_end_brackets;
				bool new_force_end;
				JTableCmdParam.ChatOneSectParam child = ReadChatSect(out new_end_brackets, out new_force_end, new_line, ref line_idx, ref reader);

				result.chat_sect.Add(child);
				result.sect_insert_idx.Add(result.chat.Count);

				if(new_force_end)
				{
					force_end = true;
					Debug.LogError("Early end!!! (" + line_idx + "):" + line);
				}

				Match new_end_bracket_match = Regex.Match(new_end_brackets, @"^\s*\](.*)");
				if(new_end_bracket_match.Success)
				{
					end_brackets = new_end_bracket_match.Groups[1].Value;
					break;
				}
			}
			else if (Regex.Match(line, @"^[A-Z]").Success)
			{
				string new_end_brackets;
				JTableCmdParam.ChatOneParam child = ReadChatOne(line, out new_end_brackets, ref line_idx, ref reader);
				if (child != null)
				{
					result.chat.Add(child);
				}

				Match new_end_bracket_match = Regex.Match(new_end_brackets, @"^\s*\](.*)");
				if (new_end_bracket_match.Success)
				{
					end_brackets = new_end_bracket_match.Groups[1].Value;
					break;
				}
			}

			line = reader.ReadLine();
			++line_idx;
		}
		return result;
	}

	bool ReadChat(string line, string cmd, ref int line_idx, ref StreamReader reader)
	{
		JTableCmdParam.ChatParam param = ReadParam(line, typeof(JTableCmdParam.ChatParam), ref line_idx, ref reader) as JTableCmdParam.ChatParam;
		if (param == null)
		{
			Debug.LogError(string.Format("Read cinema param failed!!! line={0}, content={1}", line_idx, line));
			return false;
		}

		TaskPack pack = new TaskPack();
		pack.command = cmd;
		pack.open_param = param;
		param.chat = new List<JTableCmdParam.ChatOneSectParam>();

		while (!reader.EndOfStream)
		{
			bool force_end;
			string end_brackets;
			JTableCmdParam.ChatOneSectParam chat_sect = ReadChatSect(out end_brackets, out force_end, line, ref line_idx, ref reader);
			param.chat.Add(chat_sect);

			if (force_end)
				break;
		}
		
		m_tasks.Add(pack);
		return true;
	}
	#endregion

	#region do tasks
	/// <summary>
	/// 开始任务流程
	/// </summary>
	public void StartTask()
	{
		RefreshTask();
	}

	public void RefreshTask()
	{
		if(m_cur_stage < m_tasks.Count)
		{
		}
	}
	#endregion
}
