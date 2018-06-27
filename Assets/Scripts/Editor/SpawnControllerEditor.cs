using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnController))]
public class SpawnControllerEditor : Editor{

    int _colorArrayLenght = 4;
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        
        _colorArrayLenght = EditorGUILayout.IntField("Amount of colors:", _colorArrayLenght);
        var spawnController = (SpawnController) target;
        if (GUILayout.Button("Fill up random!")){
            spawnController.FillUpColors(_colorArrayLenght);
        }
    }
}
