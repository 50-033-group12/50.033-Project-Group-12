using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerSpawner))]
public class PlayerSpawnerEditor : Editor
{
    public Events.PrimaryWeapon primaryWeapon;
    public Events.SecondaryWeapon secondaryWeapon;
    public Events.UltimateWeapon ultimateWeapon;
    public int playerId = 1;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("(DEBUG) Spawn Tanks", EditorStyles.boldLabel);
        primaryWeapon = (Events.PrimaryWeapon)EditorGUILayout.EnumPopup("Primary Weapon", primaryWeapon);
        secondaryWeapon = (Events.SecondaryWeapon)EditorGUILayout.EnumPopup("Secondary Weapon", secondaryWeapon);
        ultimateWeapon = (Events.UltimateWeapon)EditorGUILayout.EnumPopup("Ultimate Weapon", ultimateWeapon);
        playerId = EditorGUILayout.IntField("Player Id", playerId);
        PlayerSpawner myScript = (PlayerSpawner)target;
        if (GUILayout.Button("Spawn"))
        {
            myScript.ReloadPrefabs();
            myScript.SpawnPlayer(playerId, new Tuple<Events.PrimaryWeapon, Events.SecondaryWeapon, Events.UltimateWeapon>(primaryWeapon, secondaryWeapon, ultimateWeapon));
        }
    }
}
