using UnityEngine;
using UnityEditor;
using System.Collections;

public class CubemapCapture : ScriptableWizard {
	public Transform center;
	public Cubemap cube;

	void OnWizardUpdate() {
		this.helpString = "Set properties";
		this.isValid = (center != null) && (cube != null);
	}
	void OnWizardCreate() {
		var go = new GameObject("CubemapCamera", typeof(Camera));
		go.transform.position = center.position;
		go.transform.rotation = center.rotation;
		var prevColorSpace = PlayerSettings.colorSpace;
		PlayerSettings.colorSpace = ColorSpace.Linear;
		go.camera.RenderToCubemap(cube);
		PlayerSettings.colorSpace = prevColorSpace;
		DestroyImmediate(go);
	}

	[MenuItem("Custom/RenderIntoCube")]
	public static void Capture() {
		ScriptableWizard.DisplayWizard<CubemapCapture>("Cubemap Capture", "Render");
	}
}
