using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_StageManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_StageManager() : base(typeof(StageManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (StageManager)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (StageManager)obj;
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
	}


	public class ES3UserType_StageManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StageManagerArray() : base(typeof(StageManager[]), ES3UserType_StageManager.Instance)
		{
			Instance = this;
		}
	}
}