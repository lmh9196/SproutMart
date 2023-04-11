using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("saveLayer", "saveTag", "saveName", "saveHideFlags", "saveActive", "saveChildren", "isQuitting", "componentsToSave")]
	public class ES3UserType_ES3AutoSave : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ES3AutoSave() : base(typeof(ES3AutoSave)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (ES3AutoSave)obj;
			
			writer.WriteProperty("saveLayer", instance.saveLayer, ES3Type_bool.Instance);
			writer.WriteProperty("saveTag", instance.saveTag, ES3Type_bool.Instance);
			writer.WriteProperty("saveName", instance.saveName, ES3Type_bool.Instance);
			writer.WriteProperty("saveHideFlags", instance.saveHideFlags, ES3Type_bool.Instance);
			writer.WriteProperty("saveActive", instance.saveActive, ES3Type_bool.Instance);
			writer.WriteProperty("saveChildren", instance.saveChildren, ES3Type_bool.Instance);
			writer.WritePrivateField("isQuitting", instance);
			writer.WriteProperty("componentsToSave", instance.componentsToSave, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UnityEngine.Component>)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (ES3AutoSave)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "saveLayer":
						instance.saveLayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveTag":
						instance.saveTag = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveName":
						instance.saveName = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveHideFlags":
						instance.saveHideFlags = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveActive":
						instance.saveActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveChildren":
						instance.saveChildren = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "isQuitting":
					instance = (ES3AutoSave)reader.SetPrivateField("isQuitting", reader.Read<System.Boolean>(), instance);
					break;
					case "componentsToSave":
						instance.componentsToSave = reader.Read<System.Collections.Generic.List<UnityEngine.Component>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ES3AutoSaveArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ES3AutoSaveArray() : base(typeof(ES3AutoSave[]), ES3UserType_ES3AutoSave.Instance)
		{
			Instance = this;
		}
	}
}