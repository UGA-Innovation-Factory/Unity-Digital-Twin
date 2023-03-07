using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of collecting all the data received from a DataPublisher,
/// to be used by the UI controller InfoMenu.cs
/// </summary>
public class DisplayDataContainer : MonoBehaviour
{
    // Range of the list from which data should be pulled
    // Could be changed so it works through a selection instead of a single range
    [Tooltip("Index of the first dataStructure to be shown on the list")]
    [SerializeField] int indexStart = 0;
    [Tooltip("Index of the last dataStructure to be shown on the list")]
    [SerializeField] int indexEnd = 0;

    [Tooltip("List of data recieved by the data generator")]
    [SerializeField]
    DataStructure[] dataStructures;

    private string[] data;

    //private SocketTest dataListContainer;
    private HiveMQConnector dataPublisher;

    private DateTime latestMesageTime;

    /// <summary>
    /// Subscribes to the corresponding dataPublisher to gather data to display in the UI
    /// </summary>
    void Start()
    {
        data = new string[indexEnd-indexStart];
        latestMesageTime = DateTime.Now.AddSeconds(-20);

        // Gathers the list of DataStructure that define the data names and units
        // STORING THIS ON SocketTest IS REALLY OUTDATED AND BAD,
        // AND THE DataStructures LIST SHOULD BE DEFINED ON THIS CLASS OR SOMEWHERE ELSE 
        //dataListContainer = GetComponent<SocketTest>();
        //dataStructures = dataListContainer.GetDataStructures()[indexStart..indexEnd];

        dataPublisher = GetComponent<HiveMQConnector>();
        dataPublisher.Subscribe(updateDataList);
    }

    /// <summary>
    /// Function to be subscribed into the DataPublisher,
    /// which is then called whenever new data comes in as a
    /// list of strings.
    /// It also updates the latestMessageTime, which represents
    /// when was the last batch of data received
    /// </summary>
    void updateDataList(string[] new_data)
    {
        data = new_data[indexStart..indexEnd];
        latestMesageTime = DateTime.Now;
    }

    /// <returns>
    /// The dataStructure list
    /// </returns>
    public DataStructure[] GetDataStructures()
    {
        return dataStructures[indexStart..indexEnd];
    }

    /// <returns>
    /// The latest data received by the dataPublisher
    /// </returns>
    public string[] getData()
    {
        return data;
    }

    /// <summary>
    /// Gets the time since the latest batch of data was received from the subscribed dataPublisher.
    /// Can be used to define wether the corresponding Digital Twin is offline or has lost connection
    /// due to not having received any new data for a while
    /// </summary>
    /// <returns>Seconds since latest batch of data</returns>
    public int getTimeSinceLatestData()
    {
        TimeSpan ts = DateTime.Now - latestMesageTime;
        return (int)ts.TotalSeconds;
    }
}
