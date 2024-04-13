using TMPro;
using UnityEngine;

public class TextTest : MonoBehaviour
{
    TextStyle style =
       new TextStyle(46f, 46f, 0f, Color.yellow, HorizontalAlignmentOptions.Geometry,
           1f, 0.7f, 0.5f, new Vector3(0f, 3f, 0f), new Vector3(0f, 3f, 0f), new Vector3(0f, -4f, 0f));

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
        }
    }
}

