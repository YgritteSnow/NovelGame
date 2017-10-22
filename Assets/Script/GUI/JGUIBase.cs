using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class JGUIBase : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		BoxCollider[] colliders = gameObject.GetComponentsInChildren<BoxCollider>();
		foreach( BoxCollider col in colliders )
		{
			UIEventListener eventListener = UIEventListener.Get(col.gameObject);
			MethodInfo method = this.GetClassType().GetMethod("onClick_" + col.name);
			eventListener.onClick += Delegate.CreateDelegate(typeof(UIEventListener.VoidDelegate), this, method, false) as UIEventListener.VoidDelegate;
		}
	}

	public virtual System.Type GetClassType()
	{
		return this.GetType();
	}

	public void DestroyPanel()
	{
		UIEventListener[] liseners = gameObject.GetComponentsInChildren<UIEventListener>();
		foreach (UIEventListener l in liseners)
		{
			l.onClick = null;
		}
		GameObject.Destroy(gameObject);
	}
}
