using M2MqttUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

/// <summary>
/// Mqtt Unity client modified to connect specifically to the HiveMQ server used for this demo
/// </summary>
public class HiveMQConnector : M2MqttUnityClient, IDataPublisher
{
    [Header("HiveMQ Configurations")]
    [Tooltip("Topic to subscribe for incoming data")]
    public string Topic = "";

    //PubSub for other parts of Unity code --------------------------------
    private Action<string[]> PublishStringArray;
    protected void PublishString(string outString)
    {
        try
        {
            string[] string_array = outString.Split(',');
            PublishStringArray(string_array);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// Subscribes a function to this instance of the HiveMQConnector, which should receive a list of strings as input.
    /// </summary>
    public void Subscribe(Action<string[]> function)
    {
        PublishStringArray += function;
    }

    //HiveMQ connection related code -------------------------------------
    /// <summary>
    /// Override/set parameters from the parent Mqtt class to avoid having to manually set them every time.
    /// Called on Start().
    /// </summary>
    protected void overrideParams()
    {
        brokerAddress = "41c1ed9ae7f140168d7f1515d3eb3fab.s2.eu.hivemq.cloud";
        brokerPort = 8883;
        isEncrypted = true;
        timeoutOnConnection = MqttSettings.MQTT_CONNECT_TIMEOUT;
        autoConnect = true;
        mqttUserName = "UGAMQTT";
        mqttPassword = "STEM2023";
    }

    /// <summary>
    /// Monobehaviour Start() function. Calls overrideParams() and then parent Mqtt class Start().
    /// </summary>
    protected override void Start()
    {
        overrideParams();
        base.Start();
    }

    // Override from parent to subscribe to specified Topic.
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
    }

    // Override from parent to unsubscribe from Topic when needed.
    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { Topic });
    }

    // Override from parent to process received messages from subscribed topics.
    protected override void DecodeMessage(string topic, byte[] message)
    {
        string msg = System.Text.Encoding.UTF8.GetString(message);
        Debug.Log("Received1: " + msg);
        PublishString(msg);
    }


}
