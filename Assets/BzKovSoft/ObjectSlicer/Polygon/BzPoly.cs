﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace BzKovSoft.ObjectSlicer.Polygon
{
	/// <summary>
	/// Polygon creator
	/// </summary>
	class BzPoly
	{
		LoopIndex[] _loopPoints;
		private List<int> _newTriangles;
		public List<IndexVector> outerToInnerConnections = new List<IndexVector>();
		/// <summary>
		/// False if it is impossible to create a polygon
		/// </summary>
		public bool Created { get; private set; }

		public BzPoly(BzPolyLoop outer, BzPolyLoop[] inners)
		{
			_loopPoints = CombineLoops(outer, inners);
			_newTriangles = GetTriangles();

			if (_newTriangles.Count != 0)
			{
				Created = true;
			}
		}

		private LoopIndex[] CombineLoops(BzPolyLoop outer, BzPolyLoop[] inners)
		{
			var outerData = GetIndexCollection(outer);
			var innersData = new LoopIndexCollection[inners.Length];
			for (int i = 0; i < innersData.Length; i++)
			{
				var loop = inners[i];
				innersData[i] = GetIndexCollection(loop);
			}

			var allLoops = new BzPolyLoop[inners.Length + 1];
			allLoops[0] = outer;
			Array.Copy(inners, 0, allLoops, 1, inners.Length);

			for (int i = 0; i < innersData.Length; i++)
			{
				LoopNode<LoopIndex> minOliNode = null;
				LoopNode<LoopIndex> minIliNode = null;
				float curDist = float.MaxValue;
				var innerData = innersData[i];

				var oliNode = outerData.items.first;
				for (int oli = 0; oli < outerData.items.size; oli++)
				{
					var oliData = oliNode.value;
					var iliNode = innerData.items.first;
					for (int ili = 0; ili < innerData.items.size; ili++)
					{
						var iliNodeCurr = iliNode;
						iliNode = iliNode.next;
						var iliData = iliNodeCurr.value;
						Vector2 currOVector = iliData.vector2d - oliData.vector2d;
						float dist = currOVector.sqrMagnitude;
						if (dist >= curDist)
						{
							continue;
						}
						
						if (HaveLineSegmentsIntersection(allLoops, oliData, iliData))
						{
							continue;
						}

						Vector2 center = oliData.vector2d;
						Vector2 vA = oliNode.previous.value.vector2d - center;
						Vector2 vB = iliData.vector2d - center;
						Vector2 vC = oliNode.next.value.vector2d - center;
						float angleA = Mathf.Atan2(vA.y, vA.x);
						float angleB = Mathf.Atan2(vB.y, vB.x);
						float angleC = Mathf.Atan2(vC.y, vC.x);

						float _2pi = 2 * Mathf.PI;
						
						if (angleA > angleB)
							angleB += _2pi;
						if (angleA > angleC)
							angleC += _2pi;

						if (angleB <= angleA | angleB >= angleC)
						{
							continue;
						}
						
						minOliNode = oliNode;
						minIliNode = iliNodeCurr;
						curDist = dist;
					}
					oliNode = oliNode.next;
				}

				if (minOliNode != null)
				{
					outerToInnerConnections.Add(new IndexVector(
						minOliNode.value.indexPointer.value,
						minIliNode.value.indexPointer.value)
					);
					var list = minOliNode.list;
					list.InsertAfter(minOliNode, minOliNode.value);
					list.InsertAfter(minOliNode, minIliNode.value);
					list.InsertAfter(minOliNode, minIliNode, minIliNode.previous);
				}
			}

			var result = new LoopIndex[outerData.items.size];
			var node = outerData.items.first;
			for (int i = 0; i < outerData.items.size; i++)
			{
				result[i] = node.value;
				node = node.next;
			}

			return result;
		}

		private static LoopIndexCollection GetIndexCollection(BzPolyLoop loop)
		{
			var loopData = new LinkedLoop<LoopIndex>();

			var loopNode = loop.edgeLoop.first;
			for (int i = 0; i < loop.edgeLoop.size; i++)
			{
				loopData.AddLast(new LoopIndex(loop, loopNode, loop.polyVertices2D[i]));
				loopNode = loopNode.next;
			}
			var indexCollection = new LoopIndexCollection(loop, loopData);
			return indexCollection;
		}

		private static bool HaveLineSegmentsIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
		{
			var d = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);

			if (d != 0.0f)
			{
				var u = ((b1.x - a1.x) * (b2.y - b1.y) - (b1.y - a1.y) * (b2.x - b1.x)) / d;
				var v = ((b1.x - a1.x) * (a2.y - a1.y) - (b1.y - a1.y) * (a2.x - a1.x)) / d;

				if (u >= 0.0f & u <= 1.0f & v >= 0.0f & v <= 1.0f)
				{
					return true;
				}
			}

			return false;
		}

		private static bool HaveLineSegmentsIntersection(BzPolyLoop[] loops, LoopIndex l1, LoopIndex l2)
		{
			Vector2 b1 = l1.vector2d;
			Vector2 b2 = l2.vector2d;
			int i1 = l1.indexPointer.value;
			int i2 = l2.indexPointer.value;
			for (int j = 0; j < loops.Length; j++)
			{
				var loop = loops[j];

				var edgeNode = loop.edgeLoop.first;

				Vector2 prevA = loop.polyVertices2D[loop.edgeLoop.size - 1];
				int prevI = edgeNode.previous.value;
				for (int i = 0; i < loop.edgeLoop.size; i++)
				{
					Vector2 a1 = prevA;
					Vector2 a2 = loop.polyVertices2D[i];
					prevA = a2;

					int ii1 = prevI;
					int ii2 = edgeNode.value;
					prevI = ii2;
					edgeNode = edgeNode.next;

					if (ii1 == i1 | ii1 == i2 |
						ii2 == i1 | ii2 == i2)
						continue;

					var d = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);

					if (d == 0.0f)
						continue;

					var u = ((b1.x - a1.x) * (b2.y - b1.y) - (b1.y - a1.y) * (b2.x - b1.x)) / d;
					var v = ((b1.x - a1.x) * (a2.y - a1.y) - (b1.y - a1.y) * (a2.x - a1.x)) / d;

					if (u >= 0.0f & u <= 1.0f & v >= 0.0f & v <= 1.0f)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Calculate triangle list
		/// </summary>
		/// <param name="right">Clockwise if True</param>
		/// <returns>True if polygon was created</returns>
		private List<int> GetTriangles()
		{
			if (_loopPoints.Length < 3)
				return new List<int>();

			var newTriangles = new List<int>((_loopPoints.Length - 2) * 3);

			var linkList = new LinkedLoop<int>();
			for (int i = 0; i < _loopPoints.Length; ++i)
			{
				linkList.AddLast(i);
			}

			var node = linkList.first;
			int counter = 0;
			while (linkList.size > 2 & counter <= linkList.size)
			{
				var node1 = node;
				var node2 = node1.next;
				var node3 = node2.next;

				var i1 = node1.value;
				var i2 = node2.value;
				var i3 = node3.value;

				++counter;

				bool allowed = IsAllowedToCreateTriangle(linkList, i1, i2, i3);

				if (allowed)
				{
					CreateTriangle(newTriangles, i1, i2, i3);
					node2.Remove();
					node = node3;
					counter = 0;
				}
				else
					node = node2;
			}

			return newTriangles;
		}

		private static void CreateTriangle(List<int> triangles, int i1, int i2, int i3)
		{
			triangles.Add(i1);
			triangles.Add(i2);
			triangles.Add(i3);
		}

		/// <summary>
		/// Check if triangle in right sequence and other points does not in the triangle
		/// </summary>
		/// <param name="right">Clockwise if True</param>
		private bool IsAllowedToCreateTriangle(LinkedLoop<int> linkList, int i1, int i2, int i3)
		{
			var p1 = _loopPoints[i1];
			var p2 = _loopPoints[i2];
			var p3 = _loopPoints[i3];
			Vector2 v1 = p1.vector2d;
			Vector2 v2 = p2.vector2d;
			Vector2 v3 = p3.vector2d;
			int ii1 = p1.indexPointer.value;
			int ii2 = p2.indexPointer.value;
			int ii3 = p3.indexPointer.value;

			var node = linkList.first;
			var pPrev = _loopPoints[node.previous.value];
			int counter = linkList.size;
			while (counter != 0)
			{
				--counter;

				int i = node.value;
				node = node.next;

				var p = _loopPoints[i];
				int ii = p.indexPointer.value;

				if (ii == ii1 | ii == ii2 | ii == ii3)
					continue;

				var v = p.vector2d;
				bool b1 = PointInTriangle(ref v, ref v1, ref v2, ref v3);
				if (b1)
					return false;
					
				if (ii == ii2 && HaveLineSegmentsIntersection(pPrev.vector2d, p.vector2d, v3, v1))
					return false;

				pPrev = p;
			}

			Vector3 vA = v1 - v2;
			Vector3 vB = v3 - v2;
			Vector3 vC = Vector3.Cross(vB, vA);

			return vC.z < 0.00001f;
		}

		/// <summary>
		/// It is 0 on the line, and +1 on one side, -1 on the other side.
		/// </summary>
		static float SideOfLine(ref Vector2 p, ref Vector2 a, ref Vector2 b)
		{
			return (b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x);
		}

		/// <summary>
		/// True if point resides inside a triangle
		/// </summary>
		static bool PointInTriangle(ref Vector2 pt, ref Vector2 v1, ref Vector2 v2, ref Vector2 v3)
		{
			float s1 = SideOfLine(ref pt, ref v1, ref v2);
			float s2 = SideOfLine(ref pt, ref v2, ref v3);
			float s3 = SideOfLine(ref pt, ref v3, ref v1);

			bool inside =
				(s1 >= 0 & s2 >= 0 & s3 >= 0) |
				(s1 <= 0 & s2 <= 0 & s3 <= 0);
			return inside;
		}

		/// <summary>
		/// Generete and return mesh of polygon
		/// </summary>
		public PolyMeshData GetMeshData()
		{
			if (!Created)
				throw new InvalidOperationException("You cannot get mesh if Created == False");

			LoopIndex[] newLoopIndexers;
			int[] triangles;
			OptimizeData(out newLoopIndexers, out triangles);

			PolyMeshData meshData = new PolyMeshData();

			// triangles
			meshData.triangles = triangles;

			// vertices
			meshData.vertices = new Vector3[newLoopIndexers.Length];
			for (int i = 0; i < newLoopIndexers.Length; i++)
			{
				var indexer = newLoopIndexers[i];
				Vector3 v = indexer.loop.meshData.Vertices[indexer.indexPointer.value];
				meshData.vertices[i] = v;
			}

			// normals
			meshData.normals = new Vector3[newLoopIndexers.Length];
			for (int i = 0; i < meshData.triangles.Length; i += 3)
			{
				int i1 = meshData.triangles[i + 0];
				int i2 = meshData.triangles[i + 1];
				int i3 = meshData.triangles[i + 2];

				var v1 = meshData.vertices[i1];
				var v2 = meshData.vertices[i2];
				var v3 = meshData.vertices[i3];

				var dir1 = v2 - v1;
				var dir2 = v3 - v1;
				var normal = Vector3.Cross(dir1.normalized, dir2.normalized);

				meshData.normals[i1] += normal;
				meshData.normals[i2] += normal;
				meshData.normals[i3] += normal;
			}

			// normalize normals
			for (int i = 0; i < meshData.normals.Length; i++)
			{
				var n = Normalize(meshData.normals[i]);
#if DEBUG
				// TODO: move it to tests
				if (n.sqrMagnitude == 0f)
					throw new InvalidOperationException("meshData.normals[i].sqrMagnitude == 0f");
#endif
				meshData.normals[i] = n;
			}

			// uv
			float wMax, wMin, hMax, hMin;
			var first2D = newLoopIndexers[0].vector2d;
			wMax = wMin = first2D.x;
			hMax = hMin = first2D.y;
			for (int i = 1; i < newLoopIndexers.Length; ++i)
			{
				var v = newLoopIndexers[i].vector2d;
				if (v.x > wMax) wMax = v.x;
				if (v.x < wMin) wMin = v.x;
				if (v.y > hMax) hMax = v.y;
				if (v.y < hMin) hMin = v.y;
			}
			float sizeX = wMax - wMin;
			float sizeY = hMax - hMin;
			float scale = Mathf.Max(sizeX, sizeY);

			Vector2 vMin = new Vector2(
				wMin + (sizeX - scale) / 2,
				hMin + (sizeY - scale) / 2);

			meshData.uv = new Vector2[newLoopIndexers.Length];
			for (int i = 0; i < newLoopIndexers.Length; ++i)
			{
				var v = newLoopIndexers[i].vector2d - vMin;

				meshData.uv[i] = new Vector2(v.x / scale, v.y / scale);
			}

			// bone weights
			if (newLoopIndexers[0].loop.meshData.BoneWeightsExists)
			{
				bool bwNotFound = false;
				meshData.boneWeights = new BoneWeight[newLoopIndexers.Length];
				BoneWeight bw = new BoneWeight();
				for (int i = 0; i < newLoopIndexers.Length; i++)
				{
					var indexer = newLoopIndexers[i];
					int index = indexer.indexPointer.value;
					var loopMeshData = indexer.loop.meshData;
					if (loopMeshData.BoneWeightsExists)
					{
						bw = loopMeshData.BoneWeights[index];
					}
					else
					{
						bwNotFound = true;
					}
					meshData.boneWeights[i] = bw;
				}

				if (bwNotFound)
				{
					Debug.LogError("BoneWaight not exists for some vertices of the model");
				}
			}
			else
				meshData.boneWeights = new BoneWeight[0];

			return meshData;
		}

		private Vector3 Normalize(Vector3 v)
		{
			// I do not know why, but standard v.Normalized do not work with very small values

			float magnitudeSqr = v.x * v.x + v.y * v.y + v.z * v.z;
			float magnitude = Mathf.Sqrt(magnitudeSqr);
			float f = 1f / magnitude;
			return new Vector3(v.x * f, v.y * f, v.z * f);
		}

		private void OptimizeData(out LoopIndex[] newLoopIndexers, out int[] triangles)
		{
			bool[] inUseValues = new bool[_loopPoints.Length];

			// if it is the same index, use only first one
			var length = _newTriangles.Count;
			for (int i = 0; i < length; i++)
			{
				int iindex = _newTriangles[i];
				int vertexIndex = _loopPoints[iindex].indexPointer.value;
				for (int l = i + 1; l < length; l++)
				{
					int iindex2 = _newTriangles[l];
					int vertexIndex2 = _loopPoints[iindex2].indexPointer.value;
					if (vertexIndex == vertexIndex2)
					{
						_newTriangles[l] = iindex;
					}
				}
			}

			for (int i = 0; i < length; ++i)
			{
				int vertexIndex = _newTriangles[i];
				inUseValues[vertexIndex] = true;
			}

			// count how much values in use
			int newLength = 0;
			for (int i = 0; i < inUseValues.Length; i++)
			{
				var inUse = inUseValues[i];
				if (inUse)
					++newLength;
			}

			var vertexIndexShift = new int[_loopPoints.Length];

			int index = 0;
			newLoopIndexers = new LoopIndex[newLength];
			for (int i = 0; i < _loopPoints.Length; i++)
			{
				var edgeValue = _loopPoints[i];
				var inUse = inUseValues[i];

				if (!inUse)
				{
					vertexIndexShift[i] = -1;
					continue;
				}

				vertexIndexShift[i] = index;
				newLoopIndexers[index] = edgeValue;
				++index;
			}

			triangles = new int[length];
			for (int i = 0; i < triangles.Length; i++)
			{
				var trIndex = _newTriangles[i];
				trIndex = vertexIndexShift[trIndex];
				triangles[i] = trIndex;
			}
		}
	}
}