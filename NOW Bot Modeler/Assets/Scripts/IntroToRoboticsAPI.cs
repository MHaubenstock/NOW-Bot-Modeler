using UnityEngine;
using System.Collections;

public class IntroToRoboticsAPI : MonoBehaviour
{	
	public Transform LeftShoulder;
	public Transform LeftElbow;
	public Transform LeftHip;
	public Transform LeftKnee;
	public Transform RightShoulder;
	public Transform RightElbow;
	public Transform RightHip;
	public Transform RightKnee;

	public Vector3 leftShoulder;
	public Vector3 leftElbow;
	public Vector3 leftHip;
	public Vector3 leftKnee;
	public Vector3 rightShoulder;
	public Vector3 rightElbow;
	public Vector3 rightHip;
	public Vector3 rightKnee;

	public float armLength;
	public float forearmLength;
	public float thighLength;
	public float legLength;

	// Use this for initialization
	void Start ()
	{
		leftShoulder = LeftShoulder.localEulerAngles;
		leftElbow = LeftElbow.localEulerAngles;
		leftHip = LeftHip.localEulerAngles;
		leftKnee = LeftKnee.localEulerAngles;

		rightShoulder = RightShoulder.localEulerAngles;
		rightElbow = RightElbow.localEulerAngles;
		rightHip = RightHip.localEulerAngles;
		rightKnee = RightKnee.localEulerAngles;

		armLength = Vector3.Distance(LeftShoulder.position, LeftElbow.position);
		forearmLength = LeftElbow.lossyScale.x;
		thighLength = Vector3.Distance(LeftHip.position, LeftKnee.position);
		legLength = LeftKnee.lossyScale.x;

		initialization();
	}

	virtual public void initialization()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		calculateIK();

		LeftShoulder.localEulerAngles = mod360(leftShoulder);
		LeftElbow.localEulerAngles = mod360(leftElbow);
		LeftHip.localEulerAngles = mod360(leftHip);
		LeftKnee.localEulerAngles = mod360(leftKnee);

		RightShoulder.localEulerAngles = mod360(rightShoulder);
		RightElbow.localEulerAngles = mod360(rightElbow);
		RightHip.localEulerAngles = mod360(rightHip);
		RightKnee.localEulerAngles = mod360(rightKnee);
	}

	virtual public void calculateIK()
	{
	}

	//Sets the rotation ofthe NAO bot's joints, Pass in null if you don't want one to change
	private void SetRotations(Vector3 lShoulder, Vector3 lElbow, Vector3 lHip, Vector3 lKnee, Vector3 rShoulder, Vector3 rElbow, Vector3 rHip, Vector3 rKnee)
	{
		leftShoulder = mod360(lShoulder);
		leftElbow = mod360(lElbow);
		leftHip = mod360(lHip);
		leftKnee = mod360(lKnee);

		rightShoulder = mod360(rShoulder);
		rightElbow = mod360(rElbow);
		rightHip = mod360(rHip);
		rightKnee = mod360(rKnee);
	}

	//Returns dictionary of the robots joint rotations
	//DO ONE WITH JUST AN ARRAY AS WELL?
	private void GetRotations()
	{

	}

	private Vector3 mod360(Vector3 vectorToMod)
	{
		return new Vector3(vectorToMod.x % 360, vectorToMod.y % 360, vectorToMod.z % 360);
	}
}
