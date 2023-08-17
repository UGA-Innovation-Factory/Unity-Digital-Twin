using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework.Internal;

[CustomPropertyDrawer(typeof(MQTTSource.Connection))]
public class MQTTTopicDrawer : PropertyDrawer
{
    SerializedProperty _topic;
    SerializedProperty _qos;
    SerializedProperty _rerouteTo;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        _topic = property.FindPropertyRelative("topic");
        _qos = property.FindPropertyRelative("qos");
        _rerouteTo = property.FindPropertyRelative("rerouteTo");

        Rect rect = new Rect(position.min.x, position.min.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);

        if (property.isExpanded)
        {
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(rect, _topic);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(rect, _qos);
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(rect, _rerouteTo);

            //add a button to test the topic
/*            rect.y += EditorGUIUtility.singleLineHeight + 2;
            if (GUI.Button(rect, "Connect"))
            {
                //ConnectToTopic(property);
            }
            rect.y += EditorGUIUtility.singleLineHeight + 2;
            if (GUI.Button(rect, "Disconnect"))
            {
                //DisconnectFromTopic(property);
            }*/
        }
        //if(GUILayout.Button("Test"))
        //   Debug.Log("It's alive: " + _topic.stringValue);
        



    }

    //void ConnectToTopic(SerializedProperty prop)
    //{
    //    Debug.Log("Connecting");
    //    (prop.serializedObject.targetObject as MQTTSource).ConnectToBroker();
    //}
    //void DisconnectFromTopic(SerializedProperty prop)
    //{
    //    Debug.Log("Disconnecting");
    //    (prop.serializedObject.targetObject as MQTTSource).DisconnectFromBroker();
    //}

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
            height = height * 6;

        return height + 10;
    }

}
