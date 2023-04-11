using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("size", "direction", "density", "isTrigger", "usedByEffector", "usedByComposite", "offset", "sharedMaterial", "enabled", "name")]
	public class ES3UserType_CapsuleCollider2D : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_CapsuleCollider2D() : base(typeof(UnityEngine.CapsuleCollider2D)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.CapsuleCollider2D)obj;
			
			writer.WriteProperty("size", instance.size, ES3Type_Vector2.Instance);
			writer.WriteProperty("direction", instance.direction, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(UnityEngine.CapsuleDirection2D)));
			writer.WriteProperty("density", instance.density, ES3Type_float.Instance);
			writer.WriteProperty("isTrigger", instance.isTrigger, ES3Type_bool.Instance);
			writer.WriteProperty("usedByEffector", instance.usedByEffector, ES3Type_bool.Instance);
			writer.WriteProperty("usedByComposite", instance.usedByComposite, ES3Type_bool.Instance);
			writer.WriteProperty("offset", instance.offset, ES3Type_Vector2.Instance);
			writer.WritePropertyByRef("sharedMaterial", instance.sharedMaterial);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.CapsuleCollider2D)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "size":
						instance.size = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "direction":
						instance.direction = reader.Read<UnityEngine.CapsuleDirection2D>();
						break;
					case "density":
						instance.density = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "isTrigger":
						instance.isTrigger = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "usedByEffector":
						instance.usedByEffector = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "usedByComposite":
						instance.usedByComposite = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "offset":
						instance.offset = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "sharedMaterial":
						instance.sharedMaterial = reader.Read<UnityEngine.PhysicsMaterial2D>(ES3Type_PhysicsMaterial2D.Instance);
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


	public class ES3UserType_CapsuleCollider2DArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CapsuleCollider2DArray() : base(typeof(UnityEngine.CapsuleCollider2D[]), ES3UserType_CapsuleCollider2D.Instance)
		{
			Instance = this;
		}
	}
}