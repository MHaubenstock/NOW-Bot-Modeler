using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
	private ModelAnimator modelAnimator;	
	private List<ModelAnimation> startingPositions;
	private bool isMoving = false;
	
	// Use this for initialization
	void Start ()
	{
		modelAnimator = GetComponent<ModelAnimator>();

		//Load starting positions
		startingPositions = modelAnimator.readAnimationsFromFile(Application.dataPath + "/Resources/NAOStartingPositions.txt");
		//Set returnToStartingPosition to false for each one
		foreach(ModelAnimation anim in startingPositions)
			anim.returnToStartingPosition = false;

		//Load locally created moves
		modelAnimator.animations = modelAnimator.readAnimationsFromFile();

		//Move to fighting stance
		StartCoroutine(modelAnimator.animateModel(startingPositions[1], val => isMoving = val));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI()
	{
		//Show buttons to play created animations
		modelAnimator.AnimationSelectionGUI();
	}
}
