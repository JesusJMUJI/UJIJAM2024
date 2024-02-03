using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPicker : Environment
{
	PartCollection[] collections;
	[SerializeField] PartCollection collectionPrefab;
	[SerializeField] CameraLerper cameraLerper;
	[SerializeField] Transform collectionContainer;
	protected override void OnEnabled(){
		//reset camera
		// cameraLerper.enabled = true;
		selectedCollection = null;
		GenerateCollections(10);
	}
	protected override void OnDisabled(){
		// cameraLerper.enabled = false;
	}
	void ClearCollections(){
		foreach (Transform child in collectionContainer) {
			Destroy(child.gameObject);
		}
	}
	void GenerateCollections(float minDistance){
		ClearCollections();
		List<PartCollection> collections = new List<PartCollection>();
		int attempts = 0;
		while(attempts < 100){
			Vector3 pos = transform.position + (Vector3)new Vector2(Random.Range(-mapSize/2+mapPadding, mapSize/2 - mapPadding),Random.Range(-mapSize/2+mapPadding, mapSize/2 - mapPadding));
			bool inRange = false;
			foreach(PartCollection collection in collections){
				if((pos-collection.transform.position).magnitude < minDistance){
					inRange = true;
					break;
				}
			}
			if(!inRange){
				collections.Add(Instantiate(collectionPrefab, pos, Quaternion.identity, collectionContainer));
				attempts = 0;
			}
			else{
				attempts++;
			}

		}
	}
	[SerializeField] float mapSize = 40;
	[SerializeField] float mapPadding = 3;
	PartCollection selectedCollection;
	[SerializeField] float zoomSize = 5;
	[SerializeField] float zoomDuration = 0.8f;
	void Update(){
		if(!active){return;}
		if(selectedCollection){return;}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(WorldCamera.GetWorldMousePos(), -Vector2.up);
			if(hit.collider != null){
				selectedCollection = hit.collider.GetComponent<PartCollection>();
				if(selectedCollection != null){
					StartCoroutine(SelectCollection());
				}
			}
		}
	}
	IEnumerator SelectCollection(){
		cameraLerper.ZoomTowards(selectedCollection.transform, zoomSize, zoomDuration);
		yield return new WaitForSeconds(zoomDuration);
		GameManager.instance.SwitchToEditor(selectedCollection);
	}
}
