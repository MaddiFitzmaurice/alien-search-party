using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Level Number")]
    public int LevelNumber;

    [Header("Narrative Data")]
    public int BarkAfterSpawned;

    // Which Abductees spawn in this level?
    [Header("Which Abductees Spawn?")]
    public bool GreenAliens;
    public bool GreyAliens;
    public bool Humans;

    // How many Abductees will spawn over the duration of the level?
    [HideInInspector] public int AmountGreenAliens = 0;
    [HideInInspector] public int AmountGreyAliens = 0;
    [HideInInspector] public int AmountHumans = 0;
    [HideInInspector] public int AmountTotal;

    // Chance for a Grey Alien to spawn
    [HideInInspector] public int SpawnChanceGrey = 0;
    [HideInInspector] public int SpawnChanceGreyStart;
    [HideInInspector] public int SpawnChanceGreyChange;

    // Spawn Rate
    [Header("Spawn Rate")]
    public float SpawnRateBase;
    [HideInInspector] public float CurrentSpawnRate;
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

        if (level.GreenAliens && level.GreyAliens)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grey Alien Spawn Chance", EditorStyles.boldLabel);
            level.SpawnChanceGreyStart = EditorGUILayout.IntField("1 in X Chance", level.SpawnChanceGreyStart);
            level.SpawnChanceGrey = EditorGUILayout.IntField("Current Spawn Chance", level.SpawnChanceGrey);
            level.SpawnChanceGreyChange = EditorGUILayout.IntField("Decrease Chance By:", level.SpawnChanceGreyChange);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Number of Abductees To Spawn", EditorStyles.boldLabel);
        
        if (level.GreenAliens && level.GreyAliens)
        {
            level.AmountGreenAliens = EditorGUILayout.IntSlider("Num of Green Aliens", level.AmountGreenAliens, 0, 100);
            level.AmountGreyAliens = EditorGUILayout.IntSlider("Num of Grey Aliens", level.AmountGreyAliens, 0, 100);
        }
        else if (level.GreenAliens)
        {
            level.AmountGreenAliens = EditorGUILayout.IntSlider("Num of Green Aliens", level.AmountGreenAliens, 0, 100);
            level.AmountGreyAliens = 0;
            level.AmountHumans = 0;
        }
        else if (level.GreyAliens)
        {
            level.AmountGreyAliens = EditorGUILayout.IntSlider("Num of Grey Aliens", level.AmountGreyAliens, 0, 100);
            level.AmountGreenAliens = 0;
            level.AmountHumans = 0;
        }

        if (level.Humans)
        {
            level.AmountHumans = EditorGUILayout.IntSlider("Num of Humans", level.AmountHumans, 0, 5);
            level.GreenAliens = false;
            level.GreyAliens = false;
        }

        level.AmountTotal = level.AmountHumans + level.AmountGreyAliens + level.AmountGreenAliens;
        EditorGUILayout.IntField("Total", level.AmountTotal);
    }
}
