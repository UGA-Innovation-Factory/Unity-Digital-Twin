using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used in DisplayDataContainer.cs
/// Class used to create a list which defines the name and unit of data.
/// These values are then displayed on the UI data list
/// </summary>
[Serializable]
public class DataStructure
{
    public string m_DataName;
    public string m_DataUnit;
}
