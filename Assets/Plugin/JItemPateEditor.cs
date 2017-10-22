using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JItemPate))]
public class JItemPateEditor : Editor {
	private Transform txt_mesh = null;
	
	void OnSceneGUI ()
	{
		Debug.Log(string.Format("on:{0}", SceneView.lastActiveSceneView.camera.transform.up));
		//txt_mesh.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
	}
}
