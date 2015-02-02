using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TangentSphere : MonoBehaviour {
	public const float CIRCLE = 2f * Mathf.PI;
	public const float EPSILON = 1e-3f;
	public const float GIZMO_LENGTH = 1f;

	public int nSubdiv = 4 * 9;

	private Mesh _mesh;

	void Start() {
		GetComponent<MeshFilter>().sharedMesh = _mesh = Generate(nSubdiv);
	}
	void OnDrawGizmos() {
		if (_mesh == null)
			return;

		var vertices = _mesh.vertices;
		var normals = _mesh.normals;
		var tangents = _mesh.tangents;
		var limit = vertices.Length;
		for (var i = 0; i < limit; i++) {
			var pos = transform.TransformPoint(vertices[i]);
			var right = transform.TransformDirection(tangents[i]);
			var forward = transform.TransformDirection(-normals[i]);
			var up = Vector3.Cross(forward, right);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(pos, right * GIZMO_LENGTH);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(pos, up * GIZMO_LENGTH);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(pos, forward * GIZMO_LENGTH);
		}
	}

	public static Mesh Generate(int nSubdiv) { 
		var mesh = new Mesh();
		return Generate(nSubdiv, ref mesh); 
	}
	public static Mesh Generate(int nSubdiv, ref Mesh mesh) {
		var dx = CIRCLE / nSubdiv;
		var dy = CIRCLE / (nSubdiv * 2);

		var nVertsOnX = nSubdiv + 1;
		var nVertsOnY = nSubdiv + 1;

		var ups = new Vector3[nVertsOnX];
		var rights = new Vector3[nVertsOnX];
		for (var x = 0; x < nVertsOnX; x++) {
			var angleZInDeg = Mathf.Repeat(-x * dx, CIRCLE) * Mathf.Rad2Deg;
			var tangent = Quaternion.Euler(0f, 0f, angleZInDeg);
			ups[x] = tangent * Vector3.up;
			rights[x] = tangent * Vector3.right;
		}

		var iVertex = 0;
		var vertices = new Vector3[nVertsOnX * nVertsOnY];
		var normals = new Vector3[vertices.Length];
		var tangents = new Vector4[vertices.Length];
		var uvs = new Vector2[vertices.Length];
		for (var x = 0; x < nVertsOnX; x++) {
			var right = rights[x];
			for (var y = 0; y < nVertsOnY; y++) {
				var angleX = Mathf.Repeat(-y * dy + CIRCLE / 4, CIRCLE);
				var angleY = Mathf.Repeat(x * dx, CIRCLE);
				var rot = Quaternion.Euler(Mathf.Rad2Deg * new Vector3(angleX, angleY, 0f));

				var forward = rot * Vector3.forward;
				vertices[iVertex] = forward;
				normals[iVertex] = -forward;
				var rotTangent = (Vector4)(rot * right);
				rotTangent.w = -1f;
				tangents[iVertex] = rotTangent;
				uvs[iVertex] = new Vector2(Mathf.Clamp01((float)x / nSubdiv), Mathf.Clamp01((float)y / nSubdiv)); 
				iVertex++;
			}
		}

		var iTriangle = 0;
		var triangles = new int[6 * nSubdiv * nSubdiv];
		for (var x = 0; x < nSubdiv; x++) {
			for (var y = 0; y < nSubdiv; y++) {
				var vOffset = y + x * nVertsOnY;
				triangles[iTriangle++] = vOffset;
				triangles[iTriangle++] = vOffset + (nVertsOnY + 1);
				triangles[iTriangle++] = vOffset + nVertsOnY;
				triangles[iTriangle++] = vOffset;
				triangles[iTriangle++] = vOffset + 1;
				triangles[iTriangle++] = vOffset + (nVertsOnY + 1);
			}
		}

		if (mesh == null)
			mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		return mesh;
	}
}
