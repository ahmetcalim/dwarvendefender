using UnityEngine;

public class Stuff : PooledObject {

	public Rigidbody Body { get; private set; }

	MeshRenderer[] meshRenderers;

	public void SetMaterial (Material m) {
		for (int i = 0; i < meshRenderers.Length; i++) {
			meshRenderers[i].material = m;
		}
	}

	void Awake () {
		Body = GetComponent<Rigidbody>();
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}

	void ReturnForPooing()
    {
        ReturnToPool();
	}

	void OnLevelWasLoaded () {
		ReturnToPool();
	}
}