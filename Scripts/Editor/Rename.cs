using UnityEditor;
using UnityEngine;


public class Rename : MonoBehaviour
{
    static Transform[] arr;
    static CarAI ca;
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Rename/Rename and Assign")]
    static void RenameChildren()
    {
        arr = GameObject.Find("AI_Path").GetComponentsInChildren<Transform>();
        GameObject[] wps = new GameObject[arr.Length-1];
        for (int i = 1; i < arr.Length; i++)
        {
            arr[i].gameObject.name = "WayPoint" + (i - 1);
            wps[i-1] = arr[i].gameObject;
        }

        ca = GameObject.Find("AI_Path").GetComponent<CarAI>();
        ca.waypoints = wps;


    }



}
/*
//-------------------------------------------------------------------------------
            if (arr[i].gameObject.name.Contains("("))
            {
                for(int k=1; k<arr.Length; i++){
                    if(arr[k].gameObject.name.Contains(arr[i].gameObject.name[8]))
                         arr[i].gameObject.transform.SetSiblingIndex(arr[i].gameObject.name

                }
               


            
            //------------------------------------------------------------------------------
*/