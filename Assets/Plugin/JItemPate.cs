using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMesh))]
public class JItemPate : MonoBehaviour
{
	public TextMesh txt_mesh = null;

	public void SetPateText(string txt)
	{
		txt_mesh.text = txt;
	}
	
	void Awake () {
		txt_mesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		#if UnityEditor
		transform.forward = SceneView.lastActiveSceneView.camera.transform.up;
#endif
		//transform.forward = JProxy.Instance.proxy_camera.transform.up;
	}
}
