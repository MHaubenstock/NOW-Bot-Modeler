﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//TODO: Not merging properly with the idle animation. Look into this

public class ModelAnimator : MonoBehaviour
{
	public GameObject model;
	public AudioClip[] audioClips;
	public List<ModelAnimation> animations;

	private Animator builtinAnimator;
	private AudioSource audioSource;

	private bool animationIsPlaying = false;
	private int numOfAnimationsPlaying = 0;
	private Vector3[] builtinAniPositions;
	private Quaternion[] builtinAniRotations;

	// Use this for initialization
	void Start ()
	{
		builtinAnimator = model.GetComponent<Animator>();
		audioSource = model.audio;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If an animation is playing, turn off the built-in animator
		/*
		if(numOfAnimationsPlaying > 0)
			builtinAnimator.enabled = false;
		else
			//builtinAnimator.enabled = true;
			builtinAnimator.enabled = false;
		*/
	}

	public void AnimationSelectionGUI()
	{
		
		for(int a = 0; a < animations.Count; ++a)
		{
			if(GUI.Button(new Rect(0, 101 + (31 * a), animations[a].name.Length * 8, 30), animations[a].name))
			{
				gatherTransforms();

				StartCoroutine(animateModel(animations[a], val => animationIsPlaying = val));
			}
		}
		
	}

	public void gatherTransforms()
	{
		if(numOfAnimationsPlaying == 0)
		{
			List<Vector3> pos = new List<Vector3>();
			List<Quaternion> rot = new List<Quaternion>();

			//collect all transforms
			foreach(Transform t in model.GetComponentsInChildren<Transform>())
			{
				pos.Add(t.localPosition);
				rot.Add(t.localRotation);
			}

			builtinAniPositions = pos.ToArray();
			builtinAniRotations = rot.ToArray();
		}
	}

	//takes in an array of the indices of the animations
	//Strings animations together so one plays after the other finishes
	IEnumerator stringTogetherAnimations(int[] animationIndices, float[] pauses)
	{
		bool animationPlaying = true;

		//calls animateModel on animations one by one as they finish
		for(int a = 0; a < animationIndices.Length; ++a)
		{
			//StartCoroutine(animateModel(animationIndices[a], fin => animationPlaying = fin));
			animateModel(animationIndices[a]);

			while(animationPlaying)
			{
				yield return false;
			}

			if(a < pauses.Length)
				yield return new WaitForSeconds(pauses[a]);
		}

		yield return true;
	}

	public void animateModel(int index)
	{
		gatherTransforms();

		StartCoroutine(animateModel(animations[index], val => animationIsPlaying = val));
	}

	public void animateModel(ModelAnimation animation)
	{
		gatherTransforms();

		StartCoroutine(animateModel(animation, val => animationIsPlaying = val));
	}

	//runs as a coroutine and animates the model
	public IEnumerator animateModel(ModelAnimation animation, Action<bool> playing)
	{
		playing(true);
		++numOfAnimationsPlaying;
		
		ModelAnimation anim = animation;
		float frameProgress = 0.0F;

		if(numOfAnimationsPlaying == 1)
		{
			List<Vector3> pos = new List<Vector3>();
			List<Quaternion> rot = new List<Quaternion>();

			//Gather from positions and rotations
			foreach(Transform t in anim.modelTransforms)
			{
				pos.Add(t.localPosition);
				rot.Add(t.localRotation);
			}

			//Lerp to origin frame
			while(frameProgress < 100)
			{
				for(int o = 0; o < anim.modelTransforms.Length; ++o)
				{
					//Don't move root transform because it leads to weird shifting
					if(anim.modelTransforms[o] == anim.modelTransforms[o].root)
						continue;

					anim.modelTransforms[o].localPosition = Vector3.Lerp(pos[o], anim.frames[0].positionStates[o], frameProgress / 100.0F);
					anim.modelTransforms[o].localRotation = Quaternion.Lerp(rot[o], anim.frames[0].rotationStates[o], frameProgress / 100.0F);
				}

				frameProgress += 5;

				yield return false;
			}
			
			//reset frame progress
			frameProgress = 0;
		}

		//Now animate
		//for each frame....
		for(int f = 1; f < anim.frames.Length; ++f)
		{
			Debug.Log("Playing frame " + f);
			//while not finished progressing through the frame
			while(frameProgress < 100)
			{
				//for each transform in frame
				for(int t = 0; t < anim.modelTransforms.Length; ++t)
				{
					//Don't move root transform because it leads to weird shifting
					if(anim.modelTransforms[t] == anim.modelTransforms[t].root)
						continue;

					anim.modelTransforms[t].localPosition = Vector3.Lerp(anim.frames[f - 1].positionStates[t], anim.frames[f].positionStates[t], frameProgress / 100.0F);
					anim.modelTransforms[t].localRotation = Quaternion.Lerp(anim.frames[f - 1].rotationStates[t], anim.frames[f].rotationStates[t], frameProgress / 100.0F);
				}

				if(anim.frames[f].playbackSpeed == 0)
					frameProgress += anim.playbackSpeed;
				else
					frameProgress += anim.frames[f].playbackSpeed;

				yield return false;
			}

			frameProgress = 0;
		}

		//return to position and rotation of model before starting the animation
		if(numOfAnimationsPlaying == 1 && anim.returnToStartingPosition)
		{
			Transform[] allTransforms = model.GetComponentsInChildren<Transform>();
			List<Vector3> pos = new List<Vector3>();
			List<Quaternion> rot = new List<Quaternion>();
			frameProgress = 0;

			//collect all transforms
			foreach(Transform t in model.GetComponentsInChildren<Transform>())
			{
				pos.Add(t.localPosition);
				rot.Add(t.localRotation);
			}

			while(frameProgress < 100)
			{
				for(int t = 0; t < builtinAniPositions.Length; ++t)
				{
					//Don't move root transform because it leads to weird shifting
					if(allTransforms[t] == allTransforms[t].root)
						continue;

					allTransforms[t].localPosition = Vector3.Lerp(pos[t], builtinAniPositions[t], frameProgress / 100.0F);
					allTransforms[t].localRotation = Quaternion.Lerp(rot[t], builtinAniRotations[t], frameProgress / 100.0F);
				}

				frameProgress += 5;

				yield return false;
			}
		}

		playing(false);
		--numOfAnimationsPlaying;
		return true;
	}

	public ModelAnimation mergeAnimations(ModelAnimation[] modelAnimations)
	{
		if(modelAnimations.Length == 1)
			return modelAnimations[0];

		ModelAnimation mergedAnimation = new ModelAnimation();

		//sort animations by priority then by longest length
		modelAnimations = modelAnimations.OrderBy(si => si.priority).ToArray();
		//reverse so it's ordered by descending priority
		Array.Reverse(modelAnimations);

		List<ModelAnimation> intermedAnimList = new List<ModelAnimation>();
		intermedAnimList.Add(modelAnimations[0]);
		int thePriority = modelAnimations[0].priority;

		//Now sort each priority level by total animation cycles
		List<ModelAnimation> tempAnimList = new List<ModelAnimation>();
		for(int a = 1; a < modelAnimations.Length; ++a)
		{
			if(modelAnimations[a].priority == thePriority)
				intermedAnimList.Add(modelAnimations[a]);
			else
			{
				tempAnimList.AddRange(intermedAnimList.OrderBy(si => si.getTotalCyclesInAnimation()).Reverse());
				intermedAnimList.Clear();
				intermedAnimList.Add(modelAnimations[a]);
			}
		}

		tempAnimList.AddRange(intermedAnimList.OrderBy(si => si.getTotalCyclesInAnimation()).Reverse());
		modelAnimations = tempAnimList.ToArray();
		//Done sorting

		//equalize all animations to make it easier to merge them
		for(int a = 0; a < modelAnimations.Length; ++a)
		{
			modelAnimations[a] = setAnimationPlaybackAsCloseAsPossible(modelAnimations[a], 4);
		}

		//Base the length of the merged animation off of the largest one with the highest priority
		//If no guide animation was sent into the function
		ModelAnimation guideAnimation = modelAnimations[0];

		List<Transform> allTransforms = new List<Transform>();
		List<AnimationFrame> allFrames = new List<AnimationFrame>();

		//for each frame build a new merged frame
		for(int f = 0; f < guideAnimation.frames.Length; ++f)
		{
			List<Transform> usedTransforms = new List<Transform>();
			List<Vector3> positions = new List<Vector3>();
			List<Quaternion> rotations = new List<Quaternion>();

			//for each animation to merge
			for(int a = 0; a < modelAnimations.Length; ++a)
			{
				//if it's transform hasn't been used this frame then use it because the animations are already sorted in order of usage
				for(int t = 0; t < modelAnimations[a].modelTransforms.Length; ++t)
				{
					if(!usedTransforms.Contains(modelAnimations[a].modelTransforms[t]))
					{
						//If new animation frame number has gone beyong this animation's last frame number, then it no longer contributes
						if(modelAnimations[a].frames.Length <= f)
						{
							positions.Add(modelAnimations[a].frames[modelAnimations[a].frames.Length - 1].positionStates[t]);
							rotations.Add(modelAnimations[a].frames[modelAnimations[a].frames.Length - 1].rotationStates[t]);
						}
						else
						{
							positions.Add(modelAnimations[a].frames[f].positionStates[t]);
							rotations.Add(modelAnimations[a].frames[f].rotationStates[t]);
						}

						usedTransforms.Add(modelAnimations[a].modelTransforms[t]);

						if(!allTransforms.Contains(modelAnimations[a].modelTransforms[t]))
							allTransforms.Add(modelAnimations[a].modelTransforms[t]);
					}
				}
			}

			allFrames.Add(new AnimationFrame("Frame " + f, positions.ToArray(), rotations.ToArray()));
		}

		string aniName = "";
		foreach(ModelAnimation ani in modelAnimations)
			aniName += ani.name + "-";

		mergedAnimation.name = aniName + "Merged";
		mergedAnimation.playbackSpeed = guideAnimation.playbackSpeed;
		mergedAnimation.modelTransforms = allTransforms.ToArray();
		mergedAnimation.frames = allFrames.ToArray();

		//make last frame a return to origin frame (maybe)

		return mergedAnimation;
	}

	//equalize animations and add or remove frames so that each frame takes the same number of cycles
	public ModelAnimation equalizeAnimation(ModelAnimation theAnimation, int resolution = 1)
	{
		if(resolution <= 0)
		{
			Debug.Log("Resolution should be greater than 0");
			return theAnimation;
		}

		ModelAnimation equalizedAnimation = new ModelAnimation();
		List<AnimationFrame> newFrameList = new List<AnimationFrame>();

		//initialize animation name, transforms, playback speed, and priority
		equalizedAnimation.name = theAnimation.name;
		equalizedAnimation.modelTransforms = theAnimation.modelTransforms;
		equalizedAnimation.playbackSpeed = theAnimation.getAveragePlaybackSpeed() * resolution;
		equalizedAnimation.priority = theAnimation.priority;

		int numberOfFrames = theAnimation.frames.Length * resolution;

		//Add origin frame
		AnimationFrame newFrame = new AnimationFrame(equalizedAnimation.modelTransforms.Length, "Origin Frame");
		newFrame.positionStates = theAnimation.frames[0].positionStates;
		newFrame.rotationStates = theAnimation.frames[0].rotationStates;
		newFrameList.Add(newFrame);

		for(int f = 1; f < numberOfFrames; ++f)
		{
			newFrame = new AnimationFrame(equalizedAnimation.modelTransforms.Length, "Frame " + f);
			newFrame.positionStates = theAnimation.getPositionsForPercentComplete((float)f / (numberOfFrames - 1));
			newFrame.rotationStates = theAnimation.getRotationsForPercentComplete((float)f / (numberOfFrames - 1));
			newFrameList.Add(newFrame);
		}

		equalizedAnimation.frames = newFrameList.ToArray();

		return equalizedAnimation;
	}

	public ModelAnimation stringAnimationsForNewAnimation(ModelAnimation[] modelAnimations, float[] pauses, bool skipOriginFrame, bool skipTheBackToOriginFrame)
	{
		ModelAnimation strungAnimation = new ModelAnimation();
		List<Transform> allTransforms = new List<Transform>();
		List<AnimationFrame> allFrames = new List<AnimationFrame>();
		AnimationFrame newFrame;
		string theName = "";

		//get transforms for all animations and initialize strungAnimation
		foreach(ModelAnimation ani in modelAnimations)
		{
			theName += ani.name + "-";

			foreach(Transform t in ani.modelTransforms)
				if(!(allTransforms.Contains(t)))
					allTransforms.Add(t);
		}

		strungAnimation.name = theName + "String";
		strungAnimation.modelTransforms = allTransforms.ToArray();
		strungAnimation.playbackSpeed = 1;
		strungAnimation.priority = 0;

		//for each animation...
		for(int a = 0; a < modelAnimations.Length; ++a)
		{
			//Put this animation's transformsinto a list for easy checking
			List<Transform> thisAnimationTransforms = modelAnimations[a].modelTransforms.ToList();

			//for each frame...
			for(int f = (a > 0 && skipOriginFrame ? 1 : 0); f < modelAnimations[a].frames.Length + (skipTheBackToOriginFrame ? -1 : 0); ++f)
			{
				newFrame = new AnimationFrame(allTransforms.Count, "Frame " + allFrames.Count);
				newFrame.playbackSpeed = modelAnimations[a].frames[f].playbackSpeed == 0 ? 0 : modelAnimations[a].frames[f].playbackSpeed;

				//for each transform...
				for(int t = 0; t < allTransforms.Count; ++t)
				{
					//if current animation contains this transform
					if(thisAnimationTransforms.Contains(allTransforms[t]))
					{
						//set position and rotation for this frame
						newFrame.positionStates[t] = modelAnimations[a].frames[f].positionStates[thisAnimationTransforms.IndexOf(allTransforms[t])];
						newFrame.rotationStates[t] = modelAnimations[a].frames[f].rotationStates[thisAnimationTransforms.IndexOf(allTransforms[t])];
					}
					else
					{
						//else set position and rotation for this transform to the same position and rotation as last frame
						
						//If this is the first frame and you reach a transform not used in this animation...
						if(allFrames.Count == 0)
						{
							//Check each animation after the first one to see if the transform is contained in it
							//if it is then use that animations first frame position state for this position state, same for rotation
							for(int an = 1; an < modelAnimations.Length; ++an)
							{
								List<Transform> otherAnimationTransforms = modelAnimations[an].modelTransforms.ToList();	
								
								if(otherAnimationTransforms.Contains(allTransforms[t]))
								{
									newFrame.positionStates[t] = modelAnimations[an].frames[0].positionStates[otherAnimationTransforms.IndexOf(allTransforms[t])];
									newFrame.rotationStates[t] = modelAnimations[an].frames[0].rotationStates[otherAnimationTransforms.IndexOf(allTransforms[t])];

									break;
								}
							}							
						}
						else
						{
							newFrame.positionStates[t] = allFrames[allFrames.Count - 1].positionStates[t];
							newFrame.rotationStates[t] = allFrames[allFrames.Count - 1].rotationStates[t];
						}
					}
				}

				//add frame to allFrames
				allFrames.Add(newFrame);
			}

			//Add the pause if there is one
			if(a < pauses.Length)
			{
				newFrame = new AnimationFrame(allTransforms.Count, "Frame " + allFrames.Count);
				newFrame.playbackSpeed = (100 / pauses[a]) * 0.0025F;

				//Set position and rotation equal to position and rotation of last frame
				newFrame.positionStates = allFrames[allFrames.Count - 1].positionStates;
				newFrame.rotationStates = allFrames[allFrames.Count - 1].rotationStates;

				allFrames.Add(newFrame);
			}
		}

		//Finalize strung animations
		strungAnimation.frames = allFrames.ToArray();

		//equalize the animation
		//strungAnimation = equalizeAnimation(strungAnimation, 3);

		return strungAnimation;
	}

	//Sets the animations playback speed as close as possible to the desired playback speed
	public ModelAnimation setAnimationPlaybackAsCloseAsPossible(ModelAnimation theAnimation, float pBackSpeed)
	{
		if(pBackSpeed <= 0)
		{
			Debug.Log("Resolution should be greater than 0");
			return theAnimation;
		}

		ModelAnimation equalizedAnimation = new ModelAnimation();
		List<AnimationFrame> newFrameList = new List<AnimationFrame>();

		float averageAnimationFrameLength = 100 / theAnimation.getAveragePlaybackSpeed();

		//get closest float that divides evenly into animation length, rounds down and increases number of frames, gets better resolut
		float actualPbackSpeed = pBackSpeed - ((Mathf.CeilToInt(averageAnimationFrameLength) % (int)pBackSpeed) / 2);
		int numberOfFrames = (int)(averageAnimationFrameLength / actualPbackSpeed);

		//initialize animation name, transforms, playback speed, and priority
		equalizedAnimation.name = theAnimation.name;
		equalizedAnimation.modelTransforms = theAnimation.modelTransforms;
		equalizedAnimation.playbackSpeed = actualPbackSpeed;
		equalizedAnimation.priority = theAnimation.priority;

		//Add origin frame
		AnimationFrame newFrame = new AnimationFrame(equalizedAnimation.modelTransforms.Length, "Origin Frame");
		newFrame.positionStates = theAnimation.frames[0].positionStates;
		newFrame.rotationStates = theAnimation.frames[0].rotationStates;
		newFrameList.Add(newFrame);

		for(int f = 1; f < numberOfFrames; ++f)
		{
			newFrame = new AnimationFrame(equalizedAnimation.modelTransforms.Length, "Frame " + f);
			newFrame.positionStates = theAnimation.getPositionsForPercentComplete((float)f / (numberOfFrames - 1));
			newFrame.rotationStates = theAnimation.getRotationsForPercentComplete((float)f / (numberOfFrames - 1));
			newFrameList.Add(newFrame);
		}

		equalizedAnimation.frames = newFrameList.ToArray();

		return equalizedAnimation;
	}

	//For saving the animation
	public string serializeAnimations()
	{
		string serializedAnimations = "";

		//serialize number of animations
		serializedAnimations += animations.Count + "\n";

		//for each animation
		foreach(ModelAnimation anim in animations)
		{
			serializedAnimations += anim.serializeAnimation() + "\n";
		}

		return serializedAnimations;
	}

	public bool saveAnimations(string serializedAnimations)
	{
		using(StreamWriter outfile = new StreamWriter(Application.dataPath + "/Resources/NAOAnimations.txt"))
		//using(StreamWriter outfile = new StreamWriter("Assets/Resources/NAOAnimations.txt"))
		//using(StreamWriter outfile = new StreamWriter("Assets/Resources/NAOStartingPositions.txt"))
        {
           	outfile.Write(serializedAnimations);
        }

        return true;
	}

	public List<ModelAnimation> readAnimationsFromFile()
	{
		return readAnimationsFromFile(Application.dataPath + "/Resources/NAOAnimations.txt");
		//return readAnimationsFromFile("Assets/Resources/NAOAnimations.txt");
	}

	public List<ModelAnimation> readAnimationsFromFile(String fileName)
	{
		StreamReader inFile = new StreamReader(fileName);

		//read number of animations
		int numOfAnimations = int.Parse(inFile.ReadLine());
		List<ModelAnimation> animations = new List<ModelAnimation>();

		//split animations into separate strings
		for(int a = 0; a < numOfAnimations; ++a)
        {
        	string animationString = "";
        	string fileLine;
        	bool readingFrame = false;

        	//read all animation data into string then send to ModelAnimation to return the animation
        	//read until you encounter an empty line
        	while((fileLine = inFile.ReadLine()) != "")
        	{
        		if(fileLine == "beginframe")
        		{
        			readingFrame = true;
        			continue;
        		}
        		else if(fileLine == "endframe")
        		{
        			readingFrame = false;
        			//ended frame, skip to next line
        			animationString += "\n";
        			continue;
        		}

        		animationString += fileLine + (readingFrame ? "\r" : "\n");
        	}

        	//send to ModelAnimation to return an animation
        	animations.Add(ModelAnimation.deserializeAnimation(animationString, model));

        	animationString = "";
        }

        //Finished creating animations from file, now assign it to our list of animations
        //this.animations = animations;
        return animations;

		//return true;
	}
}

[System.Serializable]
public class ModelAnimation
{
	public string name;
	public Transform [] modelTransforms;
	public AnimationFrame [] frames;
	public float playbackSpeed = 1;
	public int priority = 0;
	public bool returnToStartingPosition = true;

	public ModelAnimation(){}

	//For saving the animation
	public string serializeAnimation()
	{
		string serializedAnimation = "";

		//insert beginning of animation indicator
		//serializedAnimation += "beginanimation\n";

		//serialize name
		serializedAnimation += name + "\n";

		//serialize playback speed
		serializedAnimation += playbackSpeed + "\n";

		//Number of transforms
		serializedAnimation += modelTransforms.Length + "\n";

		//serialize transform hierarchies
		foreach(Transform tr in modelTransforms)
		{
			serializedAnimation += getHierarchy(tr) + "\n";
		}

		//Number of frames
		serializedAnimation += frames.Length + "\n";

		//serialize frames
		foreach(AnimationFrame anim in frames)
		{
			serializedAnimation += anim.serializeFrame();
		}

		//insert ending animation indicator
		//serializedAnimation += "endanimation\n";

		return serializedAnimation;
	}

	public static ModelAnimation deserializeAnimation(string serAnim, GameObject animModel)
	{
		ModelAnimation anim = new ModelAnimation();
		Queue<string> animData = new Queue<string>(serAnim.Split('\n'));

		//Store animation name
		anim.name = animData.Dequeue();

		//Store animation playback speed
		anim.playbackSpeed = float.Parse(animData.Dequeue());

		//Create array for transform of length of number of transforms
		Transform [] transforms = new Transform[int.Parse(animData.Dequeue())];

		//Load model transforms into transform array
		for(int t = 0; t < transforms.Length; ++t)
		{
			string transName = animData.Dequeue();

			if(transName == animModel.transform.root.name)
				transforms[t] = animModel.transform.root;
			else
				transforms[t] = animModel.transform.Find(transName);
		}

		//set animation transforms
		anim.modelTransforms = transforms;

		//Next dequeue gets number of animations
		anim.frames = new AnimationFrame[int.Parse(animData.Dequeue())];

		//Get animation frames
		for(int a = 0; a < anim.frames.Length; ++a)
		{
			anim.frames[a] = AnimationFrame.deserializeFrame(animData.Dequeue());
		}

		return anim;
	}

	//Recursively builds the transforms hierarchy
	string getHierarchy(Transform tr)
	{
		if(tr == tr.root)
			return tr.name;

		string hierarchy = tr.name;

		while(tr.parent != tr.root)
		{
			tr = tr.parent;

			hierarchy = tr.name + "/" + hierarchy;
		}

		return hierarchy;

		/*
		//base case
		if(tr == tr.root)
			return "";

		return getHierarchy(tr.parent) + (tr.parent == tr.root ? "" : "/") + tr.name;
		*/
	}

	public int getCyclesForFrame(int frameIndex)
	{
		return (frames[frameIndex].playbackSpeed <= 0) ? (int)(100 / playbackSpeed) : (int)(100 / frames[frameIndex].playbackSpeed);
	}

	public int getTotalCyclesInAnimation()
	{
		int totalCycles = 0;

		//Exclude the origin frame
		for(int a = 1; a < frames.Length; ++a)
		{
			if(frames[a].playbackSpeed > 0)
				totalCycles += (int)(100 / frames[a].playbackSpeed);
			else
				totalCycles += (int)(100 / playbackSpeed);
		}

		return totalCycles;
	}

	public float getAveragePlaybackSpeed()
	{
		float totalPlayback = 0;

		//Exclude the origin frame
		for(int a = 1; a < frames.Length; ++a)
		{
			if(frames[a].playbackSpeed > 0)
				totalPlayback += frames[a].playbackSpeed;
			else
				totalPlayback += playbackSpeed;
		}

		return totalPlayback / (frames.Length - 1);
	}

	public Vector3[] getPositionsForPercentComplete(float percentComplete01)
	{
		if(percentComplete01 < 0 || percentComplete01 > 1)
		{
			Debug.Log("Percent Complete should be between 0 and 1");
			return new Vector3[modelTransforms.Length];
		}

		Vector3[] posStates = new Vector3[modelTransforms.Length];
		int totalCycles = getTotalCyclesInAnimation();
		int lastFrameEndTime = 0;
		int thisFrameEndTime;

		for(int f = 1; f < frames.Length; ++f)
		{
			thisFrameEndTime = lastFrameEndTime + getCyclesForFrame(f);
			float percentThrough = (float)thisFrameEndTime / totalCycles;

			//The correct position is in this frame
			if(percentComplete01 <= percentThrough)
			{
				for(int p = 0; p < posStates.Length; ++p)
				{
					posStates[p] = Vector3.Lerp(frames[f - 1].positionStates[p], frames[f].positionStates[p], (percentComplete01 - ((float)lastFrameEndTime / totalCycles)) / (((float)thisFrameEndTime / totalCycles) - ((float)lastFrameEndTime / totalCycles)));
				}

				break;
			}

			lastFrameEndTime = thisFrameEndTime;
		}

		return posStates;
	}

	public Quaternion[] getRotationsForPercentComplete(float percentComplete01)
	{
		if(percentComplete01 < 0 || percentComplete01 > 1)
		{
			Debug.Log("Percent Complete should be between 0 and 1");
			return new Quaternion[modelTransforms.Length];
		}

		Quaternion[] rotStates = new Quaternion[modelTransforms.Length];
		int totalCycles = getTotalCyclesInAnimation();
		int lastFrameEndTime = 0;
		int thisFrameEndTime;

		for(int f = 1; f < frames.Length; ++f)
		{
			thisFrameEndTime = lastFrameEndTime + getCyclesForFrame(f);
			float percentThrough = (float)thisFrameEndTime / totalCycles;

			//The correct position is in this frame
			if(percentComplete01 <= percentThrough)
			{
				for(int p = 0; p < rotStates.Length; ++p)
				{
					rotStates[p] = Quaternion.Lerp(frames[f - 1].rotationStates[p], frames[f].rotationStates[p], (percentComplete01 - ((float)lastFrameEndTime / totalCycles)) / (((float)thisFrameEndTime / totalCycles) - ((float)lastFrameEndTime / totalCycles)));
				}

				break;
			}

			lastFrameEndTime = thisFrameEndTime;
		}

		return rotStates;
	}
}

[System.Serializable]
public class AnimationFrame
{
	public string frameName;
	public Vector3 [] positionStates;
	public Quaternion [] rotationStates;
	public float playbackSpeed = 0;

	public AnimationFrame(){}

	public AnimationFrame(int numOfTransforms, string name)
	{
		frameName = "Frame: " + name;
		positionStates = new Vector3[numOfTransforms];
		rotationStates = new Quaternion[numOfTransforms];
	}

	public AnimationFrame(string name, Vector3[] positions, Quaternion[] rotations)
	{
		frameName = "Frame: " + name;
		positionStates = positions;
		rotationStates = rotations;
	}

	//For saving the frame
	public string serializeFrame()
	{
		string serializedFrame = "";

		//insert beginning frame indicator
		serializedFrame += "beginframe\n";

		//serialize frame name
		serializedFrame += frameName + "\n";

		//Number of position states
		serializedFrame += positionStates.Length + "\n";

		//serialize position states
		foreach(Vector3 pos in positionStates)
		{
			serializedFrame += pos.x + " " + pos.y + " " + pos.z + "\n";
		}

		//Number of rotation states
		serializedFrame += rotationStates.Length + "\n";

		//serialize rotation states
		foreach(Quaternion rot in rotationStates)
		{
			serializedFrame += rot.x + " " + rot.y + " " + rot.z + " " + rot.w + "\n";
		}

		//serialize playback speed
		serializedFrame += playbackSpeed + "\n";

		//insert ending frame indicator
		serializedFrame += "endframe\n";

		return serializedFrame;
	}

	public static AnimationFrame deserializeFrame(string serFrame)
	{
		AnimationFrame frame = new AnimationFrame();
		//Has an extra empty line
		string [] data = serFrame.Split('\r');
		//This constructs a queue with the empty line removed
		Queue<string> frameData = new Queue<string>(data.Take(data.Length - 1));

		//Store frame name
		frame.frameName = frameData.Dequeue();

		//Create array of positions (Vector3s)
		frame.positionStates = new Vector3[int.Parse(frameData.Dequeue())];

		//Store position states
		for(int p = 0; p < frame.positionStates.Length; ++p)
		{
			string [] pos = frameData.Dequeue().Split(' ');

			frame.positionStates[p].x = float.Parse(pos[0]);
			frame.positionStates[p].y = float.Parse(pos[1]);
			frame.positionStates[p].z = float.Parse(pos[2]);
		}

		//Create array of rotations (Quaternions)
		frame.rotationStates = new Quaternion[int.Parse(frameData.Dequeue())];

		//Store rotation states
		for(int r = 0; r < frame.rotationStates.Length; ++r)
		{
			string [] rots = frameData.Dequeue().Split(' ');

			frame.rotationStates[r].x = float.Parse(rots[0]);
			frame.rotationStates[r].y = float.Parse(rots[1]);
			frame.rotationStates[r].z = float.Parse(rots[2]);
			frame.rotationStates[r].w = float.Parse(rots[3]);
		}

		//Store animation playback speed
		frame.playbackSpeed = float.Parse(frameData.Dequeue());

		return frame;
	}
}

public class ModelAnimationRaw
{
	public string animationName;
	public Transform[] modelTransforms;
	public List<AnimationFrameRaw> frames = new List<AnimationFrameRaw>();
	public int frameNumber = 1;
	public bool includeOriginFrame = false;
	public bool includeUnusedTransforms = false;

	ModelAnimationRaw(){}

	public ModelAnimationRaw(string name, Transform[] points)
	{
		animationName = name;
		modelTransforms = points;
		addState(getState());
	}

	public AnimationFrameRaw getState()
	{
		AnimationFrameRaw tempFrame = new AnimationFrameRaw(modelTransforms.Length, animationName + "_Frame_" + frameNumber.ToString());

		//Iterate through each transform in the model and gather its position and rotation
		for(int t = 0; t < modelTransforms.Length; ++t)
		{
			tempFrame.theTransforms[t] = modelTransforms[t];
			tempFrame.positionStates[t] = modelTransforms[t].localPosition;
			tempFrame.rotationStates[t] = modelTransforms[t].localRotation;
		}

		return tempFrame;
	}

	public void addState(AnimationFrameRaw state)
	{
		//Add frame to array of frames
		frames.Add(state);
		++frameNumber;
	}

	public ModelAnimation processFinishedAnimation()
	{
		//Placeholders
		List<Transform> transformArr = new List<Transform>();
		List<Vector3> positionArr = new List<Vector3>();
		List<Quaternion> rotationArr = new List<Quaternion>();
		bool usedATransform = false;

		ModelAnimation tempAnimation = new ModelAnimation();
		tempAnimation.name = animationName;
		Debug.Log(frames.Count - (includeOriginFrame ? 0 : 1));
		tempAnimation.frames = new AnimationFrame[frames.Count - (includeOriginFrame ? 0 : 1)];

		Debug.Log("Start Processing");
		//keep list of bools for tranforms that change throughout then use it at the end
		//to create a starting state as the first frame
		bool[] usedTransforms = new bool[modelTransforms.Length];

		//Get used transforms
		for(int ua = 1; ua < frames.Count; ++ua)
		{
			//compare this frame to frame a-1, keep only transforms and vector 3's that change
			for(int uf = 0; uf < frames[ua].theTransforms.Length; ++uf)
			{
				if((frames[ua].positionStates[uf] != frames[ua - 1].positionStates[uf]) || (frames[ua].rotationStates[uf] != frames[ua - 1].rotationStates[uf]))
				{
					usedTransforms[uf] = true;
					usedATransform = true;
				}
			}
		}

		int usedFrameNum;

		//for each frame in the working animation
		for(int a = 1 - (includeOriginFrame ? 0 : 1); a < frames.Count - (includeOriginFrame ? 0 : 1); ++a)
		{
			usedFrameNum = a + (includeOriginFrame ? 0 : 1);

			Debug.Log("Process frame: " + a);
			//Build the frame from the working animation
			for(int f = 0; f < frames[usedFrameNum].theTransforms.Length; ++f)
			{
				if(includeUnusedTransforms || usedTransforms[f] || !usedATransform)
				{
					//add to array of used transforms for this frame
					transformArr.Add(frames[usedFrameNum].theTransforms[f]);
					positionArr.Add(frames[usedFrameNum].positionStates[f]);
					rotationArr.Add(frames[usedFrameNum].rotationStates[f]);
				}
			}

			//Finish processing frame
			tempAnimation.frames[a] = new AnimationFrame(transformArr.Count, "Frame " + usedFrameNum);
			tempAnimation.modelTransforms = transformArr.ToArray() as Transform[];
			tempAnimation.frames[a].positionStates = positionArr.ToArray() as Vector3[];
			tempAnimation.frames[a].rotationStates = rotationArr.ToArray() as Quaternion[];

			transformArr.Clear();
			positionArr.Clear();
			rotationArr.Clear();
		}

		if(includeOriginFrame)
		{
			//process first frame as a starting state
			for(int x = 0; x < usedTransforms.Length; ++x)
			{
				if(usedTransforms[x] || !usedATransform)
				{
					transformArr.Add(frames[0].theTransforms[x]);
					positionArr.Add(frames[0].positionStates[x]);
					rotationArr.Add(frames[0].rotationStates[x]);
				}
			}

			tempAnimation.frames[0] = new AnimationFrame(transformArr.Count, "Origin Frame");
			tempAnimation.modelTransforms = transformArr.ToArray() as Transform[];
			tempAnimation.frames[0].positionStates = positionArr.ToArray() as Vector3[];
			tempAnimation.frames[0].rotationStates = rotationArr.ToArray() as Quaternion[];
		}

		//modelAnimator.animations.Add(tempAnimation);
		return tempAnimation;
		Debug.Log("Done Processing");
	}
}

public class AnimationFrameRaw
{
	public string frameName;
	public Transform[] theTransforms;
	public Vector3[] positionStates;
	public Quaternion[] rotationStates;

	public AnimationFrameRaw(int numOfTransforms, string name)
	{
		frameName = name;
		theTransforms = new Transform[numOfTransforms];
		positionStates = new Vector3[numOfTransforms];
		rotationStates = new Quaternion[numOfTransforms];
	}
}