using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JItemBase : MonoBehaviour
{
	public GameObject model_item;
	public GameObject model_line;
	public JItemPate model_txt;

	private List<JItemBase> list_to;
	private List<JItemBase> list_from;

	public virtual List<JItemBase> GetListTo()
	{
		return new List<JItemBase>();
	}
	public virtual List<JItemBase> GetListFrom()
	{
		return new List<JItemBase>();
	}

	// Use this for initialization
	void Awake () {
		list_to = GetListTo();
		list_from = GetListFrom();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region show params
	#endregion

	#region show links
	#endregion
}
