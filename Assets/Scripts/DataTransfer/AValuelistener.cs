using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Value listener/subscriber abstract class.
/// Main purpose of this class is to easily change which class is in charge of publishing data,
/// defined on the class type of _DataPublisher.
/// It also defines the int field _DataIndex as every subscriber needs this field anyways.
/// </summary>
public abstract class AValuelistener : MonoBehaviour {

    [SerializeField]
    [Tooltip("Data publisher to subscribe to using it's Subscribe() function")]
    protected HiveMQConnector _DataPublisher;
    [SerializeField]
    [Tooltip("Index in the published list of the data that should be used")]
    protected int _DataIndex;
}
