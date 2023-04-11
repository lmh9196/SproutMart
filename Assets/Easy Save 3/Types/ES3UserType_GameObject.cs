using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_GameObject : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameObject() : base(typeof(UnityEngine.GameObject)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.GameObject)obj;
			
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.GameObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new UnityEngine.GameObject();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_GameObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameObjectArray() : base(typeof(UnityEngine.GameObject[]), ES3UserType_GameObject.Instance)
		{
			Instance = this;
		}
	}
}