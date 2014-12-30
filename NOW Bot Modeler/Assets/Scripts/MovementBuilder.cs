using UnityEngine;
using System.Collections;

public class MovementBuilder : MonoBehaviour
{
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
	public Transform leftHand;	//For measuring, so no need for right hand
	public Transform leftShoulder;
	public Transform rightShoulder;

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
			int layerMask = 1 << 12;	//DraggableLayer

			if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
			{
				// We need to hit a rigidbody that is not kinematic
				if (hit.transform.tag == "DraggableArm")
				{
					if(hit.transform.localPosition.y < 0)	//If it is the left arm
						StartCoroutine (DragArm(hit.distance, hit.transform, 1 << 8));
					else
						StartCoroutine (DragArm(hit.distance, hit.transform, 1 << 9));
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
		Transform shoulder = (layerMask == 1 << 8 ? leftShoulder : rightShoulder);
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
			hand.position += ((hand.position - shoulder.position).normalized * shoulder.lossyScale.x * .8F) - (hand.position - shoulder.position);
			
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

	static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;

		if (angle > 360)
			angle -= 360;

		return Mathf.Clamp (angle, min, max);
	}
}