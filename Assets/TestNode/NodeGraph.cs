
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UNEB
{
    /// <summary>
    /// Represents a graph of nodes.
    /// </summary>
    public class NodeGraph : ScriptableObject
    {
        /// <summary>
        /// The types of nodes accepted by the graph.
        /// </summary>
        public static HashSet<Type> nodeTypes = new HashSet<Type>();

        //[HideInInspector]
        public List<Node> nodes = new List<Node>();

        [MenuItem("自定义节点/Test")]
        public static void CreateMenu()
        {
            var savePath = EditorUtility.SaveFilePanel(
                "保存剧情数据",
                "Assets",
                "WordEditorData22",
                "asset"
            );
            savePath = savePath.Substring(Application.dataPath.Length - 6);

            NodeGraph nodeGraph = ScriptableObject.CreateInstance<NodeGraph>();

            Node curveNode1 = ScriptableObject.CreateInstance<CurveNode>() as Node;
            Node curveNode2 = ScriptableObject.CreateInstance<CurveNode>() as Node;
            Node curveNode3 = ScriptableObject.CreateInstance<CurveNode>() as Node;

            //AssetDatabase.AddObjectToAsset(curveNode1, nodeGraph);
            //AssetDatabase.AddObjectToAsset(curveNode2, nodeGraph);
            //AssetDatabase.AddObjectToAsset(curveNode3, nodeGraph);

            nodeGraph.nodes.Add(curveNode1);
            nodeGraph.nodes.Add(curveNode2);
            nodeGraph.nodes.Add(curveNode3);
            
            AssetDatabase.CreateAsset(nodeGraph, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        

        /// <summary>
        /// Add a node to the graph.
        /// It is recommended that the save manager adds the nodes.
        /// </summary>
        /// <param name="n"></param>
        public void Add(Node n)
        {
            nodes.Add(n);
        }

        /// <summary>
        /// Removes a node from the graph but it is not destroyed.
        /// </summary>
        /// <param name="node"></param>
        public void Remove(Node node)
        {
            nodes.Remove(node);
        }

        /// <summary>
        /// Put the node at the end of the node list.
        /// </summary>
        /// <param name="node"></param>
        public void PushToEnd(Node node)
        {
            if (nodes.Remove(node))
            {
                nodes.Add(node);
            }
        }

        /// <summary>
        /// Gets called right before the graph is saved.
        /// Can be used to setup things before saving like sorting nodes.
        /// </summary>
        public virtual void OnSave() { }
    }
}