using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPicker : Environment
{
	[SerializeField] CameraLerper cameraLerper;
	[SerializeField] AudioSource camAudio;
	[SerializeField] CameraFrameController cameraFrame;
	[SerializeField] Transform collectionContainer;
	[SerializeField] PartAsset[] partAssets;
	[SerializeField] Animator animator;
	[SerializeField] float mapSize = 40;
	[SerializeField] float mapPadding = 3;
	[SerializeField] float zoomDuration = 0.8f;
	protected override void OnEnabled(){
		collectionSelected = false;
		cameraFrame.enabled = true;
		cameraLerper.Reset();
		int amount = 30 + GameManager.instance.cycleIndex*10;
		amount = Mathf.Min(amount, 50);
		GenerateParts(amount);
	}
	protected override void OnDisabled(){
		cameraFrame.enabled = false;
		collectionSelected = true;
		// cameraLerper.enabled = false;
	}
	void ClearCollections(){
		foreach (Transform child in collectionContainer) {
			Destroy(child.gameObject);
		}
	}
	void GenerateParts(int amount){
		ClearCollections();
		List<PartAsset> availableParts = new List<PartAsset>(partAssets);
		Vector2[] spawnPoints = GenerateSpawnPoints(amount);
		foreach(Vector2 point in spawnPoints){
			int partIndex = Random.Range(0,availableParts.Count);
			PartAsset asset = availableParts[partIndex];
			availableParts.RemoveAt(partIndex);
			Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
			Transform part = Instantiate(asset.previewObject, point, rotation, collectionContainer);
			part.gameObject.AddComponent<PartPreview>().asset = asset;
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
		float aspectRatio = WorldCamera.GetCamera().aspect;
		for(int i = 0; i< iterations; i++){
			float minLocalDistance = Mathf.Infinity;
			float randX = Random.Range(-(mapSize*aspectRatio)/2+mapPadding, (mapSize*aspectRatio)/2 - mapPadding);
			float randY = Random.Range(-mapSize/2+mapPadding, mapSize/2 - mapPadding);
			Vector2 pos = (Vector2)transform.position + new Vector2(randX,randY);
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

	bool collectionSelected = false;
	void Update(){
		if(!active){return;}
		if(collectionSelected){return;}
		if (Input.GetMouseButtonDown(0))
		{
			if(cameraFrame.GetPartsInFrame().Length > 0){
				StartCoroutine(SelectCollection());
			}
		}
	}
	IEnumerator SelectCollection(){
		collectionSelected = true;
		cameraFrame.enabled = false;
		cameraLerper.ZoomTowards(cameraFrame.transform, cameraFrame.GetSize()/2, zoomDuration);
		yield return new WaitForSeconds(zoomDuration-0.15f);
		camAudio.Play();
		yield return new WaitForSeconds(0.15f);
		animator.Play("Blink");
		yield return new WaitForSeconds(0.2f);
		GameManager.instance.SwitchToEditor(cameraFrame.GetPartsInFrame(), cameraLerper.transform.localPosition,cameraFrame.GetSize()/2);
	}

}