using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    // Which Aliens spawn in this level?
    [Header("Which Aliens Spawn?")]
    public bool SpawnGreenAliens;
    public bool SpawnGreyAliens;

    // How many Aliens will spawn over the duration of the level?
    [HideInInspector]
    public int AmountGreenAliens = 0;
    [HideInInspector]
    public int AmountGreyAliens = 0;

    // Spawn Rate
    [Header("Spawn Rate")]
    public float SpawnRate;
}

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var level = target as Level;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Number of Aliens To Spawn", EditorStyles.boldLabel);

        if (level.SpawnGreenAliens)
        {
            level.AmountGreenAliens = EditorGUILayout.IntSlider("Num of Green Aliens", level.AmountGreenAliens, 1, 100);
        }

        if (level.SpawnGreyAliens)
        {
            level.AmountGreyAliens = EditorGUILayout.IntSlider("Num of Grey Aliens", level.AmountGreyAliens, 1, 100);
        }
    }
}
