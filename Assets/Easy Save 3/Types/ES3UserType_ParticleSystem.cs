using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("time", "randomSeed", "useAutoRandomSeed", "name")]
	public class ES3UserType_ParticleSystem : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ParticleSystem() : base(typeof(UnityEngine.ParticleSystem)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.ParticleSystem)obj;
			
			writer.WriteProperty("time", instance.time, ES3Type_float.Instance);
			writer.WriteProperty("randomSeed", instance.randomSeed, ES3Type_uint.Instance);
			writer.WriteProperty("useAutoRandomSeed", instance.useAutoRandomSeed, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.ParticleSystem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "time":
						instance.time = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "randomSeed":
						instance.randomSeed = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "useAutoRandomSeed":
						instance.useAutoRandomSeed = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ParticleSystemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ParticleSystemArray() : base(typeof(UnityEngine.ParticleSystem[]), ES3UserType_ParticleSystem.Instance)
		{
			Instance = this;
		}
	}
}