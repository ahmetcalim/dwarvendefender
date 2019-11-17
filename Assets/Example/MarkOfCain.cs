using UnityEngine;
using System.Collections.Generic;

namespace NobleMuffins.LimbHacker.Examples
{
	public class MarkOfCain : MonoBehaviour
	{
        public List<GameObject> yarraks;
		private readonly static List<GameObject> markedObjects = new List<GameObject>();

		void Start()
        {
          
			markedObjects.Add(gameObject);
            Destroy(gameObject, 5f);
            foreach (var item in GetComponentsInChildren<Rigidbody>())
            {
                item.velocity = Vector3.zero;
                item.angularVelocity = Vector3.zero;
            }
		}

		public static void DestroyAllMarkedObjects()
		{
			foreach (var go in markedObjects)
			{
				if (go != null) Destroy(go);
			}

			markedObjects.Clear();
		}
         void LateUpdate()
        {
            foreach (var item in yarraks)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);

                }
              
            }
        }
    }
}
