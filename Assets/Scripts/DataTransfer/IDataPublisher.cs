using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Currently unused, should consider removal.
/// Original purpose was to use as an interface to quickly test different DataPublisher-s.
/// This was better achieved through the abstract class AValueListener.
/// Implemented by HiveMQConnector and SocketTest
/// </summary>
public interface IDataPublisher
{

    public void Subscribe(Action<string[]> function);
}
