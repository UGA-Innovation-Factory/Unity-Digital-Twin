using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TODOMessageRemoverDELETELATER : MonoBehaviour
{
    // This script is used to remove the big TODO message from the screen
    // For the love of god remove this script and the label once is not being used anymore
    // and before moving to production or whathever is done with this project
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Label todoLabel = root.Q<Label>("todo-message");
        todoLabel.visible = false;

        Debug.LogWarning("TODOMessageRemoverDELETELATER still exists and is instanciated. It should be removed before finishing work with this project!!!!!");
    }
}
