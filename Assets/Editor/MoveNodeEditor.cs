using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveNode))]
public class MoveNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MoveNode node = (MoveNode)target;

        if (GUILayout.Button("Auto-Generate Connections"))
        {
            node.AutoGenerateConnections();
            EditorUtility.SetDirty(node);
        }
    }
}
