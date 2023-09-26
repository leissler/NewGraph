using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NewGraph {

    [Serializable]
    public class GraphModelBase {

        /// <summary>
        /// List of all the nodes we want to work on.
        /// </summary>
        [SerializeReference, HideInInspector]
        public List<NodeModel> nodes = new List<NodeModel>();

#if UNITY_EDITOR
        // FROM HERE BE DRAGONS...
        [SerializeReference, HideInInspector]
        public List<NodeModel> utilityNodes = new List<NodeModel>();

        [NonSerialized]
        public UnityEngine.Object baseObject;

        [NonSerialized]
        private SerializedProperty nodesProperty = null;
        
        [NonSerialized]
        private SerializedProperty utilityNodesProperty = null;

        [SerializeField, HideInInspector]
		private string tmpName;
        private SerializedProperty tmpNameProperty = null;
        private SerializedProperty originalNameProperty = null;

        [NonSerialized]
        private string basePropertyPath = null;
        
        [SerializeField, HideInInspector]
		private bool viewportInitiallySet = false;
        public bool ViewportInitiallySet => viewportInitiallySet;

        [SerializeField, HideInInspector]
        private Vector3 viewPosition;
        public Vector3 ViewPosition => viewPosition;
        
        [SerializeField, HideInInspector]
        private Vector3 viewScale;
        public Vector3 ViewScale => viewScale;

        public void SetViewport(Vector3 position, Vector3 scale) {
            if (position != viewPosition || scale != viewScale) {
                viewportInitiallySet = true;
                viewScale = scale;
                viewPosition = position;
            }
        }

        public NodeModel AddNode(INode node, bool isUtilityNode, UnityEngine.Object scope) {
            NodeModel baseNodeItem = new NodeModel(node);
            baseNodeItem.isUtilityNode = isUtilityNode;
            AddNode(baseNodeItem);
            ForceSerializationUpdate(scope);
            return baseNodeItem;
        }

        public NodeModel AddNode(NodeModel nodeItem) {
            if (!nodeItem.isUtilityNode) {
                nodes.Add(nodeItem);
            } else {
                utilityNodes.Add(nodeItem);
            }
            return nodeItem;
        }

        public void RemoveNode(NodeModel node) {
            if (!node.isUtilityNode) {
                nodes.Remove(node);

            } else {
                utilityNodes.Remove(node);
            }
        }

        public void RemoveNodes(List<NodeModel> nodesToRemove, UnityEngine.Object scope) {
            if (nodesToRemove.Count > 0) {
                Undo.RecordObject(scope, "Remove Nodes");
                foreach (NodeModel node in nodesToRemove) {
                    RemoveNode(node);
                }
            }
        }

        public void ForceSerializationUpdate(UnityEngine.Object scope) {
            EditorUtility.SetDirty(scope);
        }

        public void CreateSerializedObject(UnityEngine.Object scope, string rootFieldName) {
            basePropertyPath= rootFieldName;
            baseObject = scope;
            nodesProperty = null;
            utilityNodesProperty = null;
        }
#endif

    }
}
