using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPoint : MonoBehaviour {

    public int numTouches = 0;
    [HideInInspector] public bool touched = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Toucherino");
        Sign parent = GetComponentInParent<Sign>();
        List<SignPoint> pointList = parent.points;

        for (int i = 0; i < parent.points.Count; i++)
        {
            if (pointList[i].gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                if (pointList[i].touched)
                    continue;
                else
                    return;
            }
            else
            {
                if (i > 0)
                    parent.DrawLine(gameObject.transform.position, pointList[i - 1].gameObject.transform.position);

                // Special case for looped signs
                if (parent.looped && touched)
                    parent.DrawSpecial();

                touched = true;
                Debug.Log("Tuts mah barreh");
            }
        }
    }
}
