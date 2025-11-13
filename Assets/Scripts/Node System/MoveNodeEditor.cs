using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveNode))]
public class MoveNodeEditor : Editor
{
    SerializedProperty connectionsProp;
    SerializedProperty autoConnectRadiusProp;

    void OnEnable()
    {
        connectionsProp = serializedObject.FindProperty("connections");
        autoConnectRadiusProp = serializedObject.FindProperty("autoConnectRadius");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MoveNode node = (MoveNode)target;

        EditorGUILayout.LabelField("Auto Connection Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(autoConnectRadiusProp);

        if (GUILayout.Button("Auto Generate Connections"))
        {
            node.AutoGenerateConnections();
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Manual Connections", EditorStyles.boldLabel);

        for (int i = 0; i < connectionsProp.arraySize; i++)
        {
            SerializedProperty conn = connectionsProp.GetArrayElementAtIndex(i);
            SerializedProperty targetNodeProp = conn.FindPropertyRelative("targetNode");
            SerializedProperty speedProp = conn.FindPropertyRelative("movementSpeed");
            SerializedProperty animProp = conn.FindPropertyRelative("animationTrigger");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(targetNodeProp, new GUIContent("Target Node"));
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                connectionsProp.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed"));
            EditorGUILayout.PropertyField(animProp, new GUIContent("Animation Trigger"));

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Connection"))
        {
            connectionsProp.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
