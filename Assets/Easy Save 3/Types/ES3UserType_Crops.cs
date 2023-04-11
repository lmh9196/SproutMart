using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("cropsData")]
	public class ES3UserType_Crops : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Crops() : base(typeof(Crops)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Crops)obj;
			
			writer.WritePropertyByRef("cropsData", instance.cropsData);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Crops)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "cropsData":
						instance.cropsData = reader.Read<CropsData>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_CropsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_CropsArray() : base(typeof(Crops[]), ES3UserType_Crops.Instance)
		{
			Instance = this;
		}
	}
}