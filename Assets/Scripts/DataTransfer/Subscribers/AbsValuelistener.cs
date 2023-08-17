using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Value listener/subscriber abstract class.
/// Abstract class that defines the basic structure the different data subscribers on an object.
/// It hooks up to the DataEntryPoint in the parent object and subscribes to data updates, which are sent in the form of a dictionary.
/// Because of this, subscriptions are made through a callback function and a dictionary key.
/// </summary>
public abstract class AbsValuelistener : MonoBehaviour {

    //[Header("OLD - Data publisher")]
    //[SerializeField]
    //[Tooltip("Data publisher to subscribe to using it's Subscribe() function")]
    //protected HiveMQConnector _DataPublisher;
    //[SerializeField]
    //[Tooltip("Index in the published list of the data that should be used")]
    //protected int _DataIndex;


    [Header("Data publisher")]
    [SerializeField]
    [Tooltip("Data entry point. For visualization only")]
    protected DataEntryPoint _DataEntryPoint;
    //[SerializeField]
    //[Tooltip("Data key of the value to be used")]
    //protected string _DataKey;


    /// <summary>
    /// Gets a reference to the DataEntryPoint in the parent object.
    /// This object is used as a "pipeline" from the DataObject's data sources into this object 
    /// </summary>
    protected virtual void Start()
    {
        _DataEntryPoint = GetComponentInParent<DataEntryPoint>();
    }

    /// <summary>
    /// Wrapper to facilitate the subscription to the Data entry point.
    /// Based on a callback and the corresponding dictionary key, it subscribes to the Data source and triggers the callback on data updates.
    /// </summary>
    /// <param name="_dataKey">Dictionary's key to listen for data updates</param>
    /// <param name="callback">Function to call on data updates</param>
    protected void SubscribeToData(string _dataKey, Action<string> callback)
    {
        if (_dataKey == null)
            throw new Exception("Attempted to subscribe with a Null key");

        // Subscribe to the data entry point by passing a lambda function that calls the callback with the data from the entrypoint data dictionary
        Action<Dictionary<string, string>> lamb = (Dictionary<string, string> dataDictionary) =>
        {
            callback(dataDictionary[_dataKey]);
        };
        _DataEntryPoint.Subscribe(lamb);
    }

}
