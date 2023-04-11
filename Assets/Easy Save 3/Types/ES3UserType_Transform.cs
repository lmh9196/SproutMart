using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("position", "localPosition", "eulerAngles", "localEulerAngles", "right", "up", "forward", "rotation", "localRotation", "rotationOrder", "localScale", "parent", "parentInternal", "hasChanged", "hierarchyCapacity", "constrainProportionsScale", "name")]
	public class ES3UserType_Transform : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Transform() : base(typeof(UnityEngine.Transform)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.Transform)obj;
			
			writer.WriteProperty("position", instance.position, ES3Type_Vector3.Instance);
			writer.WriteProperty("localPosition", instance.localPosition, ES3Type_Vector3.Instance);
			writer.WriteProperty("eulerAngles", instance.eulerAngles, ES3Type_Vector3.Instance);
			writer.WriteProperty("localEulerAngles", instance.localEulerAngles, ES3Type_Vector3.Instance);
			writer.WriteProperty("right", instance.right, ES3Type_Vector3.Instance);
			writer.WriteProperty("up", instance.up, ES3Type_Vector3.Instance);
			writer.WriteProperty("forward", instance.forward, ES3Type_Vector3.Instance);
			writer.WriteProperty("rotation", instance.rotation, ES3Type_Quaternion.Instance);
			writer.WriteProperty("localRotation", instance.localRotation, ES3Type_Quaternion.Instance);
			writer.WritePrivateProperty("rotationOrder", instance);
			writer.WriteProperty("localScale", instance.localScale, ES3Type_Vector3.Instance);
			writer.WritePropertyByRef("parent", instance.parent);
			writer.WritePrivatePropertyByRef("parentInternal", instance);
			writer.WriteProperty("hasChanged", instance.hasChanged, ES3Type_bool.Instance);
			writer.WriteProperty("hierarchyCapacity", instance.hierarchyCapacity, ES3Type_int.Instance);
			writer.WritePrivateProperty("constrainProportionsScale", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.Transform)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "position":
						instance.position = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "localPosition":
						instance.localPosition = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "eulerAngles":
						instance.eulerAngles = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "localEulerAngles":
						instance.localEulerAngles = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "right":
						instance.right = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "up":
						instance.up = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "forward":
						instance.forward = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "rotation":
						instance.rotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "localRotation":
						instance.localRotation = reader.Read<UnityEngine.Quaternion>(ES3Type_Quaternion.Instance);
						break;
					case "localScale":
						instance.localScale = reader.Read<UnityEngine.Vector3>(ES3Type_Vector3.Instance);
						break;
					case "parent":
						instance.parent = reader.Read<UnityEngine.Transform>(ES3UserType_Transform.Instance);
						break;
					case "parentInternal":
					instance = (UnityEngine.Transform)reader.SetPrivateProperty("parentInternal", reader.Read<UnityEngine.Transform>(), instance);
					break;
					case "hasChanged":
						instance.hasChanged = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "hierarchyCapacity":
						instance.hierarchyCapacity = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "constrainProportionsScale":
					instance = (UnityEngine.Transform)reader.SetPrivateProperty("constrainProportionsScale", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_TransformArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TransformArray() : base(typeof(UnityEngine.Transform[]), ES3UserType_Transform.Instance)
		{
			Instance = this;
		}
	}
}