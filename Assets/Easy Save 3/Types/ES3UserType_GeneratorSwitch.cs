using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("animator", "generatorController", "enabled")]
	public class ES3UserType_GeneratorSwitch : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GeneratorSwitch() : base(typeof(GeneratorSwitch)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GeneratorSwitch)obj;
			
			writer.WritePrivateFieldByRef("animator", instance);
			writer.WritePrivateFieldByRef("generatorController", instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GeneratorSwitch)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "animator":
					instance = (GeneratorSwitch)reader.SetPrivateField("animator", reader.Read<UnityEngine.Animator>(), instance);
					break;
					case "generatorController":
					instance = (GeneratorSwitch)reader.SetPrivateField("generatorController", reader.Read<GeneratorController>(), instance);
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


	public class ES3UserType_GeneratorSwitchArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GeneratorSwitchArray() : base(typeof(GeneratorSwitch[]), ES3UserType_GeneratorSwitch.Instance)
		{
			Instance = this;
		}
	}
}