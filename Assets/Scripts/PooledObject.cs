using UnityEngine;

public class PooledObject : MonoBehaviour {

	[System.NonSerialized]
	ObjectPool poolInstanceForPrefab;

	public T GetPooledInstance<T> () where T : PooledObject {
       
            if (!poolInstanceForPrefab)
            {
                poolInstanceForPrefab = ObjectPool.GetPool(this);
            }
            return (T)poolInstanceForPrefab.GetObject();

	}

	public ObjectPool Pool { get; set; }

	public void ReturnToPool () {
		if (Pool) {
            if (GetComponent<Mob>())
            {
                
                if (!GetComponent<Mob>().sliced)
                {
                    Pool.AddObject(this);
                }
                else
                {
                    Destroy(gameObject, 3);
                }
            }
			
		}
		else {
			Debug.Log("I die!");
			Destroy(gameObject);
		}
	}
}