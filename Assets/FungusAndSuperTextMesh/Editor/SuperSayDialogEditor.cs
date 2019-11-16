using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
	
	[CustomEditor (typeof(SuperSayDialog))]
	public class SuperSayDialogEditor : Editor
	{
		protected SerializedProperty fade;
        protected SerializedProperty button;
        protected SerializedProperty canvas;
        protected SerializedProperty nameText;
        protected SerializedProperty image;
        protected SerializedProperty fitText;
        protected SerializedProperty closeOther;
        protected SerializedProperty disappear;

        [ExecuteInEditMode]
		protected virtual void OnEnable()
		{
            fade = serializedObject.FindProperty("fadeDuration");
            button = serializedObject.FindProperty("continueButton");
            canvas = serializedObject.FindProperty("dialogCanvas");
            nameText = serializedObject.FindProperty("nameSuperText");
            image = serializedObject.FindProperty("characterImage");
            fitText = serializedObject.FindProperty("fitTextWithImage");
            closeOther = serializedObject.FindProperty("closeOtherDialogs");
            disappear = serializedObject.FindProperty("disappearMode");		
		}
		
		public override void OnInspectorGUI() 
		{
			serializedObject.Update();
			
            EditorGUILayout.PropertyField(fade);
            EditorGUILayout.PropertyField(button);
            EditorGUILayout.PropertyField(canvas);
            EditorGUILayout.PropertyField(nameText);
            EditorGUILayout.PropertyField(image);
            EditorGUILayout.PropertyField(fitText);
            EditorGUILayout.PropertyField(closeOther);
            EditorGUILayout.PropertyField(disappear);
				
			serializedObject.ApplyModifiedProperties();
		}		
	}
	
}