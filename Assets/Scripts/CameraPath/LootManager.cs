using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private bool mouseHasBeenDrag = false;

    void Start ()
    {
    }
	
	void Update ()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Loot");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            if (Input.GetMouseButtonUp(0) && !mouseHasBeenDrag)
            {
                Debug.Log(hit.collider.name);
                DestroyImmediate(hit.collider.gameObject);
            }

            
        }

        if (Input.GetMouseButtonUp(0)) mouseHasBeenDrag = false;
    }

    void OnGUI()
    {
        //Event e = Event.current;

        //switch (e.type)
        //{
        //    case EventType.MouseDrag:   mouseHasBeenDrag = true; break;
        //    //case EventType.MouseUp:     mouseHasBeenDrag = false; break;
        //}

        if (Event.current.type == EventType.MouseDrag)
            mouseHasBeenDrag = true;
    }
}
