using UnityEngine;
using System.Collections;

public class ToppleHandler : MonoBehaviour
{
	public float correctionPower = 10;

	// Use this for initialization
	void Start ()
	{
		rigidbody.centerOfMass = transform.position - Vector3.up * 10;
		//rigidbody.centerOfMass = new Vector3(transform.position.x, - 20, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawRay(transform.position, transform.up * 10, Color.blue);
		
		
		//Get angle off of completely vertical
		float angleBetween = Vector3.Angle(transform.TransformPoint(transform.up), Vector3.up);
		/*
		Debug.Log(angleBetween);
		
		//If angle is close to 180 then the bot fell over and you lose a point
		*/


		//Apply rotational force to self-correct the NAO bot's orientation
		float xCorrection = Mathf.Min(360 - transform.rotation.x, transform.rotation.x);
		float zCorrection = Mathf.Min(360 - transform.rotation.z, transform.rotation.z);

		//transform.rigidbody.AddTorque(new Vector3(xCorrection, 0, zCorrection) * (angleBetween / 180) * correctionPower, ForceMode.Acceleration);
		Debug.DrawRay(transform.position + transform.up * 2, (new Vector3(xCorrection, 0, zCorrection)).normalized * 4, Color.red);
	}
}
