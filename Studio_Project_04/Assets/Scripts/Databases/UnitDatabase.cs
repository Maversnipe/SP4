using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

[System.Serializable]
public class UnitDatabase : GenericSingleton<UnitDatabase>
{

	public static List<UnitVariables> unitDatabase = new List<UnitVariables>();

	private JsonData unitData;

	// Use this for initialization
	void Start () {
		unitData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/UnitVariables.json"));

		ConstructUnitDatabase();

	}

	public UnitVariables FetchUnitByName(string _name)
	{
		for(int i = 0; i < unitDatabase.Count; i++)
		{
			if (unitDatabase[i].Name == _name)
			{
				return unitDatabase[i];
			}
		}

		return null;
	}

	void ConstructUnitDatabase()
	{
		for(int i = 0; i < unitData.Count; i++)
		{
			UnitVariables newUnit = gameObject.AddComponent<UnitVariables> ();
			newUnit.SetUnitVariables (unitData[i]["name"].ToString(), (int)unitData[i]["hp"], (int)unitData[i]["ap"],
				(int)unitData[i]["initiative"], (int)unitData[i]["id"], unitData[i]["weapon"].ToString(),
				unitData[i]["armor"].ToString());

			unitDatabase.Add(newUnit);
		}
	}
}
