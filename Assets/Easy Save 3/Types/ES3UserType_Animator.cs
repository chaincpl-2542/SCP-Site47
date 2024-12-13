using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("rootPosition", "rootRotation", "applyRootMotion", "updateMode", "allowConstantClipSamplingOptimization", "bodyPosition", "bodyPositionInternal", "bodyRotation", "bodyRotationInternal", "stabilizeFeet", "feetPivotActive", "speed", "cullingMode", "playbackTime", "recorderStartTime", "recorderStopTime", "runtimeAnimatorController", "avatar", "layersAffectMassCenter", "logWarnings", "fireEvents", "keepAnimatorStateOnDisable", "writeDefaultValuesOnDisable", "enabled", "name")]
	public class ES3UserType_Animator : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Animator() : base(typeof(UnityEngine.Animator)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.Animator)obj;
			
			writer.WriteProperty("rootPosition", instance.rootPosition, ES3Type_Vector3.Instance);
			writer.WriteProperty("rootRotation", instance.rootRotation, ES3Type_Quaternion.Instance);
			writer.WriteProperty("applyRootMotion", instance.applyRootMotion, ES3Type_bool.Instance);
			writer.WriteProperty("updateMode", instance.updateMode, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.AnimatorUpdateMode)));
			writer.WritePrivateProperty("allowConstantClipSamplingOptimization", instance);
			writer.WriteProperty("bodyPosition", instance.bodyPosition, ES3Type_Vector3.Instance);
			writer.WritePrivateProperty("bodyPositionInternal", instance);
			writer.WriteProperty("bodyRotation", instance.bodyRotation, ES3Type_Quaternion.Instance);
			writer.WritePrivateProperty("bodyRotationInternal", instance);
			writer.WriteProperty("stabilizeFeet", instance.stabilizeFeet, ES3Type_bool.Instance);
			writer.WriteProperty("feetPivotActive", instance.feetPivotActive, ES3Type_float.Instance);
			writer.WriteProperty("speed", instance.speed, ES3Type_float.Instance);
			writer.WriteProperty("cullingMode", instance.cullingMode, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.AnimatorCullingMode)));
			writer.WriteProperty("playbackTime", instance.playbackTime, ES3Type_float.Instance);
			writer.WriteProperty("recorderStartTime", instance.recorderStartTime, ES3Type_float.Instance);
			writer.WriteProperty("recorderStopTime", instance.recorderStopTime, ES3Type_float.Instance);
			writer.WritePropertyByRef("runtimeAnimatorController", instance.runtimeAnimatorController);
			writer.WritePropertyByRef("avatar", instance.avatar);
			writer.WriteProperty("layersAffectMassCenter", instance.layersAffectMassCenter, ES3Type_bool.Instance);
			writer.WriteProperty("logWarnings", instance.logWarnings, ES3Type_bool.Instance);
			writer.WriteProperty("fireEvents", instance.fireEvents, ES3Type_bool.Instance);
			writer.WriteProperty("keepAnimatorStateOnDisable", instance.keepAnimatorStateOnDisable, ES3Type_bool.Instance);
			writer.WriteProperty("writeDefaultValuesOnDisable", instance.writeDefaultValuesOnDisable, ES3Type_bool.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.Animator)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "rootPosition":
						instance.rootPosition = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "rootRotation":
						instance.rootRotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "applyRootMotion":
						instance.applyRootMotion = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "updateMode":
						instance.updateMode = reader.Read<UnityEngine.AnimatorUpdateMode>();
						break;
					case "allowConstantClipSamplingOptimization":
					instance = (UnityEngine.Animator)reader.SetPrivateProperty("allowConstantClipSamplingOptimization", reader.Read<System.Boolean>(), instance);
					break;
					case "bodyPosition":
						instance.bodyPosition = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "bodyPositionInternal":
					instance = (UnityEngine.Animator)reader.SetPrivateProperty("bodyPositionInternal", reader.Read<UnityEngine.Vector3>(), instance);
					break;
					case "bodyRotation":
						instance.bodyRotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "bodyRotationInternal":
					instance = (UnityEngine.Animator)reader.SetPrivateProperty("bodyRotationInternal", reader.Read<UnityEngine.Quaternion>(), instance);
					break;
					case "stabilizeFeet":
						instance.stabilizeFeet = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "feetPivotActive":
						instance.feetPivotActive = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "speed":
						instance.speed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "cullingMode":
						instance.cullingMode = reader.Read<UnityEngine.AnimatorCullingMode>();
						break;
					case "playbackTime":
						instance.playbackTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "recorderStartTime":
						instance.recorderStartTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "recorderStopTime":
						instance.recorderStopTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "runtimeAnimatorController":
						instance.runtimeAnimatorController = reader.Read<UnityEngine.RuntimeAnimatorController>();
						break;
					case "avatar":
						instance.avatar = reader.Read<UnityEngine.Avatar>();
						break;
					case "layersAffectMassCenter":
						instance.layersAffectMassCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "logWarnings":
						instance.logWarnings = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "fireEvents":
						instance.fireEvents = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "keepAnimatorStateOnDisable":
						instance.keepAnimatorStateOnDisable = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "writeDefaultValuesOnDisable":
						instance.writeDefaultValuesOnDisable = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_AnimatorArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_AnimatorArray() : base(typeof(UnityEngine.Animator[]), ES3UserType_Animator.Instance)
		{
			Instance = this;
		}
	}
}