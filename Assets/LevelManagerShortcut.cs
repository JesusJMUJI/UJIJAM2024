using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManagerShortcut : MonoBehaviour
{
	LevelManager levelManager;
	void Start(){
		levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	public void LoadLevel(int levelIndex){
		LevelManager.instance.LoadScene(levelIndex);
	}
	public void ReloadLevel(){
		LevelManager.instance.ReloadScene();
	}
	public void LoadNextLevel(){
		LevelManager.instance.LoadNextScene();
	}
}
