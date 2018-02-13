using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI))]
[CanEditMultipleObjects]
public class WeaponEditor : Editor {
	string[] _weapondata = new [] {"Empty"};
	int _choice = 0;

	public override void OnInspectorGUI()
	{
		if (WeaponDatabase.Database.Count == 0) {
			return;
		}

		_weapondata = WeaponDatabase.StringData.ToArray ();
		DrawDefaultInspector ();
		_choice = EditorGUILayout.Popup ("Weapon", _choice, _weapondata);
		var someClass = target as AI;
		someClass._Weapon = WeaponDatabase.Database [_choice];
		EditorUtility.SetDirty (target);
	}
}
