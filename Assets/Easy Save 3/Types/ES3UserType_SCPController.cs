using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("player", "SCP_State", "agent", "agentSpeed", "agentAcceleration", "detectionRange", "attackRange", "fieldOfViewAngle", "soundDetectionRange", "scpHead", "obstructionMask", "animator", "scpModel", "invisibleDuration", "minVisibleTime", "maxVisibleTime", "isVisible", "isBlinking", "playerInSight", "playerHeard", "isCatchPlayer", "floatingSound", "glitchSound", "jumpscareSound", "stunSound", "simpleFirstPersonController", "forcePlayer", "actionSoundControl", "volumeEffect", "wanderingPositions", "currentWanderingTarget", "isWanderingMode")]
	public class ES3UserType_SCPController : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SCPController() : base(typeof(SCPController)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (SCPController)obj;
			
			writer.WritePropertyByRef("player", instance.player);
			writer.WriteProperty("SCP_State", instance.SCP_State, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(SCPController.SCPState)));
			writer.WritePropertyByRef("agent", instance.agent);
			writer.WriteProperty("agentSpeed", instance.agentSpeed, ES3Type_float.Instance);
			writer.WriteProperty("agentAcceleration", instance.agentAcceleration, ES3Type_float.Instance);
			writer.WriteProperty("detectionRange", instance.detectionRange, ES3Type_float.Instance);
			writer.WriteProperty("attackRange", instance.attackRange, ES3Type_float.Instance);
			writer.WriteProperty("fieldOfViewAngle", instance.fieldOfViewAngle, ES3Type_float.Instance);
			writer.WriteProperty("soundDetectionRange", instance.soundDetectionRange, ES3Type_float.Instance);
			writer.WritePropertyByRef("scpHead", instance.scpHead);
			writer.WriteProperty("obstructionMask", instance.obstructionMask, ES3Type_LayerMask.Instance);
			writer.WritePropertyByRef("animator", instance.animator);
			writer.WritePropertyByRef("scpModel", instance.scpModel);
			writer.WriteProperty("invisibleDuration", instance.invisibleDuration, ES3Type_float.Instance);
			writer.WriteProperty("minVisibleTime", instance.minVisibleTime, ES3Type_float.Instance);
			writer.WriteProperty("maxVisibleTime", instance.maxVisibleTime, ES3Type_float.Instance);
			writer.WriteProperty("isVisible", instance.isVisible, ES3Type_bool.Instance);
			writer.WritePrivateField("isBlinking", instance);
			writer.WriteProperty("playerInSight", instance.playerInSight, ES3Type_bool.Instance);
			writer.WriteProperty("playerHeard", instance.playerHeard, ES3Type_bool.Instance);
			writer.WritePrivateField("isCatchPlayer", instance);
			writer.WritePropertyByRef("floatingSound", instance.floatingSound);
			writer.WritePropertyByRef("glitchSound", instance.glitchSound);
			writer.WritePropertyByRef("jumpscareSound", instance.jumpscareSound);
			writer.WritePropertyByRef("stunSound", instance.stunSound);
			writer.WritePropertyByRef("simpleFirstPersonController", instance.simpleFirstPersonController);
			writer.WriteProperty("forcePlayer", instance.forcePlayer, ES3Type_bool.Instance);
			writer.WritePropertyByRef("actionSoundControl", instance.actionSoundControl);
			writer.WritePropertyByRef("volumeEffect", instance.volumeEffect);
			writer.WriteProperty("wanderingPositions", instance.wanderingPositions, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.Transform>)));
			writer.WritePrivateFieldByRef("currentWanderingTarget", instance);
			writer.WriteProperty("isWanderingMode", instance.isWanderingMode, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (SCPController)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "player":
						instance.player = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "SCP_State":
						instance.SCP_State = reader.Read<SCPController.SCPState>();
						break;
					case "agent":
						instance.agent = reader.Read<UnityEngine.AI.NavMeshAgent>(ES3UserType_NavMeshAgent.Instance);
						break;
					case "agentSpeed":
						instance.agentSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "agentAcceleration":
						instance.agentAcceleration = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "detectionRange":
						instance.detectionRange = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "attackRange":
						instance.attackRange = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "fieldOfViewAngle":
						instance.fieldOfViewAngle = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "soundDetectionRange":
						instance.soundDetectionRange = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "scpHead":
						instance.scpHead = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "obstructionMask":
						instance.obstructionMask = reader.Read<UnityEngine.LayerMask>(ES3Type_LayerMask.Instance);
						break;
					case "animator":
						instance.animator = reader.Read<UnityEngine.Animator>(ES3UserType_Animator.Instance);
						break;
					case "scpModel":
						instance.scpModel = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "invisibleDuration":
						instance.invisibleDuration = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "minVisibleTime":
						instance.minVisibleTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "maxVisibleTime":
						instance.maxVisibleTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "isVisible":
						instance.isVisible = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isBlinking":
					instance = (SCPController)reader.SetPrivateField("isBlinking", reader.Read<System.Boolean>(), instance);
					break;
					case "playerInSight":
						instance.playerInSight = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "playerHeard":
						instance.playerHeard = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isCatchPlayer":
					instance = (SCPController)reader.SetPrivateField("isCatchPlayer", reader.Read<System.Boolean>(), instance);
					break;
					case "floatingSound":
						instance.floatingSound = reader.Read<UnityEngine.AudioSource>();
						break;
					case "glitchSound":
						instance.glitchSound = reader.Read<UnityEngine.AudioSource>();
						break;
					case "jumpscareSound":
						instance.jumpscareSound = reader.Read<UnityEngine.AudioSource>();
						break;
					case "stunSound":
						instance.stunSound = reader.Read<UnityEngine.AudioSource>();
						break;
					case "simpleFirstPersonController":
						instance.simpleFirstPersonController = reader.Read<SimpleFirstPersonController>();
						break;
					case "forcePlayer":
						instance.forcePlayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "actionSoundControl":
						instance.actionSoundControl = reader.Read<ActionSoundControl>();
						break;
					case "volumeEffect":
						instance.volumeEffect = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					case "wanderingPositions":
						instance.wanderingPositions = reader.Read<System.Collections.Generic.List<UnityEngine.Transform>>();
						break;
					case "currentWanderingTarget":
					instance = (SCPController)reader.SetPrivateField("currentWanderingTarget", reader.Read<UnityEngine.Transform>(), instance);
					break;
					case "isWanderingMode":
						instance.isWanderingMode = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SCPControllerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SCPControllerArray() : base(typeof(SCPController[]), ES3UserType_SCPController.Instance)
		{
			Instance = this;
		}
	}
}