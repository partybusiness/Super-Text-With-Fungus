using UnityEditor;
using UnityEngine;
using System.Collections;
using Rotorz.ReorderableList;
using System.Collections.Generic;

namespace Fungus
{
	
	[CustomEditor (typeof(SuperTextMeshWriter))]
	public class SuperTextMeshWriterEditor : Editor
	{
		protected SerializedProperty textObject;
        protected SerializedProperty extraDelay;
		

		protected virtual void OnEnable()
		{
            textObject = serializedObject.FindProperty("targetTextObject");
            extraDelay = serializedObject.FindProperty("extraDelay");
		}
		
		public override void OnInspectorGUI() 
		{
			serializedObject.Update();
			
			//DialogInput t = target as DialogInput;
            EditorGUILayout.PropertyField(textObject);
            EditorGUILayout.PropertyField(extraDelay);

			/*EditorGUILayout.PropertyField(clickModeProp);
			EditorGUILayout.PropertyField(nextClickDelayProp);
			EditorGUILayout.PropertyField(ignoreMenuClicksProp);

			EditorGUILayout.PropertyField(keyPressModeProp);
			if (t.keyPressMode == DialogInput.KeyPressMode.KeyPressed)
			{
				EditorGUILayout.PropertyField(shiftKeyEnabledProp);
				ReorderableListGUI.Title(new GUIContent("Key List", "Keycodes to check for user input"));
				ReorderableListGUI.ListField(keyListProp);			
			}*/
				
			serializedObject.ApplyModifiedProperties();
		}		
	}
	
}