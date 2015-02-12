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

	public float leftShoulderF;
	public float leftShoulderL;
	public float leftElbowF;
	public float leftElbowL;
	public float leftHipF;
	public float leftHipL;
	public float leftKneeF;
	public float leftKneeL;
	public float rightShoulderF;
	public float rightShoulderL;
	public float rightElbowF;
	public float rightElbowL;
	public float rightHipF;
	public float rightHipL;
	public float rightKneeF;
	public float rightKneeL;

	private float leftShoulderPF;
	private float leftShoulderPL;
	private float leftElbowPF;
	private float leftElbowPL;
	private float leftHipPF;
	private float leftHipPL;
	private float leftKneePF;
	private float leftKneePL;
	private float rightShoulderPF;
	private float rightShoulderPL;
	private float rightElbowPF;
	private float rightElbowPL;
	private float rightHipPF;
	private float rightHipPL;
	private float rightKneePF;
	private float rightKneePL;

	public float armLength;
	public float forearmLength;
	public float thighLength;
	public float legLength;

	// Use this for initialization
	void Start ()
	{
		//Forward joint rotations
		leftShoulderF = LeftShoulder.localRotation.x;
		leftElbowF = LeftElbow.localRotation.x;
		leftHipF = LeftHip.localRotation.x;
		leftKneeF = LeftKnee.localRotation.x;

		rightShoulderF = RightShoulder.localRotation.x;
		rightElbowF = RightElbow.localRotation.x;
		rightHipF = RightHip.localRotation.x;
		rightKneeF = RightKnee.localRotation.x;

		leftShoulderPF = LeftShoulder.localRotation.x;
		leftElbowPF = LeftElbow.localRotation.x;
		leftHipPF = LeftHip.localRotation.x;
		leftKneePF = LeftKnee.localRotation.x;

		rightShoulderPF = RightShoulder.localRotation.x;
		rightElbowPF = RightElbow.localRotation.x;
		rightHipPF = RightHip.localRotation.x;
		rightKneePF = RightKnee.localRotation.x;

		//Lateral joint rotations
		leftShoulderL = LeftShoulder.localRotation.z;
		leftElbowL = LeftElbow.localRotation.y;
		leftHipL = LeftHip.localRotation.z;
		leftKneeL = LeftKnee.localRotation.y;

		rightShoulderL = RightShoulder.localRotation.z;
		rightElbowL = RightElbow.localRotation.y;
		rightHipL = RightHip.localRotation.z;
		rightKneeL = RightKnee.localRotation.y;

		leftShoulderPL = LeftShoulder.localRotation.z;
		leftElbowPL = LeftElbow.localRotation.y;
		leftHipPL = LeftHip.localRotation.z;
		leftKneePL = LeftKnee.localRotation.y;

		rightShoulderPL = RightShoulder.localRotation.z;
		rightElbowPL = RightElbow.localRotation.y;
		rightHipPL = RightHip.localRotation.z;
		rightKneePL = RightKnee.localRotation.y;

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

		/*
		leftShoulderPF = leftShoulderF - leftShoulderPF;
		leftElbowPF = leftElbowF - leftElbowPF;
		leftHipPF = leftHipF - leftHipPF;
		leftKneePF = leftKneeF - leftKneePF;
		rightShoulderPF = rightShoulderF - rightShoulderPF;
		rightElbowPF = rightElbowF - rightElbowPF;
		rightHipPF = rightHipF - rightHipPF;
		rightKneePF = rightKneeF - rightKneePF;

		leftShoulderPL = leftShoulderL - leftShoulderPL;
		leftElbowPL = leftElbowL - leftElbowPL;
		leftHipPL = leftHipL - leftHipPL;
		leftKneePL = leftKneeL - leftKneePL;
		rightShoulderPL = rightShoulderL - rightShoulderPL;
		rightElbowPL = rightElbowL - rightElbowPL;
		rightHipPL = rightHipL - rightHipPL;
		rightKneePL = rightKneeL - rightKneePL;
		*/
		leftShoulderPF = leftShoulderPF - leftShoulderF;
		leftElbowPF = leftElbowPF - leftElbowF;
		leftHipPF = leftHipPF - leftHipF;
		leftKneePF = leftKneePF - leftKneeF;
		rightShoulderPF = rightShoulderPF - rightShoulderF;
		rightElbowPF = rightElbowPF - rightElbowF;
		rightHipPF = rightHipPF - rightHipF;
		rightKneePF = rightKneePF - rightKneeF;

		leftShoulderPL = leftShoulderPL - leftShoulderL;
		leftElbowPL = leftElbowPL - leftElbowL;
		leftHipPL = leftHipPL - leftHipL;
		leftKneePL = leftKneePL - leftKneeL;
		rightShoulderPL = rightShoulderPL - rightShoulderL;
		rightElbowPL = rightElbowPL - rightElbowL;
		rightHipPL = rightHipPL - rightHipL;
		rightKneePL = rightKneePL - rightKneeL;


		LeftShoulder.localRotation = LeftShoulder.localRotation * Quaternion.Euler(leftShoulderPF, 0, leftShoulderPL);
		LeftElbow.localRotation = LeftElbow.localRotation * Quaternion.Euler(leftElbowPF, leftElbowPL, 0);
		LeftHip.localRotation = LeftHip.localRotation * Quaternion.Euler(leftHipPF, 0, leftHipPL);
		LeftKnee.localRotation = LeftKnee.localRotation * Quaternion.Euler(leftKneePF, leftKneePL, 0);

		RightShoulder.localRotation = RightShoulder.localRotation * Quaternion.Euler(rightShoulderPF, 0, rightShoulderPL);
		RightElbow.localRotation = RightElbow.localRotation * Quaternion.Euler(rightElbowPF, rightElbowPL, 0);
		RightHip.localRotation = RightHip.localRotation * Quaternion.Euler(rightHipPF, 0, rightHipPL);
		RightKnee.localRotation = RightKnee.localRotation * Quaternion.Euler(rightKneePF, rightKneePL, 0);



		leftShoulderPF = leftShoulderF;
		leftElbowPF = leftElbowF;
		leftHipPF = leftHipF;
		leftKneePF = leftKneeF;
		rightShoulderPF = rightShoulderF;
		rightElbowPF = rightElbowF;
		rightHipPF = rightHipF;
		rightKneePF = rightKneeF;

		leftShoulderPL = leftShoulderL;
		leftElbowPL = leftElbowL;
		leftHipPL = leftHipL;
		leftKneePL = leftKneeL;
		rightShoulderPL = rightShoulderL;
		rightElbowPL = rightElbowL;
		rightHipPL = rightHipL;
		rightKneePL = rightKneeL;
	}

	virtual public void calculateIK()
	{
	}

	//Sets the rotation ofthe NAO bot's joints, Pass in null if you don't want one to change
	private void SetRotations(float lShoulder, float lElbow, float lHip, float lKnee, float rShoulder, float rElbow, float rHip, float rKnee)
	{

	}

	//Returns dictionary of the robots joint rotations
	//DO ONE WITH JUST AN ARRAY AS WELL?
	private void GetRotations()
	{

	}
}
