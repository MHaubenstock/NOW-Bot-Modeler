using UnityEngine;
using System.Collections;

public class MovementBuilderOLD : MonoBehaviour
{
	//For testing///
	public GameObject marker;
	///

	public float theta = 0f;
	//For moving body parts
	public float spring = 50.0f;
	public float damper = 5.0f;
	public float drag = 10.0f;
	public float angularDrag = 5.0f;
	public float dragDistance = 0.2f;
	public bool attachToCenterOfMass = false;

	private SpringJoint springJoint;
	private Camera mainCamera;

		//It is assumed that the NAO bot is symmetrical
	public Transform leftHand;
	public Transform leftShoulder;

	public Transform testSphere;

	private float maxElbowDistanceFromShoulder;
	private float maxHandDistanceFromShoulder;
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

	// Use this for initialization
	void Start ()
	{
		//Set up mouse orbit stuff
		var angles = transform.eulerAngles;
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
		//maxHandDistanceFromShoulder = leftShoulder.localScale.x;
		maxElbowDistanceFromShoulder = leftShoulder.lossyScale.x * .8F;
		maxHandDistanceFromShoulder = (leftShoulder.lossyScale.x * .8F) + (leftHand.lossyScale.x * .8F);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Dragging stuff
		// Make sure the user pressed the mouse down
		if (Input.GetMouseButtonDown (0))
		{
			// We need to actually hit an object
			RaycastHit hit;

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				// We need to hit a rigidbody that is not kinematic
				if (hit.rigidbody && hit.transform.tag == "DraggablePart")
				{					
					StartCoroutine (DragObject(hit.distance, hit.transform));
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

	IEnumerator DragObject (float distance, Transform go)
	{
		while (Input.GetMouseButton (0))
		{
			//Set distance based on distance to the plane for that limb
			//Get distance to plane
			RaycastHit hit;
			int layerMask = 1 << 8;
			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
			{				
					distance = hit.distance;
			}

			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			//go.position = constrainRobotArmMovement(ray.GetPoint(distance));
			Vector3 targetPos = constrainRobotArmMovement(ray.GetPoint(distance));
			go.position = Vector3.Lerp(leftShoulder.position + (Vector3.down * maxElbowDistanceFromShoulder), leftShoulder.position + ((targetPos - leftShoulder.position) * maxElbowDistanceFromShoulder), Vector3.Distance(targetPos, leftShoulder.position) / maxHandDistanceFromShoulder);
			//go.position = (go.position - leftShoulder.position).normalized * leftShoulder.lossyScale.x * .8F;
			go.position += ((go.position - leftShoulder.position).normalized * leftShoulder.lossyScale.x * .8F) - (go.position - leftShoulder.position);
			//Debug.Log(maxElbowDistanceFromShoulder + "    " + Vector3.Distance(leftHand.position, leftShoulder.position) + "    " + Vector3.Distance(targetPos, leftShoulder.position) / maxHandDistanceFromShoulder);
			//


			//marker.transform.position = targetPos;
			//Do IK Stuff
			
			Vector3 resultDirection = leftHand.position - constrainRobotArmMovement(ray.GetPoint(distance));
			leftHand.right = resultDirection;
			
			leftShoulder.right = -(leftHand.position - leftShoulder.position);
			
			yield return true;
		}
	}

	//OLD
	/*
	IEnumerator DragObject (float distance, Transform go)
	{
		while (Input.GetMouseButton (0))
		{
			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			//go.position = constrainRobotArmMovement(ray.GetPoint(distance));
			Vector3 targetPos = constrainRobotArmMovement(ray.GetPoint(distance));
			go.position = Vector3.Lerp(leftShoulder.position + (Vector3.down * maxElbowDistanceFromShoulder), leftShoulder.position + ((leftShoulder.position - targetPos) * maxElbowDistanceFromShoulder), Vector3.Distance(targetPos, leftShoulder.position) / maxHandDistanceFromShoulder);
			//marker.transform.position = go.position;

			//Radians from 0 to PI/2 based proportionally on distance of the hand to the shoulder over max distance from hand to shoulder
			float theta = (Mathf.PI / 2) * ((Vector3.Distance(leftHand.position - (leftHand.right * leftHand.lossyScale.x), leftShoulder.position) / maxHandDistanceFromShoulder));
			//float incr = (Mathf.PI * 2) * 0.001F;
			//theta = (theta + incr) % (Mathf.PI * 2);
			//Do IK Stuff
			//Rotation matrix
			Matrix4x4 rotationMatrix = Matrix4x4.identity;
			rotationMatrix[0,0] = Mathf.Cos(theta);	//Mathf.Cos(100); so its always positive rotation
			rotationMatrix[0,1] = -Mathf.Sin(theta);
			rotationMatrix[1,0] = Mathf.Sin(theta);
			rotationMatrix[1,1] = Mathf.Cos(theta);

			Matrix4x4 armDirectionMatrix = Matrix4x4.identity;
			//might need to use x and z
			armDirectionMatrix[0,0] = (leftShoulder.position - leftHand.position - (leftHand.right * leftHand.lossyScale.x)).x;
			armDirectionMatrix[0,1] = (leftShoulder.position - leftHand.position - (leftHand.right * leftHand.lossyScale.x)).y;

			Vector3 armDirection = new Vector3((leftShoulder.position - leftHand.position - (leftHand.right * leftHand.lossyScale.x)).x, (leftShoulder.position - leftHand.position - (leftHand.right * leftHand.lossyScale.x)).y, leftHand.right.z);

			//Get new arm direction
			//Matrix4x4 result = rotationMatrix * armDirectionMatrix;
			Vector3 resultDirection = rotationMatrix.MultiplyVector(armDirection);
			//Debug.Log(result[0,0] + "     " + result[0,1]);
			resultDirection = leftHand.position - constrainRobotArmMovement(ray.GetPoint(distance));
			
			//Debug.Log(result);
			Debug.Log(theta);
			//leftHand.right = new Vector3(result[0,0], result[0,1], leftHand.right.z);
			leftHand.right = resultDirection;
			//leftHand.right = Vector3.Distance(leftHand.position - (leftHand.right * leftHand.lossyScale.x), leftShoulder)
			//Debug.Log(Vector3.Angle(leftHand.position - leftShoulder.position, ))
			leftShoulder.right = -(leftHand.position - leftShoulder.position);
			//leftHand.right = leftHand.position - (leftShoulder.position - leftShoulder.localScale.x * leftShoulder.right);
			
			yield return true;
		}
	}
	*/

	Vector3 constrainRobotArmMovement(Vector3 proposedPosition)
	{
		Debug.DrawLine(proposedPosition, leftShoulder.position, Color.red);
		Debug.DrawRay(leftShoulder.position, leftHand.position - leftShoulder.position, Color.blue);
		Debug.DrawRay(leftHand.position, leftHand.position - (leftShoulder.position - leftShoulder.localScale.x * leftShoulder.right), Color.green);

		//if proposed position is a valid position, return it
		if(Vector3.Distance(proposedPosition, leftShoulder.position) <= maxHandDistanceFromShoulder)
			return proposedPosition;

		//else return the closest valid position to the proposed position
		else
			return leftShoulder.position + (proposedPosition - leftShoulder.position).normalized * maxHandDistanceFromShoulder;
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