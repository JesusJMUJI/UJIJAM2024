using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
	static SlowMotion instance;
	void Awake(){
		if(instance != null){
			Debug.LogError("Multiple SlowMotion active!");
		}
		instance = this;
	}
	static float realPhysicsTime = 0.02f;
	static float realTime = 1;
	static Coroutine Timelerp;
	public static void SlowDown(float speed){
		SetTimeScale(speed);
	}
	public static void SpeedUp(){
		SetTimeScale(realTime);
		
	}
	public static void LerpSlowDown(float speed, float duration){
		if(Timelerp != null){
			instance.StopCoroutine(Timelerp);
		}
		Timelerp = instance.StartCoroutine(lerpTime(speed, duration));
	}
	static IEnumerator lerpTime(float finaltime, float duration){
		float originaltime = Time.timeScale;
		for(float elapsed = 0; elapsed <= duration; elapsed += Time.deltaTime){
			float newTime = Mathf.Lerp(originaltime, finaltime, elapsed/duration);
			SetTimeScale(newTime);
			yield return null;
		}
		SetTimeScale(finaltime);
	}
	public static void LerpSpeedUp(float duration){
		if(Timelerp != null){
			instance.StopCoroutine(Timelerp);
		}
		Timelerp = instance.StartCoroutine(lerpTime(realTime, duration));
	}
	static void SetTimeScale(float newTimeScale){
		float delta = Time.timeScale/newTimeScale;
		Time.timeScale = newTimeScale;
		Time.fixedDeltaTime = newTimeScale * (realPhysicsTime/realTime);
	}
}
