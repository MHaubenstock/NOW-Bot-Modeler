using UnityEngine;
using System.Collections;

public class NAOBotKinematics : IntroToRoboticsAPI
{

	// Use this for initialization
	override public void initialization()
	{
		Debug.Log("This is how you write to the console.");
	}
	
	//Use this to calculate IK
	override public void calculateIK()
	{
		leftShoulder += new Vector3(1.0F, 0, 0);
	}
}
