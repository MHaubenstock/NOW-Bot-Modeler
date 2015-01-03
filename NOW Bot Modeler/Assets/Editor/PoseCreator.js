import ModelAnimator;

class PoseCreator extends EditorWindow
{
	var modelAnimator : ModelAnimator;
	var workingAnimation : ModelAnimationRaw;
	var animationName : String = "";
	var includeOriginFrame : boolean = false;
	var includeUnusedTransforms : boolean = false;
	var animAndFrameToJumpTo : Vector2;

	// Add menu named "Pose Creator" to the Window menu
	@MenuItem ("Window/Pose Creator")
	static function Init ()
	{
		// Get existing open window or if none, make a new one:		
		var window = ScriptableObject.CreateInstance.<PoseCreator>();
		window.position = Rect(0, 0, 250, 80);
		window.Show();
	}

	function OnGUI()
	{
		addSpace(3);

		EditorGUILayout.PrefixLabel("Model Poser Script");
		modelAnimator = EditorGUILayout.ObjectField(modelAnimator, ModelAnimator, true);

		//If there is no Model Animator attached, then don't show the rest
		if(!modelAnimator)
			return;

		addSpace(2);

		if(!workingAnimation)
		{
			EditorGUILayout.PrefixLabel("Animation Name:");
			animationName = GUILayout.TextField(animationName);

			if(GUILayout.Button("Begin New Animation"))
			{
				//Creates new animation and sets begin state
				workingAnimation = new ModelAnimationRaw(animationName, modelAnimator.model.GetComponentsInChildren.<Transform>());
			}
		}
		else
		{
			EditorGUILayout.LabelField(animationName);

			addSpace(1);

			if(GUILayout.Button("Save State"))
			{
				getState();
				Debug.Log(workingAnimation.frames.length);
			}

			addSpace(1);

			includeOriginFrame = EditorGUILayout.Toggle("Include Origin Frame", includeOriginFrame);
			includeUnusedTransforms = EditorGUILayout.Toggle("Include Unused Transforms", includeUnusedTransforms);

			addSpace(1);

			if(GUILayout.Button("Finsh & Process"))
			{
				processFinishedAnimation();
			}

			//Information
			for(var fr : AnimationFrameRaw in workingAnimation.frames.ToBuiltin(AnimationFrameRaw) as AnimationFrameRaw[])
			{
				EditorGUILayout.LabelField(fr.frameName);
			}
		}

		addSpace(3);

		animAndFrameToJumpTo = EditorGUILayout.Vector2Field("Animation # and Frame #", animAndFrameToJumpTo);
		if(GUILayout.Button("Jump to Frame State"))
		{
			var theFrame : AnimationFrame = modelAnimator.animations[animAndFrameToJumpTo.x].frames[animAndFrameToJumpTo.y];

			for(var t : int = 0; t < modelAnimator.animations[animAndFrameToJumpTo.x].modelTransforms.length; ++t)
			{
				modelAnimator.animations[animAndFrameToJumpTo.x].modelTransforms[t].localPosition = theFrame.positionStates[t];
				modelAnimator.animations[animAndFrameToJumpTo.x].modelTransforms[t].localRotation = theFrame.rotationStates[t];
			}
		}

		addSpace(2);

		//Reset
		if(GUILayout.Button("Reset"))
			reset();

		/*
		if(GUILayout.Button("Smooth Recent Animation"))
		{
			//modelAnimator.animations.Add(modelAnimator.equalizeAnimation(modelAnimator.animations[6], 1));
			//modelAnimator.animations.Add(modelAnimator.setAnimationPlaybackAsCloseAsPossible(modelAnimator.animations[4], 4));
			modelAnimator.animations.RemoveAt(2);
		}

		if(GUILayout.Button("Merge Animations"))
		{
			var modTest : ModelAnimation[] = [modelAnimator.animations[0], modelAnimator.animations[2]];
			modelAnimator.animations.Add(modelAnimator.mergeAnimations(modTest));
		}

		if(GUILayout.Button("String Together Animations"))
		{
			//var animations : ModelAnimation[] = [modelAnimator.animations[0], modelAnimator.animations[0], modelAnimator.animations[5], modelAnimator.animations[0], modelAnimator.animations[5]];
			var animations : ModelAnimation[] = [modelAnimator.animations[0]];
			var pauses : float[] = [];

			modelAnimator.animations.Add(modelAnimator.stringAnimationsForNewAnimation(animations, pauses, true, false));
		}
		*/

		if(GUILayout.Button("Save All Animations"))
		{
			//get serialized animations and save them to a file
			var serializedAnimations : String = modelAnimator.serializeAnimations();
			modelAnimator.saveAnimations(serializedAnimations);			
		}

		if(GUILayout.Button("Load All Animations"))
			modelAnimator.animations = modelAnimator.readAnimationsFromFile();

	}

	function addSpace(spaces : int)
	{
		for(var x : int = 0; x < spaces; ++x)
			EditorGUILayout.Space();
	}

	function getState()
	{
		if(workingAnimation)
			workingAnimation.getState();
	}

	function processFinishedAnimation()
	{
		//Placeholders
		var transformArr : Array = new Array();
		var positionArr : Array = new Array();
		var rotationArr : Array = new Array();
		var usedATransform : boolean = false;

		var tempAnimation : ModelAnimation = new ModelAnimation();
		tempAnimation.name = animationName;
		Debug.Log(workingAnimation.frames.length - (includeOriginFrame ? 0 : 1));
		tempAnimation.frames = new AnimationFrame[workingAnimation.frames.length - (includeOriginFrame ? 0 : 1)];

		Debug.Log("Start Processing");
		//keep list of bools for tranforms that change throughout then use it at the end
		//to create a starting state as the first frame
		var usedTransforms : boolean[] = new boolean[workingAnimation.modelTransforms.length];

		//Get used transforms
		for(var ua : int = 1; ua < workingAnimation.frames.length; ++ua)
		{
			//compare this frame to frame a-1, keep only transforms and vector 3's that change
			for(var uf : int = 0; uf < workingAnimation.frames[ua].theTransforms.length; ++uf)
			{
				if((workingAnimation.frames[ua].positionStates[uf] != workingAnimation.frames[ua - 1].positionStates[uf]) || (workingAnimation.frames[ua].rotationStates[uf] != workingAnimation.frames[ua - 1].rotationStates[uf]))
				{
					usedTransforms[uf] = true;
					usedATransform = true;
				}
			}
		}

		//for each frame in the working animation
		for(var a : int = 1 - (includeOriginFrame ? 0 : 1); a < workingAnimation.frames.length - (includeOriginFrame ? 0 : 1); ++a)
		{
			usedFrameNum = a  + (includeOriginFrame ? 0 : 1);

			Debug.Log("Process frame: " + a);
			//Build the frame from the working animation
			for(var f : int = 0; f < workingAnimation.frames[usedFrameNum].theTransforms.length; ++f)
			{
				if(includeUnusedTransforms || usedTransforms[f] || !usedATransform)
				{
					//add to array of used transforms for this frame
					transformArr.Add(workingAnimation.frames[usedFrameNum].theTransforms[f]);
					positionArr.Add(workingAnimation.frames[usedFrameNum].positionStates[f]);
					rotationArr.Add(workingAnimation.frames[usedFrameNum].rotationStates[f]);
				}
			}

			//Finish processing frame
			tempAnimation.frames[a] = new AnimationFrame(transformArr.length, "Frame " + usedFrameNum);
			tempAnimation.modelTransforms = transformArr.ToBuiltin(Transform);
			tempAnimation.frames[a].positionStates = positionArr.ToBuiltin(Vector3);
			tempAnimation.frames[a].rotationStates = rotationArr.ToBuiltin(Quaternion);

			transformArr.Clear();
			positionArr.Clear();
			rotationArr.Clear();
		}

		if(includeOriginFrame)
		{
			//process first frame as a starting state
			for(var x : int = 0; x < usedTransforms.length; ++x)
			{
				if(usedTransforms[x] || !usedATransform)
				{
					transformArr.Add(workingAnimation.frames[0].theTransforms[x]);
					positionArr.Add(workingAnimation.frames[0].positionStates[x]);
					rotationArr.Add(workingAnimation.frames[0].rotationStates[x]);
				}
			}

			tempAnimation.frames[0] = new AnimationFrame(transformArr.length, "Origin Frame");
			tempAnimation.modelTransforms = transformArr.ToBuiltin(Transform);
			tempAnimation.frames[0].positionStates = positionArr.ToBuiltin(Vector3);
			tempAnimation.frames[0].rotationStates = rotationArr.ToBuiltin(Quaternion);
		}

		modelAnimator.animations.Add(tempAnimation);
		Debug.Log("Done Processing");
	}

	function reset()
	{
		workingAnimation = null;
	}
}