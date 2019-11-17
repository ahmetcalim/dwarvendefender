using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using BzKovSoft.CharacterSlicer;
using BzKovSoft.ObjectSlicer.Polygon;

namespace BzKovSoft.CharacterSlicerSamples
{
	/// <summary>
	/// Example of implementation character sliceable object
	/// </summary>
	public class CharacterSlicerSample : BzSliceableCharacterBase, IDeadable
	{
#pragma warning disable 0649
		[SerializeField]
		GameObject _bloodPrefub;
		[SerializeField]
		Transform _characterRoot;
		[SerializeField]
		Vector3 _prefubDirection;
#pragma warning restore 0649

		public bool IsDead { get; private set; }

		protected override BzSliceTryData PrepareData(Plane plane)
		{
			if (_characterRoot == null)
				throw new InvalidOperationException("Character Root must be defined");

			// remember some date. Later we could use it after the slice is done.
			// here I add Stopwatch object to see how much time it takes
			ResultData addData = new ResultData();
			addData.stopwatch = Stopwatch.StartNew();

			// collider we want to participate in slicing
			var collidersArr = GetComponentsInChildren<Collider>();

			// create component manager.
			var componentManager = new CharacterComponentManager(this.gameObject, _characterRoot, plane, collidersArr);
			addData.componentManager = componentManager;  // I need to save it to fix root fields after slice

			return new BzSliceTryData()
			{
				componentManager = componentManager,
				plane = plane,
				addData = addData,
			};
		}

		protected override void OnSliceFinishedWorkerThread(bool sliced, object addData)
		{
			((ResultData)addData).stopwatch.Stop();
		}

		protected override void OnSliceFinished(BzSliceTryResult result)
		{
			if (!result.sliced)
				return;

			var addData = (ResultData)result.addData;

			// add blood
			AddBlood(result);

			// convert to ragdoll

			// set 'root' fields
			var charCompNeg = result.outObjectNeg.GetComponent<CharacterSlicerSample>();
			var charCompPos = result.outObjectPos.GetComponent<CharacterSlicerSample>();
			charCompNeg._characterRoot = addData.componentManager.RootNeg;
			charCompPos._characterRoot = addData.componentManager.RootPos;
			charCompNeg._bloodPrefub = null;
			charCompPos._bloodPrefub = null;

			// show elapsed time
			drawText += addData.stopwatch.ElapsedMilliseconds.ToString() + " - " + gameObject.name + Environment.NewLine;
			if (drawText.Length > 1500) // prevent very long text
				drawText = drawText.Substring(drawText.Length - 1000, 1000);

			IsDead = true;
		}

		private void ConvertToRagdoll(BzSliceTryResult result)
		{
			Vector3 velocityContinue = Vector3.zero;
			Vector3 angularVelocityContinue = Vector3.zero;

			Animator animator = this.GetComponent<Animator>();
			if (animator != null)
			{
				velocityContinue = animator.velocity;
				angularVelocityContinue = animator.angularVelocity;
			}

			ConvertToRagdoll(result.outObjectPos, velocityContinue, angularVelocityContinue);
			ConvertToRagdoll(result.outObjectNeg, velocityContinue, angularVelocityContinue);
		}

		private void AddBlood(BzSliceTryResult result)
		{
			if (_bloodPrefub == null)
				return;

			for (int i = 0; i < result.meshItems.Length; i++)
			{
				var meshItem = result.meshItems[i];

				if (meshItem == null)
					continue;

				for (int j = 0; j < meshItem.sliceEdgesNeg.Length; j++)
				{
					var meshData = meshItem.sliceEdgesNeg[j].capsData;
					SetBleedingObjects(meshData, meshItem.rendererNeg);
				}

				for (int j = 0; j < meshItem.sliceEdgesPos.Length; j++)
				{
					var meshData = meshItem.sliceEdgesPos[j].capsData;
					SetBleedingObjects(meshData, meshItem.rendererPos);
				}
			}
		}

		private void SetBleedingObjects(PolyMeshData meshData, Renderer renderer)
		{
			var meshRenderer = renderer as MeshRenderer;
			if (meshRenderer != null)
			{
				// add blood object
				Vector3 position = AVG(meshData.vertices);
				Vector3 direction = AVG(meshData.normals).normalized;
				var rotation = Quaternion.FromToRotation(_prefubDirection, direction);
				var blood = Instantiate(_bloodPrefub, renderer.gameObject.transform);

				blood.transform.localPosition = position;
				blood.transform.localRotation = rotation;

				return;
			}

			var skinnedRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedRenderer != null)
			{
				var bones = skinnedRenderer.bones;
				float[] weightSums = new float[bones.Length];
				for (int i = 0; i < meshData.boneWeights.Length; i++)
				{
					var w = meshData.boneWeights[i];
					weightSums[w.boneIndex0] += w.weight0;
					weightSums[w.boneIndex1] += w.weight1;
					weightSums[w.boneIndex2] += w.weight2;
					weightSums[w.boneIndex3] += w.weight3;
				}

				// detect most weightful bone for this PolyMeshData
				int maxIndex = 0;
				for (int i = 0; i < weightSums.Length; i++)
				{
					float maxValue = weightSums[maxIndex];
					float current = weightSums[i];

					if (current > maxValue)
						maxIndex = i;
				}
				Transform bone = bones[maxIndex];

				// add blood object to the bone
				Vector3 position = AVG(meshData.vertices);
				Vector3 normal = AVG(meshData.normals).normalized;
				var rotation = Quaternion.FromToRotation(_prefubDirection, normal);

				var m = skinnedRenderer.sharedMesh.bindposes[maxIndex];
				position = m.MultiplyPoint3x4(position);

				var blood = Instantiate(_bloodPrefub, bone);
				blood.transform.localPosition = position;
				blood.transform.localRotation = rotation;

				return;
			}

			throw new InvalidOperationException();
		}

		private static Vector3 AVG(Vector3[] vertices)
		{
			Vector3 result = Vector3.zero;

			for (int i = 0; i < vertices.Length; i++)
			{
				result += vertices[i];
			}

			return result / vertices.Length;
		}

		private void ConvertToRagdoll(GameObject go, Vector3 velocityContinue, Vector3 angularVelocityContinue)
		{
			// if your player is dead, you do not need animator or collision collider
			Animator animator = go.GetComponent<Animator>();
			Collider triggerCollider = go.GetComponent<Collider>();

			if (animator != null)
				UnityEngine.Object.Destroy(animator);

			if (triggerCollider != null)
				UnityEngine.Object.Destroy(triggerCollider);

			StartCoroutine(SmoothDepenetration(go, velocityContinue, angularVelocityContinue));

			// switch all triggers ON.
			var collidersArr = go.GetComponentsInChildren<Collider>();
			for (int i = 0; i < collidersArr.Length; i++)
			{
				var collider = collidersArr[i];
				if (collider == null)
					continue;

				collider.isTrigger = false;
			}

			// set rigid bodies as non kinematic
			var rigidsArr = go.GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < rigidsArr.Length; i++)
			{
				var rigid = rigidsArr[i];
				rigid.isKinematic = false;
			}
		}

		static IEnumerator SmoothDepenetration(GameObject go, Vector3 velocityContinue, Vector3 angularVelocityContinue)
		{
			var rigids = go.GetComponentsInChildren<Rigidbody>();
			var maxVelocitys = new float[rigids.Length];
			for (int i = 0; i < rigids.Length; i++)
			{
				var rigid = rigids[i];
				maxVelocitys[i] = rigid.maxDepenetrationVelocity;

				rigid.velocity = velocityContinue;
				rigid.angularVelocity = angularVelocityContinue;
			}

			const float duration = 2f;
			float t = 0f;
			do
			{
				t += Time.deltaTime;
				float r = duration;

				for (int i = 0; i < rigids.Length; i++)
				{
					var rigid = rigids[i];
					if (rigid == null)
						continue;

					float maxVel = maxVelocitys[i];
					rigid.maxDepenetrationVelocity = Mathf.Lerp(0.0f, maxVel, r);
				}
				yield return null;
			}
			while (t < duration);


			for (int i = 0; i < rigids.Length; i++)
			{
				var rigid = rigids[i];
				if (rigid == null)
					continue;

				float maxVel = maxVelocitys[i];
				rigid.maxDepenetrationVelocity = maxVel;
			}
		}

		static string drawText = string.Empty;

		void OnGUI()
		{
			GUI.Label(new Rect(10, 10, 2000, 2000), drawText);
		}

		// Sample of data that can be attached to slice request.
		// In this the Stopwatch is used to time duration of slice operation.
		class ResultData
		{
			public Stopwatch stopwatch;
			public CharacterComponentManager componentManager;
		}
	}
}