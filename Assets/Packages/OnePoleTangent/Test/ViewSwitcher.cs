using UnityEngine;
using System.Collections;

public class ViewSwitcher : MonoBehaviour {
	public float interval = 5f;

	void Start () {
		StartCoroutine(Switcher());
	}

	IEnumerator Switcher() {
		var rotEnd = transform.localRotation;
		var rotStart = rotEnd;
		var t = 1f;
		var dt = 1f / interval;
		while (true) {
			yield return null;
			if (t >= 1f) {
				t = 0f;
				rotStart = rotEnd;
				rotEnd = Random.rotationUniform;
			}
			t += Time.deltaTime * dt;
			transform.localRotation = Quaternion.Slerp(rotStart, rotEnd, t);
		}
	}
}
