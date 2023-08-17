using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataEntryPoint : MonoBehaviour
{
    private Action<Dictionary<string, string>> action;
    public string[] topics;

    public void Subscribe(Action<Dictionary<string, string>> function)
    {
        action += function;
    }

    public void callAction(Dictionary<string, string> data)
    {
        if (action != null)
        {
            action(data);
        }

        //Log the name of gameobject this belongs to
        string[] dataKeys = data.Keys.ToArray();
        if (!dataKeys.Equals(topics)) 
        { 
            topics = dataKeys;
        }

        Debug.Log("DataEntryPoint: " + data.Keys.ToString());
        Debug.Log(data.Keys.GetType());
        Debug.Log("DataEntryPoint: " + dataKeys);
        Debug.Log(dataKeys.GetType());
    }
}
