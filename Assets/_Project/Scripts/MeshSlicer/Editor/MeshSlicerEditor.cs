using System.Collections;
using System.Collections.Generic;
using Codice.CM.SEIDInfo;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshSlicerScaffolding))]
public class MeshSlicerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshSlicerScaffolding meshSlicer = target as MeshSlicerScaffolding;

        if (GUILayout.Button("Find Dice Faces"))
        {
            Undo.RecordObject(meshSlicer, "Slice");
            meshSlicer.SliceMesh();
            EditorUtility.SetDirty(meshSlicer);
        }
    }
}
