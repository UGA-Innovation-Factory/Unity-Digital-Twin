using M2MqttUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json.Linq;
using System.Linq;



public class MQTTSource : M2MqttUnityClient
{
    [Serializable]
    public class MQTTTopic
    {
        public enum qos_levels {
            AtMostOnce0 = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
            AtLeastOnce1 = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
            ExactlyOnce2 = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE}
        [Tooltip("Topic to subscribe to")]
        public string topic;
        [Tooltip("Quality of Service level")]
        public qos_levels qos;
    }

    [Serializable]
    protected class Connection
    {
        public MQTTTopic topic;
        public GameObject rerouteTo;
    }


    [Header("MQTT Source Configurations")]
    [Tooltip("Address of the MQTT broker")]
    [SerializeField]
    protected string brokerIpAddress = "localhost";
    [Tooltip("Port of the MQTT broker (usually 1883 or 8883)")]
    [SerializeField]
    protected int Port = 1883;
    [Tooltip("Whether the Broker is using a secure connection or not")]
    [SerializeField]
    protected bool SecureConnecting = false;
    [Tooltip("Username to connect to the MQTT broker")]
    [SerializeField]
    protected string UserName = null;
    [Tooltip("Password to connect to the MQTT broker")]
    [SerializeField]
    protected string Password = null;

    [Header("MQTT Topics")]
    [Tooltip("List of topics to subscribe to")]
    [SerializeField]
    protected Connection[] topics;

    private Dictionary<string, GameObject> topicReference;
    private void initializeDictionary()
    {
        topicReference = new Dictionary<string, GameObject>();
        foreach (Connection connection in topics)
        {
            topicReference.Add(connection.topic.topic, connection.rerouteTo);
        }
    }

    /// <summary>
    /// Override/set parameters from the parent Mqtt class to avoid having to manually set them every time.
    /// Called on Start().
    /// </summary>
    protected void overrideParams()
    {
        brokerAddress = brokerIpAddress;
        brokerPort = Port;
        isEncrypted = SecureConnecting;
        timeoutOnConnection = MqttSettings.MQTT_CONNECT_TIMEOUT;
        autoConnect = true;
        mqttUserName = UserName;
        mqttPassword = Password;
    }



    // Start is called before the first frame update
    protected override void Start()
    {
        overrideParams();
        initializeDictionary();
        base.Start();
        Debug.Log("Starting MQTTSource");
    }


    // Override from parent to subscribe to specified Topic.
    protected override void SubscribeTopics()
    {
        List<string> topicList = new List<string>();
        List<byte> qosList = new List<byte>();
        foreach (Connection connection in topics)
        {
            topicList.Add(connection.topic.topic);
            qosList.Add((byte)connection.topic.qos);
        }
        client.Subscribe(topicList.ToArray(), qosList.ToArray());
        //client.Subscribe(new string[] { "#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

    }

    // Override from parent to unsubscribe from Topic when needed.
    protected override void UnsubscribeTopics()
    {
        List<string> topicList = new List<string>();
        foreach (Connection connection in topics)
        {
            topicList.Add(connection.topic.topic);
        }
        client.Unsubscribe(topicList.ToArray());
    }


    // Override from parent to process received messages from subscribed topics.
    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);

        //get dictionary from msg
        var flattened = JsonFlattener.DeserializeAndFlatten(msg);
        
        GameObject rerouteTo = topicReference[topic];
        rerouteTo.GetComponentInChildren<DataEntryPoint>().callAction(flattened);
    }
}
