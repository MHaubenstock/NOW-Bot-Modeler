using UnityEngine;
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
	public Camera mainCamera;

		//It is assumed that the NAO bot is symmetrical
	public Transform leftHand;	//For measuring, so no need for right hand
	public Transform leftShoulder;
	public Transform rightShoulder;
	public Transform leftCalf;	//For measuring, so no need for right calf
	public Transform leftThigh;
	public Transform rightThigh;
	public Transform movementPlane;
	public Stack<AnimationFrameRaw> undoFrameStack = new Stack<AnimationFrameRaw>();
	public Stack<AnimationFrameRaw> redoFrameStack = new Stack<AnimationFrameRaw>();


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

	private float x = 180.0f;
	private float y = 0.0f;
	//

	//For GUI
	public Camera builderCamera;
	public Camera fineTuneCamera;
	private bool isBuilding = true;
	private ModelAnimator modelAnimator;
	private ModelAnimationRaw workingAnimation;
	private List<ModelAnimation> startingPositions;
	private Rect setupWindowRect = new Rect(Screen.width * 0.2F, Screen.height * 0.25F, Screen.width * 0.6F, Screen.height * 0.5F);
	private Rect animateControlWindowRect = new Rect(0, 0, 500, 200);
	private Rect fineTuneWindowRect = new Rect(0, 0, 500, 500);
	private string animationName = "";
	private bool setupAnimation = true;
	private bool choseName = false;
	private bool animationIsPlaying = false;
	private int lastStartingPositionIndex = 0;
	private int movementSelectionInt = 0;
	private string[] movementNames;
	private Vector2 scrollPosition = Vector2.zero;
	private float sliderValue = 1;

		//For debugging
		private string debugString = "None";
	//

	// Use this for initialization
	void Start ()
	{
		modelAnimator = GetComponent<ModelAnimator>();

		//Load starting positions
		//startingPositions = modelAnimator.readAnimationsFromFile("Assets/Resources/NAOStartingPositions.txt");
		startingPositions = modelAnimator.readAnimationsFromFile(Application.dataPath + "/Resources/NAOStartingPositions.txt");
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
		//mainCamera = Camera.mainCamera;

		maxElbowDistanceFromShoulder = leftShoulder.lossyScale.x * .8F;
		maxHandDistanceFromShoulder = (leftShoulder.lossyScale.x * .8F) + (leftHand.lossyScale.x * .8F);
		maxKneeDistanceFromHip = leftThigh.lossyScale.x * 1.15F; //* .8F;
		//maxFootDistanceFromHip = (leftThigh.lossyScale.x * .8F) + (leftCalf.lossyScale.x * .8F);
		maxFootDistanceFromHip = (leftThigh.lossyScale.x * 1.15F) + (leftCalf.lossyScale.x * 1.15F);
	}
	
	void OnGUI()
	{
		//For debugging on runtime
		//GUI.Label(new Rect(100, 100, 1000, 30), debugString);
		//

		if(isBuilding)
			BuilderGUI();
		else
			FineTuneGUI();		
	}

	void BuilderGUI()
	{
		//Show buttons to play created animations
		modelAnimator.AnimationSelectionGUI();

		//Show restart button
		if(GUI.Button(new Rect(Screen.width - 71, 0, 70, 35), "Restart"))
		{
			resetBuilder();
		}

		//Show save all button
		if(GUI.Button(new Rect(Screen.width - 71, 36, 70, 35), "Save All"))
		{
			string serializedAnimations = modelAnimator.serializeAnimations();
			modelAnimator.saveAnimations(serializedAnimations);
		}

		//Show undo button, moves NAO back to its last position based on mouse down and up
		if(GUI.Button(new Rect(Screen.width - 141, 0, 70, 35), "Undo"))
		{
			undoMovement();
		}

		//Show undo button, moves NAO back to its last position based on mouse down and up
		if(GUI.Button(new Rect(Screen.width - 141, 36, 70, 35), "Redo"))
		{
			redoMovement();
		}

		//Show fine-tune button, switches to another camera containing the gui to tweak the moves
		if(GUI.Button(new Rect(Screen.width - 70, Screen.height - 35, 70, 35), "Fine-Tune"))
		{
			//populate with movement names
			movementNames = new string[modelAnimator.animations.Count];

			for(int m = 0; m < modelAnimator.animations.Count; ++m)
			{
				movementNames[m] += modelAnimator.animations[m].name;
			}

			//Turn off builder camer, turn on fine-tune camera
			fineTuneCamera.enabled = true;
			builderCamera.enabled = false;

			isBuilding = false;
		}


		//Show setup window before you begin making the animation
		if(setupAnimation)
			setupWindowRect = GUI.Window(0, setupWindowRect, SetupWindow, "Setup");
		else
		//Show the animation building controls
			animateControlWindowRect = GUI.Window(1, animateControlWindowRect, animationControls, "Controls");
	}

	void FineTuneGUI()
	{
		//Show builder button, switches back to the move builder
		if(GUI.Button(new Rect(Screen.width - 70, Screen.height - 35, 70, 35), "Build"))
		{
			//Turn off builder camer, turn on fine-tune camera
			builderCamera.enabled = true;
			fineTuneCamera.enabled = false;

			isBuilding = true;
			resetBuilder();
		}

		fineTuneWindowRect = GUI.Window(2, fineTuneWindowRect, fineTuneWindow, "Animation Settings");
	}

	void fineTuneWindow(int windowID)
	{
		//Display movements to choose
		movementSelectionInt = GUI.SelectionGrid(new Rect(0, 20, fineTuneWindowRect.width, 150), movementSelectionInt, movementNames, 3);

		scrollPosition = GUI.BeginScrollView(new Rect(0, 180, fineTuneWindowRect.width, 150), scrollPosition, new Rect(0, 0, fineTuneWindowRect.width, movementNames.Length * 30));
		//Show chosen movements settings
		for(int f = 0; f < modelAnimator.animations[movementSelectionInt].frames.Length; ++f)
		{
			//frame label
			GUI.Label(new Rect(10, 21 * f, fineTuneWindowRect.width * 0.3F, 20), "Frame " + f);

			sliderValue = modelAnimator.animations[movementSelectionInt].frames[f].playbackSpeed;

			//frame speed slider
			modelAnimator.animations[movementSelectionInt].frames[f].playbackSpeed = GUI.HorizontalSlider(new Rect(20 + fineTuneWindowRect.width * 0.3F, 21 * f, fineTuneWindowRect.width * 0.5F, 20), sliderValue, 0, 100);

		}

		GUI.EndScrollView();

		//Test button tests the current movement
		if(GUI.Button(new Rect(0, 335, 120, 30), "Test Movement"))
		{
			modelAnimator.animateModel(movementSelectionInt);
		}

		//Save button saves all
		if(GUI.Button(new Rect(0, 370, 120, 30), "Save"))
		{
			string serializedAnimations = modelAnimator.serializeAnimations();
			modelAnimator.saveAnimations(serializedAnimations);
		}
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
			workingAnimation.addState(workingAnimation.getState());
		}

		//Finish and proceess the animation, then move back to original stance and ask if they want to make another
		if(GUI.Button(new Rect(marginSize * 3 + fixtureWidth, animateControlWindowRect.height * 0.45F, fixtureWidth, animateControlWindowRect.height * 0.5F), "Finish"))
		{
			//proccess and store animation
			modelAnimator.animations.Add(workingAnimation.processFinishedAnimation());

			//Reset the process
			resetBuilder();
		}

		GUI.Label(new Rect(marginSize * 5 + fixtureWidth * 2, animateControlWindowRect.height * 0.45F, fixtureWidth, animateControlWindowRect.height * 0.5F), "Num of Frames: " + (workingAnimation.frameNumber - 2));
	}

	void resetBuilder()
	{
		//Move back to original stance
		StartCoroutine(modelAnimator.animateModel(startingPositions[lastStartingPositionIndex], val => animationIsPlaying = val));

		setupAnimation = true;
		choseName = false;
		animationIsPlaying = false;

		resetUndoStack();
		resetRedoStack();
	}

	void undoMovement()
	{
		if(undoFrameStack.Count == 0)
			return;

		//Set NAO positions and rotations to last position
		AnimationFrameRaw lastState = undoFrameStack.Pop();

		//Add to redo movement stack
		redoFrameStack.Push(workingAnimation.getState());

		//Iterate through each transform in the model and gather its position and rotation
		for(int t = 0; t < lastState.theTransforms.Length; ++t)
		{
			lastState.theTransforms[t].localPosition = lastState.positionStates[t];
			lastState.theTransforms[t].localRotation = lastState.rotationStates[t];
		}
	}

	void redoMovement()
	{
		if(redoFrameStack.Count == 0)
			return;

		//Set NAO positions and rotations to last position
		AnimationFrameRaw lastState = redoFrameStack.Pop();

		//Add to redo movement stack
		undoFrameStack.Push(workingAnimation.getState());

		//Iterate through each transform in the model and gather its position and rotation
		for(int t = 0; t < lastState.theTransforms.Length; ++t)
		{
			lastState.theTransforms[t].localPosition = lastState.positionStates[t];
			lastState.theTransforms[t].localRotation = lastState.rotationStates[t];
		}
	}

	void resetUndoStack()
	{
		undoFrameStack = new Stack<AnimationFrameRaw>();
	}

	void resetRedoStack()
	{
		redoFrameStack = new Stack<AnimationFrameRaw>();
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
				debugString = Camera.mainCamera.transform.name + "  ";
				if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
				{
					movementPlane.position = hit.transform.position;
					movementPlane.rotation = Quaternion.LookRotation(new Vector3(movementPlane.forward.x, 0, movementPlane.forward.z), Vector3.up);

					debugString += "\n" + hit.transform.tag;

					if (hit.transform.tag == "DraggableArm")
					{
						//Save current state to stack so movement can be undone
						undoFrameStack.Push(workingAnimation.getState());
						resetRedoStack();

						StartCoroutine (DragArm(hit.distance, hit.transform, 1 << 8));
					}

					else if (hit.transform.tag == "DraggableLeg")
					{
						//Save current state to stack so movement can be undone
						undoFrameStack.Push(workingAnimation.getState());
						resetRedoStack();

						StartCoroutine (DragLeg(hit.distance, hit.transform, 1 << 8));
					}

					else if(hit.transform.tag == "DraggableTorso")
					{
						//Save current state to stack so movement can be undone
						undoFrameStack.Push(workingAnimation.getState());
						resetRedoStack();

						StartCoroutine (DragTorso(hit.distance, hit.transform, 1 << 8));	
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
		bool isLeftSide = go.name == "Forearm_Left";
		Transform shoulder = (isLeftSide ? leftShoulder : rightShoulder);
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
		bool isLeftSide = go.name == "Foot_Left";
		Transform thigh = (isLeftSide ? leftThigh : rightThigh);
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
			calf.Rotate((180 + Vector3.Angle(calf.right, Vector3.down)) * (isLeftSide ? 1 : -1), 0, 0);

			//Debug.Log(Quaternion.LookRotation(thigh.position - calf.position, Vector3.right));
			thigh.right = thigh.position - calf.position;
			thigh.Rotate((180 - Vector3.Angle(thigh.right, Vector3.up)) * (isLeftSide ? 1 : -1), 0, 0);
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

	IEnumerator DragTorso (float distance, Transform go, int layerMask)
	{
		Transform torso = go;

		while (Input.GetMouseButton (0))
		{
			//Set distance based on distance to the plane for that limb
			//Get distance to plane
			RaycastHit hit;
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100, layerMask))
			{
				distance = hit.distance;
			}

			//Do IK Stuff
			Vector3 targetDirection = torso.position - hit.point;
			torso.right = targetDirection;

			float angleToNAO = Vector3.Angle(ray.direction, Vector3.forward);
			if(angleToNAO < 135 || angleToNAO > 180)
			{
				Debug.Log(angleToNAO);
				torso.Rotate(Vector3.Angle(torso.right, Vector3.down) * (hit.point.z >= 0 ? 1 : -1), 0, 0);
			}

			yield return true;
		}
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