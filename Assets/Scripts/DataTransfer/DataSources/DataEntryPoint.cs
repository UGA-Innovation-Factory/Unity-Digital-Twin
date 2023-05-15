using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEntryPoint : MonoBehaviour
{
    private Action<Dictionary<string, string>> action;

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
        //Debug.Log("DataEntryPoint: " + transform.root.gameObject.name + data.Keys);
    }
}
