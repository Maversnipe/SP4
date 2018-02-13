using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI))]
[CanEditMultipleObjects]
public class ArmorEditor : Editor {
	string[] _armordata = new [] {"Empty"};
	int _choice = 0;

	public override void OnInspectorGUI ()
	{
		if (ArmorDatabase.StringData.Count == 0) {
			return;
		}

		_armordata = ArmorDatabase.StringData.ToArray ();
		DrawDefaultInspector ();
		_choice = EditorGUILayout.Popup ("Armor", _choice, _armordata);
		var someClass = target as AI;
		someClass._Armor = ArmorDatabase.Database[_choice];
		EditorUtility.SetDirty (target);
	}
}
