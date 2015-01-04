﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementBuilder : MonoBehaviour
{
	//For moving body parts
	public float spring = 50.0f;
	public float damper = 5.0f;
	public float drag = 10.0f;
	public float angularDrag = 5.0f;
	public float dragDistance = 0.2f;
	public bool attachToCenterOfMass = false;
	public bool allowDragging = false;

	private SpringJoint springJoint;
	private Camera mainCamera;

		//It is assumed that the NAO bot is symmetrical
	public Transform leftHand;	//For measuring, so no need for right hand
	public Transform leftShoulder;
	public Transform rightShoulder;
	public Transform leftCalf;	//For measuring, so no need for right calf
	public Transform leftThigh;
	public Transform rightThigh;
	public Transform movementPlane;

	private float maxElbowDistanceFromShoulder;
	private float maxHandDistanceFromShoulder;
	private float maxKneeDistanceFromHip;
	private float maxFootDistanceFromHip;
	//

	//For mouse orbiting
	public Transform target;
	public float cameraOrbitDistance = 10.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	private float x = 0.0f;
	private float y = 0.0f;
	//

	//For GUI
	private ModelAnimator modelAnimator;
	private ModelAnimationRaw workingAnimation;
	private List<ModelAnimation> startingPositions;
	private Rect setupWindowRect = new Rect(Screen.width * 0.2F, Screen.height * 0.25F, Screen.width * 0.6F, Screen.height * 0.5F);
	private Rect animateControlWindowRect = new Rect(0, 0, Screen.width * 0.6F, Screen.height * 0.15F);
	private string animationName = "";
	private bool setupAnimation = true;
	private bool choseName = false;
	private bool animationIsPlaying = false;
	private int lastStartingPositionIndex = 0;
	//

	// Use this for initialization
	void Start ()
	{
		modelAnimator = GetComponent<ModelAnimator>();;
		//Load starting positions
		startingPositions = modelAnimator.readAnimationsFromFile("Assets/Resources/NAOStartingPositions.txt");
		//Set returnToStartingPosition to false for each one
		foreach(ModelAnimation anim in startingPositions)
			anim.returnToStartingPosition = false;

		//Load locally created moves
		modelAnimator.animations = modelAnimator.readAnimationsFromFile();

		//Set up mouse orbit stuff
		Vector3 angles = transform.eulerAngles;
	    x = angles.y;
	    y = angles.x;

		transform.rotation = Quaternion.Euler(y, x, 0);
		//transform.position = transform.rotation * new Vector3(0.0f, 1.0f, -cameraOrbitDistance) + target.position;
		transform.position = target.transform.position - new Vector3(0.0f, -1.0f, -cameraOrbitDistance);

		// Make the rigid body not change rotation
	   	if (rigidbody)
			rigidbody.freezeRotation = true;

		//Set up dragging stuff
		mainCamera = Camera.mainCamera;

		maxElbowDistanceFromShoulder = leftShoulder.lossyScale.x * .8F;
		maxHandDistanceFromShoulder = (leftShoulder.lossyScale.x * .8F) + (leftHand.lossyScale.x * .8F);
		maxKneeDistanceFromHip = leftThigh.lossyScale.x * .8F;
		maxFootDistanceFromHip = (leftThigh.lossyScale.x * .8F) + (leftCalf.lossyScale.x * .8F);
	}
	
	void OnGUI()
	{
		//Show restart button
		if(GUI.Button(new Rect(Screen.width * 0.8F, 0, Screen.width * 0.2F, Screen.width * 0.08F), "Restart"))
		{
			resetBuilder();
		}

		//Show save all button
		if(GUI.Button(new Rect(Screen.width * 0.8F, Screen.width * 0.08F, Screen.width * 0.2F, Screen.width * 0.08F), "Save All"))
		{
			string serializedAnimations = modelAnimator.serializeAnimations();
			modelAnimator.saveAnimations(serializedAnimations);
		}

		//Show undo button, moves NAO back to its last position based on mouse down and up
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************

		//Show fine-tune button, switches to another camera containing the gui to tweak the moves
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************
		//************************************************************************************

		//Show setup window before you begin making the animation
		if(setupAnimation)
			setupWindowRect = GUI.Window(0, setupWindowRect, SetupWindow, "Setup");
		else
		//Show the animation building controls
			animateControlWindowRect = GUI.Window(1, animateControlWindowRect, animationControls, "Controls");
	}

	void SetupWindow(int windowID)
	{
		//Prompt animation name
		if(!choseName)
		{
			GUI.Label(new Rect(setupWindowRect.width * 0.025F, setupWindowRect.height * 0.2F, setupWindowRect.width * 0.95F, 25), "Enter a name for the movement:");
			animationName = GUI.TextField(new Rect(setupWindowRect.width * 0.025F, setupWindowRect.height * 0.4F, setupWindowRect.width * 0.95F, 20), animationName, 25);

			if(GUI.Button(new Rect(setupWindowRect.width * 0.15F, setupWindowRect.height * 0.7F, setupWindowRect.width * 0.7F, 40), "Next") && animationName != "")
				choseName = true;
		}
		//prompt for starting position
		else
		{
			GUI.Label(new Rect(setupWindowRect.width * 0.025F, setupWindowRect.height * 0.2F, setupWindowRect.width * 0.95F, 25), "Choose a starting position");

			for(int x = 0; x < startingPositions.Count; ++x)
			{
				if(GUI.Button(new Rect(setupWindowRect.width * 0.15F, setupWindowRect.height * 0.2F + (43 * (x + 1)), setupWindowRect.width * 0.7F, 40), startingPositions[x].name))
				{
					setupAnimation = false;
					allowDragging = true;
					lastStartingPositionIndex = x;

					//Animate to starting position
					modelAnimator.gatherTransforms();
					StartCoroutine(modelAnimator.animateModel(startingPositions[x], val => animationIsPlaying = val));

					//Begin creating an animation
					workingAnimation = new ModelAnimationRaw(animationName, modelAnimator.model.GetComponentsInChildren<Transform>());
				}
			}
		}
	}

	void animationControls(int windowID)
	{
		int numOfFixtures = 3;
		float marginSize = animateControlWindowRect.width * 0.05F;
		float fixtureWidth = (animateControlWindowRect.width - marginSize * 6) / numOfFixtures;

		if(GUI.Button(new Rect(marginSize, animateControlWindowRect.height * 0.45F, fixtureWidth, animateControlWindowRect.height * 0.5F), "Save State"))
		{
			workingAnimation.getState();
		}

		//Finish and proceess the animation, then move back to original stance and ask if they want to make another
		if(GUI.Button(new Rect(marginSize * 3 + fixtureWidth, animateControlWindowRect.height * 0.45F, fixtureWidth, animateControlWindowRect.height * 0.5F), "Finish"))
		{
			//proccess and store animation
			modelAnimator.animations.Add(workingAnimation.processFinishedAnimation());

			//Move back to original stance
			StartCoroutine(modelAnimator.animateModel(startingPositions[lastStartingPositionIndex], val => animationIsPlaying = val));

			//Reset the process
			resetBuilder();
		}

		GUI.Label(new Rect(marginSize * 5 + fixtureWidth * 2, animateControlWindowRect.height * 0.45F, fixtureWidth, animateControlWindowRect.height * 0.5F), "Num of Frames: " + workingAnimation.frameNumber);
	}

	void resetBuilder()
	{
		setupAnimation = true;
		choseName = false;
		animationIsPlaying = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if(allowDragging)
		{
			//Dragging stuff
			// Make sure the user pressed the mouse down
			if (Input.GetMouseButtonDown (0))
			{
				// We need to actually hit an object
				RaycastHit hit;
				int layerMask = 1 << 12;	//DraggableLayer

				if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
				{
					movementPlane.position = hit.transform.position;
					movementPlane.rotation = Quaternion.LookRotation(new Vector3(movementPlane.forward.x, 0, movementPlane.forward.z), Vector3.up);

					if (hit.transform.tag == "DraggableArm")
					{
						StartCoroutine (DragArm(hit.distance, hit.transform, 1 << 8));
					}

					if (hit.transform.tag == "DraggableLeg")
					{
						StartCoroutine (DragLeg(hit.distance, hit.transform, 1 << 8));
					}
				}
			}
		}
	}

	void LateUpdate()
	{
		if (target && Input.GetMouseButton(1)) 
		{
	        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
	        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
	 		
	 		y = ClampAngle(y, yMinLimit, yMaxLimit);
	 		       
	        Quaternion rotation = Quaternion.Euler(y, x, 0);
	        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -cameraOrbitDistance) + target.position;
	        
	        transform.rotation = rotation;
	        transform.position = position;
	    }
	}

	IEnumerator DragArm (float distance, Transform go, int layerMask)
	{
		Transform shoulder = (go.name == "Forearm_Left" ? leftShoulder : rightShoulder);
		Transform hand = go;

		while (Input.GetMouseButton (0))
		{
			//Set distance based on distance to the plane for that limb
			//Get distance to plane
			RaycastHit hit;
			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
			{
				distance = hit.distance;
			}

			//Do IK Stuff
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			Vector3 targetPos = constrainRobotArmMovement(ray.GetPoint(distance), shoulder, hand);
			hand.position = Vector3.Lerp(shoulder.position + (Vector3.down * maxElbowDistanceFromShoulder), shoulder.position + ((targetPos - shoulder.position) * maxElbowDistanceFromShoulder), Vector3.Distance(targetPos, shoulder.position) / maxHandDistanceFromShoulder);
			hand.position += ((hand.position - shoulder.position).normalized * maxElbowDistanceFromShoulder) - (hand.position - shoulder.position);
			
			Vector3 resultDirection = hand.position - constrainRobotArmMovement(ray.GetPoint(distance), shoulder, hand);
			hand.right = resultDirection;
			
			shoulder.right = -(hand.position - shoulder.position);
			
			yield return true;
		}
	}

	Vector3 constrainRobotArmMovement(Vector3 proposedPosition, Transform shoulder, Transform hand)
	{
		Debug.DrawLine(proposedPosition, shoulder.position, Color.red);
		Debug.DrawRay(shoulder.position, hand.position - shoulder.position, Color.blue);
		Debug.DrawRay(hand.position, hand.position - (shoulder.position - shoulder.localScale.x * shoulder.right), Color.green);

		//if proposed position is a valid position, return it
		if(Vector3.Distance(proposedPosition, shoulder.position) <= maxHandDistanceFromShoulder)
			return proposedPosition;

		//else return the closest valid position to the proposed position
		else
			return shoulder.position + (proposedPosition - shoulder.position).normalized * maxHandDistanceFromShoulder;
	}

	IEnumerator DragLeg (float distance, Transform go, int layerMask)
	{
		Transform thigh = (go.name == "Foot_Left" ? leftThigh : rightThigh);
		Transform calf = go;

		while (Input.GetMouseButton (0))
		{
			//Set distance based on distance to the plane for that limb
			//Get distance to plane
			RaycastHit hit;
			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
			{
				distance = hit.distance;
			}

			//Do IK Stuff
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			Vector3 targetPos = constrainRobotLegMovement(ray.GetPoint(distance), thigh, calf);
			calf.position = Vector3.Lerp(thigh.position + (Vector3.up * maxKneeDistanceFromHip), thigh.position + ((targetPos - thigh.position) * maxKneeDistanceFromHip), Vector3.Distance(targetPos, thigh.position) / maxFootDistanceFromHip);
			calf.position += ((calf.position - thigh.position).normalized * maxKneeDistanceFromHip) - (calf.position - thigh.position);
			
			Vector3 resultDirection = calf.position - constrainRobotLegMovement(ray.GetPoint(distance), thigh, calf);
			calf.right = resultDirection;

			//The leg segments are rotating incorrectly, come back to this later
			Debug.Log(Vector3.Angle(calf.right, Vector3.up));
			calf.Rotate(180, 0, 0);
			calf.Rotate(180 + Vector3.Angle(calf.right, Vector3.down), 0, 0);

			//Debug.Log(Quaternion.LookRotation(thigh.position - calf.position, Vector3.right));
			thigh.right = thigh.position - calf.position;
			thigh.Rotate(180 - Vector3.Angle(thigh.right, Vector3.up), 0, 0);
			//thigh.rotation = Quaternion.LookRotation(thigh.position - calf.position, Vector3.up);

			//thigh.up = Vector3.left;
			//thigh.rotation.x = 180;
			//Debug.Log(thigh.rotation);
			
			yield return true;
		}
	}

	Vector3 constrainRobotLegMovement(Vector3 proposedPosition, Transform thigh, Transform calf)
	{
		Debug.DrawLine(proposedPosition, thigh.position, Color.red);
		Debug.DrawRay(thigh.position, calf.position - thigh.position, Color.blue);
		Debug.DrawRay(calf.position, calf.position - (thigh.position - thigh.localScale.x * thigh.right), Color.green);

		//if proposed position is a valid position, return it
		if(Vector3.Distance(proposedPosition, thigh.position) <= maxFootDistanceFromHip)
			return proposedPosition;

		//else return the closest valid position to the proposed position
		else
			return thigh.position + (proposedPosition - thigh.position).normalized * maxFootDistanceFromHip;
	}

	static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;

		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp (angle, min, max);
	}
}