using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollection : MonoBehaviour
{
	[SerializeField] public Part[] parts;
	[SerializeField] float radius;
	[SerializeField] float minDistance;
	public void GenerateParts(List<PartAsset> availableParts){
		Vector2[] spawnPoints = GenerateSpawnPoints(5);
		List<Part> createdParts = new List<Part>();
		foreach(Vector2 point in spawnPoints){
			int partIndex = Random.Range(0,availableParts.Count);
			PartAsset asset = availableParts[partIndex];
			availableParts.RemoveAt(partIndex);
			Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
			createdParts.Add(new Part(asset, Instantiate(asset.previewObject, point, rotation, transform)));
		}
		
	}
	Vector2[] GenerateSpawnPoints(int amount){
		List<Vector2> points = new List<Vector2>();
		for(int i = 0; i < amount; i++){
			points.Add(GenerateSpawnPoint(10,points));
		}
		return points.ToArray();
	}
	Vector2 GenerateSpawnPoint(int iterations, List<Vector2> occupiedPositions){
		float maxGlobalDistance = 0;
		Vector2 maxGlobalDistancePoint = Vector2.zero;
		for(int i = 0; i< iterations; i++){
			float minLocalDistance = Mathf.Infinity;
			Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle*radius;
			foreach(Vector2 point in occupiedPositions){
				float pointDistance = (pos-point).magnitude;
				if(pointDistance < minLocalDistance){
					minLocalDistance = pointDistance;
				}
			}
			if(minLocalDistance > maxGlobalDistance){
				maxGlobalDistance = minLocalDistance;
				maxGlobalDistancePoint = pos;
			}
		}
		return maxGlobalDistancePoint;

	}
	[System.Serializable]
	public class Part
	{
		public Part(PartAsset _asset, Transform _element){
			asset = _asset;
			element = _element;
		}
		public PartAsset asset;
		public Transform element;
	}
}
