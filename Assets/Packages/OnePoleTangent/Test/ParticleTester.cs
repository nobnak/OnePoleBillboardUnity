using UnityEngine;
using System.Collections;

public class ParticleTester : MonoBehaviour {
	public GameObject partiflefab;
	public int nParticles = 10;
	public float distance = 10f;
	public Vector3 rotation = new Vector3(30f, 10f, 0f);

	private GameObject[] _particles;

	void Start () {
		for (var i = 0; i < nParticles; i++) {
			var pt = (GameObject)Instantiate(partiflefab);
			pt.transform.localPosition = distance * Random.onUnitSphere;
			pt.transform.SetParent(transform, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation *= Quaternion.Euler(rotation * Time.deltaTime);
	}
}
