using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManagerShortcut : MonoBehaviour
{
	public void LoadLevel(int levelIndex){
		LevelManager.instance.LoadScene(levelIndex);
	}
	public void ReloadLevel(){
		LevelManager.instance.ReloadScene();
	}
	public void LoadNextLevel(){
		LevelManager.instance.LoadNextScene();
	}
	public void LoadAbout(){
		LevelManager.instance.LoadScene(1);
	}

	public void LoadBuilder()
	{
		LevelManager.instance.LoadScene(2);
	}

	public void TestExit()
	{
		Application.Quit();
		Debug.Log("Your game perhaps has quitten, you should consider death");
	}
}
