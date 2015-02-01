using UnityEngine;
using UnityEditor;
using System.Collections;

public class FrontFacingParticleGen : ScriptableWizard {
	public Material mat;
	public float size = 1f;
	public string path = "FrontFacingMesh";

	void OnWizardUpdate() {
		helpString = "Front Facing Billboard Gen";
		isValid = (mat != null) && (size > 0);
	}
	void OnWizardCreate() {
		var guid = AssetDatabase.CreateFolder("Assets", path);
		var folder = AssetDatabase.GUIDToAssetPath(guid);
		var mesh = Generate(size);
		AssetDatabase.CreateAsset(mesh, string.Format("{0}/Particle.asset", folder));
		var go = new GameObject("FrontFacingParticle", typeof(MeshFilter), typeof(MeshRenderer));
		go.GetComponent<MeshFilter>().sharedMesh = mesh;
		go.renderer.sharedMaterial = mat;
		PrefabUtility.CreatePrefab(string.Format("{0}/Particle.prefab", folder), go);
	}

	[MenuItem("Custom/FrontFacingParticle")]
	public static void CreateParticle() {
		ScriptableWizard.DisplayWizard<FrontFacingParticleGen>("Particle Gen");
	}

	public static Mesh Generate(float size) {
		var vertices = new Vector3[]{ Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		var triangles = new int[]{ 0, 3, 1, 0, 2, 3 };
		var uv = new Vector2[]{ Vector2.zero, new Vector2(1, 0), new Vector2(0, 1), Vector2.one };
		var uv2 = new Vector2[]{ 
			new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f) };
		for (var i = 0; i < uv2.Length; i++)
			uv2[i] *= size;
		var normals = new Vector3[]{ Vector3.back, Vector3.back, Vector3.back, Vector3.back };
		var tangents = new Vector4[]{ 
			new Vector4(1, 0, 0, -1), new Vector4(1, 0, 0, -1), new Vector4(1, 0, 0, -1), new Vector4(1, 0, 0, -1)
		};
		
		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.uv2 = uv2;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(Vector3.zero, Vector3.one);
		return mesh;
	}
}
