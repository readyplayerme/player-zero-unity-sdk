using System.Collections.Generic;
using UnityEngine;

namespace ReadyPlayerMe.AvatarLoader
{
	public class SkeletonBuilder
	{
		//A static dictionary containing the mapping from joint/bones names of standard Ready Player Me Avatars
		//in the modelto the names Unity uses for them internally.
		//In this case they match the naming from the included Mixamo model on the left
		//and the Unity equivalent name on the right. 
		//This does not need to be hard-coded. 
		private Dictionary<string, string> DefaultBoneNames = new Dictionary<string, string>()
		{
			{ "Spine1", "Chest" },
			{ "Head", "Head" },
			{ "Hips", "Hips" },
			{ "LeftHandIndex3", "Left Index Distal" },
			{ "LeftHandIndex2", "Left Index Intermediate" },
			{ "LeftHandIndex1", "Left Index Proximal" },
			{ "LeftHandPinky3", "Left Little Distal" },
			{ "LeftHandPinky2", "Left Little Intermediate" },
			{ "LeftHandPinky1", "Left Little Proximal" },
			{ "LeftHandMiddle3", "Left Middle Distal" },
			{ "LeftHandMiddle2", "Left Middle Intermediate" },
			{ "LeftHandMiddle1", "Left Middle Proximal" },
			{ "LeftHandRing3", "Left Ring Distal" },
			{ "LeftHandRing2", "Left Ring Intermediate" },
			{ "LeftHandRing1", "Left Ring Proximal" },
			{ "LeftHandThumb3", "Left Thumb Distal" },
			{ "LeftHandThumb2", "Left Thumb Intermediate" },
			{ "LeftHandThumb1", "Left Thumb Proximal" },
			{ "LeftFoot", "LeftFoot" },
			{ "LeftHand", "LeftHand" },
			{ "LeftForeArm", "LeftLowerArm" },
			{ "LeftLeg", "LeftLowerLeg" },
			{ "LeftShoulder", "LeftShoulder" },
			{ "LeftToeBase", "LeftToes" },
			{ "LeftArm", "LeftUpperArm" },
			{ "LeftUpLeg", "LeftUpperLeg" },
			{ "Neck", "Neck" },
			{ "RightHandIndex3", "Right Index Distal" },
			{ "RightHandIndex2", "Right Index Intermediate" },
			{ "RightHandIndex1", "Right Index Proximal" },
			{ "RightHandPinky3", "Right Little Distal" },
			{ "RightHandPinky2", "Right Little Intermediate" },
			{ "RightHandPinky1", "Right Little Proximal" },
			{ "RightHandMiddle3", "Right Middle Distal" },
			{ "RightHandMiddle2", "Right Middle Intermediate" },
			{ "RightHandMiddle1", "Right Middle Proximal" },
			{ "RightHandRing3", "Right Ring Distal" },
			{ "RightHandRing2", "Right Ring Intermediate" },
			{ "RightHandRing1", "Right Ring Proximal" },
			{ "RightHandThumb3", "Right Thumb Distal" },
			{ "RightHandThumb2", "Right Thumb Intermediate" },
			{ "RightHandThumb1", "Right Thumb Proximal" },
			{ "RightFoot", "RightFoot" },
			{ "RightHand", "RightHand" },
			{ "RightForeArm", "RightLowerArm" },
			{ "RightLeg", "RightLowerLeg" },
			{ "RightShoulder", "RightShoulder" },
			{ "RightToeBase", "RightToes" },
			{ "RightArm", "RightUpperArm" },
			{ "RightUpLeg", "RightUpperLeg" },
			{ "Spine", "Spine" },
			{ "Spine2", "UpperChest" }
		};
		
		/// <summary>
		/// Create a HumanDescription out of an avatar GameObject.
		/// The HumanDescription is what is needed to create an Avatar object
		/// using the AvatarBuilder API. This function takes care of
		/// creating the HumanDescription by going through the avatar's
		/// hierarchy, defining its T-Pose in the skeleton, and defining
		/// the transform/bone mapping in the HumanBone array.
		/// </summary>
		/// <param name="avatarRoot">Root of your avatar object</param>
		/// <returns>A HumanDescription which can be fed to the AvatarBuilder API</returns>
		public HumanDescription CreateHumanDescription(GameObject avatarRoot, Dictionary<string, string> bonesNames = null)
		{
			HumanDescription description = new HumanDescription()
			{
				armStretch = 0.05f,
				feetSpacing = 0f,
				hasTranslationDoF = false,
				legStretch = 0.05f,
				lowerArmTwist = 0.5f,
				lowerLegTwist = 0.5f,
				upperArmTwist = 0.5f,
				upperLegTwist = 0.5f,
				skeleton = CreateSkeleton(avatarRoot),
				human = CreateHuman(avatarRoot, bonesNames),
			};
			return description;
		}

		// Create a SkeletonBone array out of an Avatar GameObject
		// This assumes that the Avatar as supplied is in a T-Pose
		// The local positions of its bones/joints are used to define this T-Pose
		private SkeletonBone[] CreateSkeleton(GameObject avatarRoot)
		{
			List<SkeletonBone> skeleton = new List<SkeletonBone>();

			Transform[] avatarTransforms = avatarRoot.GetComponentsInChildren<Transform>();
			foreach (Transform avatarTransform in avatarTransforms)
			{
				SkeletonBone bone = new SkeletonBone()
				{
					name = avatarTransform.name,
					position = avatarTransform.localPosition,
					rotation = avatarTransform.localRotation,
					scale = avatarTransform.localScale
				};

				skeleton.Add(bone);
			}

			return skeleton.ToArray();
		}

		// Create a HumanBone array out of an Avatar GameObject
		// This is where the various bones/joints get associated with the
		// joint names that Unity understands. This is done using the
		// static dictionary defined at the top. 
		private HumanBone[] CreateHuman(GameObject avatarRoot, Dictionary<string, string> boneNames)
		{
			List<HumanBone> human = new List<HumanBone>();

			Transform[] avatarTransforms = avatarRoot.GetComponentsInChildren<Transform>();
			foreach (Transform avatarTransform in avatarTransforms)
			{
				if (boneNames.TryGetValue(avatarTransform.name, out string humanName))
				{
					HumanBone bone = new HumanBone
					{
						boneName = avatarTransform.name,
						humanName = humanName,
						limit = new HumanLimit()
					};
					bone.limit.useDefaultValues = true;

					human.Add(bone);
				}
			}
			return human.ToArray();
		}
		
		/// <summary>
		///		Applies a skeleton to the given Avatar GameObject.
		/// </summary>
		/// <param name="source">Avatar GameObject</param>
		public void Build(GameObject source)
		{
			// TODO: These are RPM spesific bones, wont work with other characters
			Transform leftArm = source.transform.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm");
			Transform rightArm = source.transform.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm");
			Transform lowerLeftArm = source.transform.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm");
			Transform lowerRightArm = source.transform.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm");

			Debug.Log(source.name);
			
			leftArm.localRotation = Quaternion.Euler(0, 10, -45f);
			rightArm.localRotation = Quaternion.Euler(0, -10, 45f);
			lowerLeftArm.localRotation = Quaternion.Euler(0, 0, 0f);
			lowerRightArm.localRotation = Quaternion.Euler(0, 0, 0f);

			var description = CreateHumanDescription(source, DefaultBoneNames);
			Avatar avatar = AvatarBuilder.BuildHumanAvatar(source, description);

			Animator animator = source.GetComponent<Animator>();
			if (animator != null)
			{
				animator.avatar = avatar;
			}
		}

		public void Build(GameObject source, Dictionary<string, string> boneNames)
		{
			var description = CreateHumanDescription(source, boneNames);
			Avatar animAvatar = AvatarBuilder.BuildHumanAvatar(source, description);
			animAvatar.name = source.name;
			
			Animator animator = source.GetComponent<Animator>();
			if (animator == null)
			{
				animator = source.AddComponent<Animator>();
			}
			animator.avatar = animAvatar;
		}
	}
}
