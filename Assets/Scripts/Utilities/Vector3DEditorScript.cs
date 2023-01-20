using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



#if(UNITY_EDITOR)
[CustomEditor(typeof(Vector3D))]
public class Vector3DEditorScript : Editor
{

    SerializedProperty x,y,z;
    // Start is called before the first frame update
    private void OnEnable()
    {
        x = serializedObject.FindProperty("x");
        y = serializedObject.FindProperty("y");
        z = serializedObject.FindProperty("z");
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        x.doubleValue = EditorGUILayout.DoubleField(x.doubleValue);
    }
}

#endif
