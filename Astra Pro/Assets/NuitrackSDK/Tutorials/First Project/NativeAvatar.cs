﻿#region Description

// The script performs a direct translation of the skeleton using only the position data of the joints.
// Objects in the skeleton will be created when the scene starts.

#endregion


using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Nuitrack/Example/TranslationAvatar")]
public class NativeAvatar : MonoBehaviour
{
    string message = "";

    public nuitrack.JointType[] typeJoint;
    GameObject[] CreatedJoint;
    public GameObject PrefabJoint;
    public GameObject JointWithCollider;

    public GestureTrigger chop;
    public GestureTrigger stir;

    void Start()
    {
        CreatedJoint = new GameObject[typeJoint.Length];
        for (int q = 0; q < typeJoint.Length; q++)
        {
            if(typeJoint[q] == nuitrack.JointType.LeftWrist || typeJoint[q] == nuitrack.JointType.RightWrist)
                CreatedJoint[q] = Instantiate(JointWithCollider, transform);
            else
            {
                CreatedJoint[q] = Instantiate(PrefabJoint, transform);

                if (typeJoint[q] == nuitrack.JointType.Torso)
                {
                    GestureDetection detector = GetComponent<ChopDetection>();
                    detector.first = Instantiate(chop, CreatedJoint[q].transform.position + new Vector3(0, 0.02f, 0), Quaternion.identity, CreatedJoint[q].transform);
                    detector.second = Instantiate(chop, CreatedJoint[q].transform.position - new Vector3(0, .04f, 0), Quaternion.identity, CreatedJoint[q].transform);

                    detector = GetComponent<StirDetection>();
                    detector.first = Instantiate(stir, CreatedJoint[q].transform.position - new Vector3(0, 0, 0.225f), Quaternion.identity, CreatedJoint[q].transform);
                    detector.second = Instantiate(stir, CreatedJoint[q].transform.position - new Vector3(0, 0, 0.225f), Quaternion.Euler(0, 90, 0), CreatedJoint[q].transform);
                }
            }
        }
        message = "Skeleton created";
    }

    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            message = "Skeleton found";

            for (int q = 0; q < typeJoint.Length; q++)
            {
                nuitrack.Joint joint = skeleton.GetJoint(typeJoint[q]);
                Vector3 newPosition = 0.001f * joint.ToVector3();
                CreatedJoint[q].transform.localPosition = newPosition;
            }
        }
        else
        {
            message = "Skeleton not found";
        }
    }

    // Display the message on the screen
    void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 50;
        GUILayout.Label(message);
    }
}