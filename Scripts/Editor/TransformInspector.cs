﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{

    public bool showTools;
    public bool copyPosition;
    public bool copyRotation;
    public bool copyScale;
    public bool pastePosition;
    public bool pasteRotation;
    public bool pasteScale;
    public bool selectionNullError;

    public override void OnInspectorGUI()
    {

        Transform t = (Transform)target;

        // Replicate the standard transform inspector gui
        EditorGUIUtility.LookLikeControls();
        EditorGUI.indentLevel = 0;
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("P", GUILayout.Width(25)))
        {
            t.localPosition = Vector3.zero;
        }
        Vector3 position = EditorGUILayout.Vector3Field("", t.localPosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(25)))
        {
            t.localRotation = Quaternion.identity;
        }
        Vector3 eulerAngles = EditorGUILayout.Vector3Field("", t.localEulerAngles);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("S", GUILayout.Width(25)))
        {
            t.localScale = Vector3.one;
        }
        Vector3 scale = EditorGUILayout.Vector3Field("", t.localScale);
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.LookLikeInspector();

       
        //  END TRANSFORM TOOLS FOLD DOWN   //

        if (GUI.changed)
        {
            SetCopyPasteBools();
            Undo.RegisterUndo(t, "Transform Change");

            t.localPosition = FixIfNaN(position);
            t.localEulerAngles = FixIfNaN(eulerAngles);
            t.localScale = FixIfNaN(scale);
        }
    }

    private Vector3 FixIfNaN(Vector3 v)
    {
        if (float.IsNaN(v.x))
        {
            v.x = 0;
        }
        if (float.IsNaN(v.y))
        {
            v.y = 0;
        }
        if (float.IsNaN(v.z))
        {
            v.z = 0;
        }
        return v;
    }

    void OnEnable()
    {
        showTools = EditorPrefs.GetBool("ShowTools", false);
        copyPosition = EditorPrefs.GetBool("Copy Position", true);
        copyRotation = EditorPrefs.GetBool("Copy Rotation", true);
        copyScale = EditorPrefs.GetBool("Copy Scale", true);
        pastePosition = EditorPrefs.GetBool("Paste Position", true);
        pasteRotation = EditorPrefs.GetBool("Paste Rotation", true);
        pasteScale = EditorPrefs.GetBool("Paste Scale", true);
    }

    void TransformCopyAll()
    {
        copyPosition = true;
        copyRotation = true;
        copyScale = true;
        GUI.changed = true;
    }

    void TransformCopyNone()
    {
        copyPosition = false;
        copyRotation = false;
        copyScale = false;
        GUI.changed = true;
    }

    void SetCopyPasteBools()
    {
        pastePosition = copyPosition;
        pasteRotation = copyRotation;
        pasteScale = copyScale;

        EditorPrefs.SetBool("Copy Position", copyPosition);
        EditorPrefs.SetBool("Copy Rotation", copyRotation);
        EditorPrefs.SetBool("Copy Scale", copyScale);
        EditorPrefs.SetBool("Paste Position", pastePosition);
        EditorPrefs.SetBool("Paste Rotation", pasteRotation);
        EditorPrefs.SetBool("Paste Scale", pasteScale);
    }
}