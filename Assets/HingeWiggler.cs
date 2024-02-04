using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeWiggler : MonoBehaviour
{
	[SerializeField] float slowdownAngle = 5;
	[SerializeField] HingeJoint2D joint;
	[SerializeField] NoiseSampler sampler;
	[SerializeField] float seed;
	[SerializeField] PartAsset asset;
	public void SetSampler(NoiseSampler _sampler){
		sampler = _sampler;
	}
	public void SetHinge(HingeJoint2D _joint){
		joint = _joint;
	}
	public void SetSeed(float _seed){
		seed = _seed;
	}
	public void SetAsset(PartAsset _asset){
		asset = _asset;
	}
	void Start(){
		joint.useMotor = true;
	}
	void Update()
	{
		if(joint == null){return;}
		if(joint.attachedRigidbody == null){
			Destroy(joint);
			Destroy(this);
			return;
		}

		float fase = sampler.SampleAt((Vector2)transform.position + Vector2.down*seed*12.445f);

		float targetAngle = Mathf.Lerp(asset.angleRange.x, asset.angleRange.y, fase);
		float deltaAngle = Mathf.DeltaAngle(joint.jointAngle,-targetAngle);
		float angleFactor = Mathf.Clamp(deltaAngle/slowdownAngle,-1,1);
		JointMotor2D motor = joint.motor;
		motor.maxMotorTorque = 4000;
		motor.motorSpeed = Mathf.LerpUnclamped(0,asset.speed,angleFactor);
		joint.motor = motor;
	}
}
