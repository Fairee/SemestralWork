using UnityEngine;
using System.Collections;
using UnityEditor;

#if(UNITY_EDITOR)
[CustomEditor(typeof(PipeCreator))]
public class PipeCreatorEditor : Editor

{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PipeCreator shapeFile = (PipeCreator)target;
        shapeFile.filename = GUILayout.TextArea(shapeFile.filename);
        if (GUILayout.Button("Read")) {
            shapeFile.shapeFileReader.Read(shapeFile.filename);
            Debug.Log("read");
        }
        if (GUILayout.Button("Generate"))
        {
            shapeFile.generate();
        }
        if (GUILayout.Button("DBF"))
        {
            shapeFile.dbfReader.Read(shapeFile.filename);
            Debug.Log("DBF");
        }
        shapeFile.isWGS = GUILayout.Toggle(shapeFile.isWGS, "IS WGS");

    }


}
#endif
