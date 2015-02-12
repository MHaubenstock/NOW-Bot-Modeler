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

	public float leftShoulder;
	public float leftElbow;
	public float leftHip;
	public float leftKnee;
	public float rightShoulder;
	public float rightElbow;
	public float rightHip;
	public float rightKnee;

	private float leftShoulderP;
	private float leftElbowP;
	private float leftHipP;
	private float leftKneeP;
	private float rightShoulderP;
	private float rightElbowP;
	private float rightHipP;
	private float rightKneeP;

	public float armLength;
	public float forearmLength;
	public float thighLength;
	public float legLength;

	// Use this for initialization
	void Start ()
	{
		leftShoulder = LeftShoulder.localRotation.x;
		leftElbow = LeftElbow.localRotation.x;
		leftHip = LeftHip.localRotation.x;
		leftKnee = LeftKnee.localRotation.x;

		rightShoulder = RightShoulder.localRotation.x;
		rightElbow = RightElbow.localRotation.x;
		rightHip = RightHip.localRotation.x;
		rightKnee = RightKnee.localRotation.x;

		leftShoulderP = LeftShoulder.localRotation.x;
		leftElbowP = LeftElbow.localRotation.x;
		leftHipP = LeftHip.localRotation.x;
		leftKneeP = LeftKnee.localRotation.x;

		rightShoulderP = RightShoulder.localRotation.x;
		rightElbowP = RightElbow.localRotation.x;
		rightHipP = RightHip.localRotation.x;
		rightKneeP = RightKnee.localRotation.x;

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

		leftShoulderP = leftShoulder - leftShoulderP;
		leftElbowP = leftElbow - leftElbowP;
		leftHipP = leftHip - leftHipP;
		leftKneeP = leftKnee - leftKneeP;
		rightShoulderP = rightShoulder - rightShoulderP;
		rightElbowP = rightElbow - rightElbowP;
		rightHipP = rightHip - rightHipP;
		rightKneeP = rightKnee - rightKneeP;

		LeftShoulder.localRotation = LeftShoulder.localRotation * Quaternion.Euler(leftShoulderP, 0, 0);
		LeftElbow.localRotation = LeftElbow.localRotation * Quaternion.Euler(leftElbowP, 0, 0);
		LeftHip.localRotation = LeftHip.localRotation * Quaternion.Euler(leftHipP, 0, 0);
		LeftKnee.localRotation = LeftKnee.localRotation * Quaternion.Euler(leftKneeP, 0, 0);

		RightShoulder.localRotation = RightShoulder.localRotation * Quaternion.Euler(rightShoulderP, 0, 0);
		RightElbow.localRotation = RightElbow.localRotation * Quaternion.Euler(rightElbowP, 0, 0);
		RightHip.localRotation = RightHip.localRotation * Quaternion.Euler(rightHipP, 0, 0);
		RightKnee.localRotation = RightKnee.localRotation * Quaternion.Euler(rightKneeP, 0, 0);

		leftShoulderP = leftShoulder;
		leftElbowP = leftElbow;
		leftHipP = leftHip;
		leftKneeP = leftKnee;
		rightShoulderP = rightShoulder;
		rightElbowP = rightElbow;
		rightHipP = rightHip;
		rightKneeP = rightKnee;
	}

	virtual public void calculateIK()
	{
	}

	//Sets the rotation ofthe NAO bot's joints, Pass in null if you don't want one to change
	private void SetRotations(float lShoulder, float lElbow, float lHip, float lKnee, float rShoulder, float rElbow, float rHip, float rKnee)
	{
		leftShoulder = lShoulder % 360;
		leftElbow = lElbow % 360;
		leftHip = lHip % 360;
		leftKnee = lKnee % 360;

		rightShoulder = rShoulder % 360;
		rightElbow = rElbow % 360;
		rightHip = rHip % 360;
		rightKnee = rKnee % 360;
	}

	//Returns dictionary of the robots joint rotations
	//DO ONE WITH JUST AN ARRAY AS WELL?
	private void GetRotations()
	{

	}
}
