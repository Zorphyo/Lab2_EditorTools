using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehavior : MonoBehaviour
{
    public float cubeSize = 1;
    public float sphereRadius = 1;
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBehavior)), CanEditMultipleObjects]
public class EnemyBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        var cubeSize = serializedObject.FindProperty("cubeSize");
        var sphereRadius = serializedObject.FindProperty("sphereRadius");

        EditorGUILayout.PropertyField(cubeSize);
        EditorGUILayout.PropertyField(sphereRadius);

        serializedObject.ApplyModifiedProperties();

        if (cubeSize.floatValue > 2)
        {
            EditorGUILayout.HelpBox("The cubes' sizes cannot be bigger than 2!", MessageType.Warning);
        }

        if (sphereRadius.floatValue < 1)
        {
            EditorGUILayout.HelpBox("The spheres' radius cannot be smaller than 1!", MessageType.Warning);
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Select All Spheres/Cubes"))
            {
                var allEnemyBehavior = GameObject.FindObjectsOfType<EnemyBehavior>();
                var allEnemyGameObjects = allEnemyBehavior.Select(enemy => enemy.gameObject).ToArray();
                Selection.objects = allEnemyGameObjects;
            }

            if (GUILayout.Button("Clear Selection"))
            {
                Selection.objects = new Object[] { (target as EnemyBehavior).gameObject };
            }
        }

        var cachedColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Disable/Enable All Spheres/Cubes", GUILayout.Height(40)))
        {
            foreach (var enemy in GameObject.FindObjectsOfType<EnemyBehavior>(true))
            {
                Undo.RecordObject(enemy.gameObject, "Disable/Enable Cube/Sphere");
                enemy.gameObject.SetActive(!enemy.gameObject.activeSelf);
            }
        }

        GUI.backgroundColor = cachedColor;
    }
}
#endif
