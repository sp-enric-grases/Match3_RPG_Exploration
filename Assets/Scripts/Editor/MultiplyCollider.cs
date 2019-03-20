using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiplyCollider
{
    [MenuItem("Stupid Tools/Get Box Collider bigger", false, 10)]
    public static void ShowInfo()
    {
        foreach (GameObject go in Selection.objects)
        {
            if (go.GetComponent<BoxCollider>())
                go.GetComponent<BoxCollider>().size *= 1.3f;
        }
    }
}
