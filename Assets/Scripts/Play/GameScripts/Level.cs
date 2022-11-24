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

    // Chance for a Grey Alien to spawn
    [HideInInspector]
    public int SpawnChanceGrey = 0;
    [HideInInspector]
    public int SpawnChanceGreyStart;
    [HideInInspector]
    public int SpawnChanceGreyChange;

    // Spawn Rate
    [Header("Spawn Rate")]
    public float SpawnRate;
    public float SpawnRateBase;
    public float SpawnRateChange;
    public int ChangeRateAfter;
}

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var level = target as Level;

        if (level.SpawnGreenAliens && level.SpawnGreyAliens)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grey Alien Spawn Chance", EditorStyles.boldLabel);
            level.SpawnChanceGreyStart = EditorGUILayout.IntField("1 in X Chance", level.SpawnChanceGreyStart);
            level.SpawnChanceGrey = EditorGUILayout.IntField("Current Spawn Chance", level.SpawnChanceGrey);
            level.SpawnChanceGreyChange = EditorGUILayout.IntField("Decrease Chance By:", level.SpawnChanceGreyChange);
        }

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
