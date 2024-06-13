using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : Environment
{
	[SerializeField] Animator animator;
    [SerializeField] float creatureAttraction = 12f;
    [SerializeField] TextWriter text;
    [SerializeField] Image spaceImage;
    [SerializeField] Vector2 imageAlphaRange;
	protected override void OnEnabled(){
		cameraController.enabled = true;
		LoadEnemy();
		battleOver = false;
		//TODO reset camera
	}
	protected override void OnDisabled(){
		cameraController.enabled = false;
		if(enemyCreature){
			Destroy(enemyCreature.gameObject);
		}
		if(playerCreature){
			Destroy(playerCreature.gameObject);
		}
	}
	[SerializeField] CameraController cameraController;
	// [SerializeField] Transform battleContainer;
	[SerializeField] EnemyTier[] enemyTiers;
	[SerializeField] Transform playerSpawn;
	[SerializeField] Transform enemySpawn;
	Creature enemyCreature;
	Creature playerCreature;
	public void SetPlayerCreature(Creature _playerCreature){
		playerCreature = _playerCreature;
		playerCreature.transform.position = playerSpawn.position;
		TrackCreature(playerCreature);
	}
	void TrackCreature(Creature creature){
		CreaturePart[] parts = creature.GetParts();
		foreach(CreaturePart part in parts){
			cameraController.trackedObjects.Add(part.transform);
		}
	}
	void LoadEnemy(){
		enemyCreature = Instantiate(GetEnemyCreature(), enemySpawn.position, Quaternion.identity);
		TrackCreature(enemyCreature);
	}
	Creature GetEnemyCreature(){
		int enemyTierIndex = GameManager.instance.cycleIndex;
		enemyTierIndex = Mathf.Min(enemyTiers.Length-1, enemyTierIndex);
		return enemyTiers[enemyTierIndex].GetRandom();
	}
	bool battleOver = false;
	void Update(){
		if(!active){return;}
		if(enemyCreature == null){
			return;
		}
		if(playerCreature == null){
			return;
		}
		if(battleOver){return;}
		enemyCreature.UpdateParts();
		playerCreature.UpdateParts();
		
		enemyCreature.PullTowards(playerCreature.GetCenter(),creatureAttraction);
		playerCreature.PullTowards(enemyCreature.GetCenter(),creatureAttraction);
		if (Input.GetKeyDown(KeyCode.Space)){
			playerCreature.Attack(enemyCreature.GetCenter());
		}

		float spaceImageAlpha = Mathf.Lerp(imageAlphaRange.x , imageAlphaRange.y,(Mathf.Sin(Time.time * 10) + 1) / 2.0f);
		spaceImage.color = Color.Lerp(spaceImage.color, playerCreature.CanAttack() ? new Color(1,1,1,spaceImageAlpha) : new Color(1,1,1,0), Time.deltaTime*30);
		if(enemyCreature.GetParts().Length == 0){
			battleOver = true;
			SlowMotion.LerpSpeedUp(0.2f);
			StartCoroutine(NextCycle());
		}
		else if(playerCreature.GetParts().Length == 0){
			battleOver = true;
			LevelManager.instance.LoadScene(3);
			//Game Over
			SlowMotion.LerpSpeedUp(0.2f);
		}
	}
	IEnumerator NextCycle(){
		text.Set(GameManager.instance.cycleIndex+1);
		animator.Play("wave");
		yield return new WaitForSeconds(2f);
		GameManager.instance.SwitchToCollectionPicker();

	}
	[System.Serializable]
	public class EnemyTier{
		[SerializeField] Creature[] enemyCreatures;
		public Creature GetRandom(){
			return enemyCreatures[Random.Range(0,enemyCreatures.Length)];
		}
	}
}
