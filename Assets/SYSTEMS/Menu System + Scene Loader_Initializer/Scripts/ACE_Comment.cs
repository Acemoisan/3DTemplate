using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACE_Comment : MonoBehaviour
{
    // This TextArea allows for multiple lines of text to be displayed in the Unity Inspector.
    [TextArea(5, 10)]
    public string comment = "Enter your comment here...";
}
