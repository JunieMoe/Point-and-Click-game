#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NodeConnection
{
    public GameObject targetNode;
    public float movementSpeed = 15f;
    public string animationTrigger = "Walk";
}

public class MoveNode : MonoBehaviour
{
    public List<NodeConnection> connections = new List<NodeConnection>();
    public float autoConnectRadius = 1.1f;

    public void AutoGenerateConnections()
    {
        MoveNode[] allNodes = FindObjectsOfType<MoveNode>();
        var newConnections = new List<NodeConnection>();

        foreach (MoveNode node in allNodes)
        {
            if (node != this && node.GetComponent<SceneExitNode>() == null)
            {
                float dist = Vector2.Distance(transform.position, node.transform.position);
                if (dist <= autoConnectRadius)
                {
                    // Add connection if it doesn't already exist
                    if (!ConnectionExists(connections, node.gameObject))
                    {
                        NodeConnection conn = new NodeConnection
                        {
                            targetNode = node.gameObject,
                            movementSpeed = 15f, // default
                            animationTrigger = "Walk" // default
                        };
                        newConnections.Add(conn);
                    }

                    // Ensure bidirectional on the other node too
                    if (!ConnectionExists(node.connections, this.gameObject))
                    {
                        NodeConnection reverseConn = new NodeConnection
                        {
                            targetNode = this.gameObject,
                            movementSpeed = 15f,
                            animationTrigger = "Walk"
                        };
                        node.connections.Add(reverseConn);
                        #if UNITY_EDITOR
                        EditorUtility.SetDirty(node);
                        #endif
                    }
                }
            }
        }

        connections = newConnections;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private bool ConnectionExists(List<NodeConnection> connList, GameObject target)
    {
        return connList.Exists(c => c.targetNode == target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (connections != null)
        {
            foreach (NodeConnection conn in connections)
            {
                if (conn.targetNode != null)
                    Gizmos.DrawLine(transform.position, conn.targetNode.transform.position);
            }
        }
    }
}
