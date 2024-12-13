using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("destination", "stoppingDistance", "velocity", "nextPosition", "baseOffset", "autoTraverseOffMeshLink", "autoBraking", "autoRepath", "isStopped", "path", "agentTypeID", "areaMask", "speed", "angularSpeed", "acceleration", "updatePosition", "updateRotation", "updateUpAxis", "radius", "height", "obstacleAvoidanceType", "avoidancePriority", "enabled", "name")]
	public class ES3UserType_NavMeshAgent : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_NavMeshAgent() : base(typeof(UnityEngine.AI.NavMeshAgent)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.AI.NavMeshAgent)obj;
			
			writer.WriteProperty("destination", instance.destination, ES3Type_Vector3.Instance);
			writer.WriteProperty("stoppingDistance", instance.stoppingDistance, ES3Type_float.Instance);
			writer.WriteProperty("velocity", instance.velocity, ES3Type_Vector3.Instance);
			writer.WriteProperty("nextPosition", instance.nextPosition, ES3Type_Vector3.Instance);
			writer.WriteProperty("baseOffset", instance.baseOffset, ES3Type_float.Instance);
			writer.WriteProperty("autoTraverseOffMeshLink", instance.autoTraverseOffMeshLink, ES3Type_bool.Instance);
			writer.WriteProperty("autoBraking", instance.autoBraking, ES3Type_bool.Instance);
			writer.WriteProperty("autoRepath", instance.autoRepath, ES3Type_bool.Instance);
			writer.WriteProperty("isStopped", instance.isStopped, ES3Type_bool.Instance);
			writer.WriteProperty("path", instance.path, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.AI.NavMeshPath)));
			writer.WriteProperty("agentTypeID", instance.agentTypeID, ES3Type_int.Instance);
			writer.WriteProperty("areaMask", instance.areaMask, ES3Type_int.Instance);
			writer.WriteProperty("speed", instance.speed, ES3Type_float.Instance);
			writer.WriteProperty("angularSpeed", instance.angularSpeed, ES3Type_float.Instance);
			writer.WriteProperty("acceleration", instance.acceleration, ES3Type_float.Instance);
			writer.WriteProperty("updatePosition", instance.updatePosition, ES3Type_bool.Instance);
			writer.WriteProperty("updateRotation", instance.updateRotation, ES3Type_bool.Instance);
			writer.WriteProperty("updateUpAxis", instance.updateUpAxis, ES3Type_bool.Instance);
			writer.WriteProperty("radius", instance.radius, ES3Type_float.Instance);
			writer.WriteProperty("height", instance.height, ES3Type_float.Instance);
			writer.WriteProperty("obstacleAvoidanceType", instance.obstacleAvoidanceType, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.AI.ObstacleAvoidanceType)));
			writer.WriteProperty("avoidancePriority", instance.avoidancePriority, ES3Type_int.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.AI.NavMeshAgent)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "destination":
						instance.destination = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "stoppingDistance":
						instance.stoppingDistance = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "velocity":
						instance.velocity = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "nextPosition":
						instance.nextPosition = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "baseOffset":
						instance.baseOffset = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "autoTraverseOffMeshLink":
						instance.autoTraverseOffMeshLink = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "autoBraking":
						instance.autoBraking = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "autoRepath":
						instance.autoRepath = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isStopped":
						instance.isStopped = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "path":
						instance.path = reader.Read<UnityEngine.AI.NavMeshPath>();
						break;
					case "agentTypeID":
						instance.agentTypeID = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "areaMask":
						instance.areaMask = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "speed":
						instance.speed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "angularSpeed":
						instance.angularSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "acceleration":
						instance.acceleration = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "updatePosition":
						instance.updatePosition = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "updateRotation":
						instance.updateRotation = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "updateUpAxis":
						instance.updateUpAxis = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "radius":
						instance.radius = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "height":
						instance.height = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "obstacleAvoidanceType":
						instance.obstacleAvoidanceType = reader.Read<UnityEngine.AI.ObstacleAvoidanceType>();
						break;
					case "avoidancePriority":
						instance.avoidancePriority = reader.Read<System.Int32>(ES3Type_int.Instance);
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


	public class ES3UserType_NavMeshAgentArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_NavMeshAgentArray() : base(typeof(UnityEngine.AI.NavMeshAgent[]), ES3UserType_NavMeshAgent.Instance)
		{
			Instance = this;
		}
	}
}