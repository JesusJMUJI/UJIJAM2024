using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int cycleIndex = -1;
	public static GameManager instance;
	void Awake(){
		if(instance != null){
			Debug.LogError("Multiple GameManagers active!");
		}
		instance = this;
	}

	[SerializeField] AudioSource battleMusic;
	[SerializeField] AudioSource menuMisic;
	[SerializeField] CollectionPicker collectionPicker;
	[SerializeField] CreatureEditor editor;
	[SerializeField] BattleManager battleManager;
	public void SwitchToEditor(PartPreview[] selectedParts, Vector2 relativePosition, float frameZoom){
		collectionPicker.Disable();
		editor.AssignCollection(selectedParts, relativePosition,frameZoom);
		editor.Enable();
	}
	public void SwitchToCollectionPicker(){
		cycleIndex++;
		battleManager.Disable();
		collectionPicker.Enable();

		battleMusic.enabled = false;
		menuMisic.enabled = true;
		menuMisic.Play();

	}
	public void SwitchToBattle(Creature playerCreature){
		editor.Disable();
		battleManager.SetPlayerCreature(playerCreature);
		battleManager.Enable();
		menuMisic.enabled = false;
		battleMusic.enabled = true;
		battleMusic.Play();
	}
	void Start(){
		SwitchToCollectionPicker();
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			LevelManager.instance.LoadScene(0);
		}
	}
}