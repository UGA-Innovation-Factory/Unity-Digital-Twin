using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MQTTSource))]
public class MQTTSourceInspector : Editor
{
    private SerializedProperty _brokerAddress;
    private SerializedProperty _brokerPort;
    private SerializedProperty _useEncryption;
    private SerializedProperty _username;
    private SerializedProperty _password;
    private SerializedProperty topics;

    private void OnEnable()
    {
        _brokerAddress = serializedObject.FindProperty("brokerAddress");
        _brokerPort = serializedObject.FindProperty("brokerPort");
        _useEncryption = serializedObject.FindProperty("isEncrypted");
        _username = serializedObject.FindProperty("mqttUserName");
        _password = serializedObject.FindProperty("mqttPassword");
        topics = serializedObject.FindProperty("topics");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //base.OnInspectorGUI();


        EditorGUILayout.LabelField("MQTT Source Configurations", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_brokerAddress, new GUIContent("Address"));
        EditorGUILayout.PropertyField(_brokerPort);
        EditorGUILayout.PropertyField(_useEncryption, new GUIContent("Use Encryption"));
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Credentials", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_username);
        EditorGUILayout.PropertyField(_password);

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("MQTT Topics", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(topics);


        serializedObject.ApplyModifiedProperties();
    }
}
