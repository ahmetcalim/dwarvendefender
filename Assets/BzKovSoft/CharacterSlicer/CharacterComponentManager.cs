using BzKovSoft.ObjectSlicer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BzKovSoft.CharacterSlicer
{
	class CharacterComponentManager : StaticComponentManager, IComponentManager
	{
		Transform _playerRoot;
		KeyValuePair<Transform, bool>[] _trSide;

		public Transform RootNeg;
		public Transform RootPos;

		public CharacterComponentManager(GameObject go, Transform playerRoot, Plane plane, Collider[] colliders)
			: base(go, plane, colliders)
		{
			_playerRoot = playerRoot;

			var transforms = go.GetComponentsInChildren<Transform>();
			_trSide = new KeyValuePair<Transform, bool>[transforms.Length];
			for (int i = 0; i < transforms.Length; i++)
			{
				var tr = transforms[i];
				bool side = plane.GetSide(tr.position);

				_trSide[i] = new KeyValuePair<Transform, bool>(tr, side);
			}
		}

		Item[] _items;

		class Item
		{
			public List<ConnectedMesh> connectionsNeg;
			public List<ConnectedMesh> connectionsPos;
			public SliceTryItem item;
		}

		public new void OnSlicedWorkerThread(SliceTryItem[] items)
		{
			base.OnSlicedWorkerThread(items);

			_items = new Item[items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				var tryItem = items[i];
				if (tryItem == null || !(tryItem.meshRenderer is SkinnedMeshRenderer))
				{
					continue;
				}

				Item item = new Item();
				item.item = tryItem;
				item.connectionsNeg = ConnectedMesh.GetConnections(tryItem.meshDissector.SliceResultNeg);
				item.connectionsPos = ConnectedMesh.GetConnections(tryItem.meshDissector.SliceResultPos);

				_items[i] = item;
			}
		}

		public new void OnSlicedMainThread(GameObject resultObjNeg, GameObject resultObjPos, Renderer[] renderersNeg, Renderer[] renderersPos)
		{
			var cldrsNeg = new List<Collider>();
			var cldrsPos = new List<Collider>();

			var playerRootDupl = BzSlicerHelper.GetSameComponentForDuplicate(_playerRoot, resultObjNeg, resultObjPos);
			var duplSide = new Dictionary<Transform, bool>(_trSide.Length);
			foreach (var tr in _trSide)
			{
				var duplTr = BzSlicerHelper.GetSameComponentForDuplicate(tr.Key, resultObjNeg, resultObjPos);
				duplSide.Add(duplTr, !tr.Value);
			}

			RepairColliders(resultObjNeg, resultObjPos, cldrsNeg, cldrsPos);

			var connectionsNeg = new List<HashSet<Transform>>(16);
			var connectionsPos = new List<HashSet<Transform>>(16);
			for (int i = 0; i < _items.Length; i++)
			{
				Item item = _items[i];
				if (item == null)
					continue;

				var bonesNeg = ((SkinnedMeshRenderer)renderersNeg[i]).bones;
				var bonesPos = ((SkinnedMeshRenderer)renderersPos[i]).bones;

				AddConnectedBones(item.connectionsNeg, connectionsNeg, bonesNeg);
				AddConnectedBones(item.connectionsPos, connectionsPos, bonesPos);
			}

			var sidesNeg = new Dictionary<Transform, bool>(_trSide.Length);
			for (int i = 0; i < _trSide.Length; i++)
			{
				var side = _trSide[i];
				bool value = side.Value;

				if (value)
				{
					// TODO: combine connections to one collection
					for (int cb = 0; cb < connectionsNeg.Count; cb++)
					{
						var connectedMesh = connectionsNeg[cb];
						if (connectedMesh.Contains(side.Key))
						{
							// if it has a mesh connections, consider it as is on the mesh side
							value = false;
							break;
						}
					}
				}

				sidesNeg.Add(side.Key, value);
			}

			RootNeg = OnCompletePerSide(cldrsNeg, resultObjNeg, connectionsNeg, _playerRoot, sidesNeg);
			RootPos = OnCompletePerSide(cldrsPos, resultObjPos, connectionsPos, playerRootDupl, duplSide);
		}

		private static Transform OnCompletePerSide(List<Collider> colliders, GameObject go,
			List<HashSet<Transform>> connections, Transform playerRoot, Dictionary<Transform, bool> boneSide)
		{
			var cldrsTest = new HashSet<Collider>(colliders);
			var parts = SplitParts(go.transform, cldrsTest, playerRoot, boneSide);
			var partArr = new Transform[parts.childCount];
			for (int i = 0; i < parts.childCount; i++)
			{
				var part = parts.GetChild(i);
				partArr[i] = part;
			}

			RemoveLostRigids(partArr, cldrsTest);

			var partList = new KeyValuePair<Transform, Transform[]>[partArr.Length];

			for (int i = 0; i < partArr.Length; i++)
			{
				var part = partArr[i];
				if (part == null)
					continue;

				var items = part.GetComponentsInChildren<Transform>();
				partList[i] = new KeyValuePair<Transform, Transform[]>(part, items);
			}

			// find part connections by mesh
			var partMeshConnections = new List<KeyValuePair<int, int>>();

			for (int i = 1; i < partList.Length; i++)
			{
				var partA = partList[i];
				if (partA.Key == null)
					continue;

				for (int j = 0; j < i; j++)
				{
					var partB = partList[j];
					if (partB.Key == null)
						continue;

					var connected = HaveMeshConnections(connections, partA.Value, partB.Value);
					if (connected)
						partMeshConnections.Add(new KeyValuePair<int, int>(i, j));
				}
			}

			// find joints
			var currentJoints = new List<CurrentJointData>();
			for (int i = 0; i < partList.Length; i++)
			{
				var part = partList[i];
				if (part.Key == null)
					continue;

				Joint[] joints = part.Key.GetComponentsInChildren<Joint>();

				for (int j = 0; j < joints.Length; j++)
				{
					Joint joint = joints[j];
					var connectedBody = joint.connectedBody;
					if (connectedBody == null)
					{
						continue;
					}

					bool conndectedBodyFound = false;
					for (int b = 0; b < partList.Length; b++)
					{
						var partItem = partList[b];
						if (partItem.Key == null)
							continue;

						if (partItem.Value.Contains(connectedBody.transform))
						{
							if (i != b)
							{
								currentJoints.Add(new CurrentJointData(joint, i, b));
							}
							conndectedBodyFound = true;
							break;
						}
					}

					if (!conndectedBodyFound)
					{
						UnityEngine.Object.Destroy(joint);
					}
				}
			}

			// remove joints
			for (int i = 0; i < currentJoints.Count; i++)
			{
				var joint = currentJoints[i];

				bool deleteJoint = true;

				for (int j = 0; j < partMeshConnections.Count; j++)
				{
					var partMeshConnection = partMeshConnections[i];

					if ((joint.a == partMeshConnection.Key &
						 joint.b == partMeshConnection.Value) |
						(joint.a == partMeshConnection.Value &
						 joint.b == partMeshConnection.Key))
					{
						deleteJoint = false;
						break;
					}
				}

				if (deleteJoint)
				{
					UnityEngine.Object.Destroy(joint.joint);
					currentJoints.RemoveAt(i);
					--i;
				}
			}
			
			// add missing joints
			for (int i = 0; i < partMeshConnections.Count; i++)
			{
				var partMeshConnection = partMeshConnections[i];
				var partA = partMeshConnection.Key;
				var partB = partMeshConnection.Value;

				bool jointExists = false;

				for (int j = 0; j < currentJoints.Count; j++)
				{
					var joint = currentJoints[j];

					if ((joint.a == partMeshConnection.Key &
						 joint.b == partMeshConnection.Value) |
						(joint.a == partMeshConnection.Value &
						 joint.b == partMeshConnection.Key))
					{
						jointExists = true;
						break;
					}
				}

				if (!jointExists)
				{
					CreateJoint(partList[partA].Key, partList[partB].Key);
				}
			}

			return parts;
		}

		private static void RemoveLostRigids(Transform[] parts, HashSet<Collider> cldrsTest)
		{
			for (int i = 0; i < parts.Length; i++)
			{
				var part = parts[i];
				var partColliders = part.GetComponentsInChildren<Collider>();

				bool haveCollider = false;

				for (int j = 0; j < partColliders.Length; j++)
				{
					var partCollider = partColliders[j];
					if (cldrsTest.Contains(partCollider))
					{
						haveCollider = true;
						break;
					}
				}

				if (haveCollider)
					continue;

				UnityEngine.Object.Destroy(part.gameObject);
				parts[i] = null;
				break;
			}
		}

		private static void AddConnectedBones(List<ConnectedMesh> connections, List<HashSet<Transform>> result, Transform[] bones)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				var connection = connections[i];
				HashSet<Transform> hashSet = new HashSet<Transform>();
				result.Add(hashSet);

				foreach (var boneIndex in connection.Bones)
				{
					var bone = bones[boneIndex];
					hashSet.Add(bone);
				}
			}
		}

		private static bool HaveMeshConnections(List<HashSet<Transform>> connectedBones, Transform[] a, Transform[] b)
		{
			for (int m = 0; m < a.Length; m++)
			{
				var aItem = a[m];
				for (int n = 0; n < b.Length; n++)
				{
					var bItem = b[n];

					for (int cb = 0; cb < connectedBones.Count; cb++)
					{
						var connectedMesh = connectedBones[cb];
						if (connectedMesh.Contains(aItem) && connectedMesh.Contains(bItem))
							return true;
					}
				}
			}

			return false;
		}

		private static Transform SplitParts(Transform goTransform, HashSet<Collider> cldrsTest, Transform root, Dictionary<Transform, bool> boneSide)
		{
			var parts = new GameObject("parts").transform;
			parts.SetParent(goTransform, false);

			bool side = GetSide(root, cldrsTest, boneSide);
			FindRoots(parts, cldrsTest, root, boneSide, side);
			root.parent = parts;
			root.name += "_" + side.ToString();

			return parts;
		}

		private static bool GetSide(Transform transform, HashSet<Collider> cldrsTest, Dictionary<Transform, bool> boneSide)
		{
			bool side = boneSide[transform];

			if (side)
			{
				var haveCollider = FindColliderOnRigid(transform, cldrsTest) != null;
				if (haveCollider)
					return false;
			}

			return side;
		}

		private static Collider FindColliderOnRigid(Transform transform, HashSet<Collider> cldrsTest)
		{
			var colliders = transform.GetComponents<Collider>();
			for (int i = 0; i < colliders.Length; i++)
			{
				var collider = colliders[i];
				if (cldrsTest.Contains(collider))
					return collider;
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				var child = transform.GetChild(i);

				var rigid = child.GetComponent<Rigidbody>();
				if (rigid != null)
					return null;

				var collider = FindColliderOnRigid(child, cldrsTest);
				if (collider != null)
					return collider;
			}

			return null;
		}

		private static void FindRoots(Transform root, HashSet<Collider> cldrsTest, Transform transform, Dictionary<Transform, bool> boneSide, bool rootSide)
		{
			bool side = GetSide(transform, cldrsTest, boneSide);
			if (rootSide != side)
			{
				bool haveRigid = transform.GetComponent<Rigidbody>() != null;

				if (!haveRigid)
				{
					side = rootSide;
				}
			}

			var children = new Transform[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				children[i] = transform.GetChild(i);
			}

			for (int i = 0; i < children.Length; i++)
			{
				var child = children[i];
				FindRoots(root, cldrsTest, child, boneSide, side);
			}

			if (rootSide != side)
			{
				transform.SetParent(root, true);
			}
		}

		private static void CreateJoint(Transform itemA, Transform itemB)
		{
			var joint = itemA.gameObject.AddComponent<FixedJoint>();
			var rigid = itemB.GetComponent<Rigidbody>();

			if (rigid == null)
				throw new InvalidOperationException();

			joint.connectedBody = rigid;
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = new Vector3();
			
			var dist = itemA.position - itemB.position;
			joint.anchor = itemA.InverseTransformDirection(-dist);
		}

		class ConnectedMesh
		{
			private HashSet<int> _bones;

			public HashSet<int> Indexes { get; private set; }
			public HashSet<int> Bones { get { return _bones; } }

			public static List<ConnectedMesh> GetConnections(BzMeshData meshData)
			{
				List<HashSet<int>> sets = new List<HashSet<int>>(128);

				// join indexes by adjacent indexes
				for (int smIndex = 0; smIndex < meshData.SubMeshes.Length; smIndex++)
				{
					int[] subMesh = meshData.SubMeshes[smIndex];

					for (int i = 0; i < subMesh.Length; i += 3)
					{
						int i1 = subMesh[i + 0];
						int i2 = subMesh[i + 1];
						int i3 = subMesh[i + 2];

						int prevMatchedSet = -1;
						for (int setIndex = 0; setIndex < sets.Count; setIndex++)
						{
							var set = sets[setIndex];

							if (set == null)
								continue;

							bool i1Contains = set.Contains(i1);
							bool i2Contains = set.Contains(i2);
							bool i3Contains = set.Contains(i3);

							if (i1Contains | i2Contains | i3Contains)
							{
								if (!i1Contains)
									set.Add(i1);
								if (!i2Contains)
									set.Add(i2);
								if (!i3Contains)
									set.Add(i3);

								if (prevMatchedSet != -1)
								{
									var prevSet = sets[prevMatchedSet];
									sets[prevMatchedSet] = null;

									set.UnionWith(prevSet);
								}
								prevMatchedSet = setIndex;
							}
						}

						if (prevMatchedSet == -1)
						{
							var set = new HashSet<int>();
							sets.Add(set);
							set.Add(i1);
							set.Add(i2);
							set.Add(i3);
						}
					}
				}

				// join indexes by adjacent bones
				List<ConnectedMesh> connections = new List<ConnectedMesh>();
				for (int i = 0; i < sets.Count; i++)
				{
					var set = sets[i];

					if (set == null)
						continue;

					var newConn = new ConnectedMesh(set, meshData);

					int prevMatchedConn = -1;
					for (int conIndex = 0; conIndex < connections.Count; conIndex++)
					{
						var conn = connections[conIndex];

						if (conn == null)
							continue;

						bool connected = conn.IsBonesConnected(newConn);

						if (connected)
						{
							conn.Add(newConn);

							if (prevMatchedConn != -1)
							{
								var prevConn = connections[prevMatchedConn];
								sets[prevMatchedConn] = null;

								conn.Add(prevConn);
							}
							prevMatchedConn = conIndex;
						}
					}

					if (prevMatchedConn == -1)
					{
						connections.Add(newConn);
					}
				}

				return connections;
			}

			private bool IsBonesConnected(ConnectedMesh another)
			{
				foreach (var conBone in another.Bones)
				{
					if (Bones.Contains(conBone))
					{
						return true;
					}
				}

				return false;
			}

			private ConnectedMesh(HashSet<int> set, BzMeshData meshData)
			{
				Indexes = set;
				_bones = new HashSet<int>();
				foreach (var index in set)
				{
					var boneWeight = meshData.BoneWeights[index];

					if (boneWeight.weight0 > 0f)
						_bones.Add(boneWeight.boneIndex0);

					if (boneWeight.weight1 > 0f)
						_bones.Add(boneWeight.boneIndex1);

					if (boneWeight.weight2 > 0f)
						_bones.Add(boneWeight.boneIndex2);

					if (boneWeight.weight3 > 0f)
						_bones.Add(boneWeight.boneIndex3);
				}
			}

			public void Add(ConnectedMesh other)
			{
				Indexes.UnionWith(other.Indexes);
				_bones.UnionWith(other._bones);

				other.Indexes.Clear();
				other._bones.Clear();
			}
		}

		struct CurrentJointData
		{
			public Joint joint;
			public int a;
			public int b;

			public CurrentJointData(Joint joint, int a, int b)
			{
				this.joint = joint;
				this.a = a;
				this.b = b;
			}
		}
	}
}