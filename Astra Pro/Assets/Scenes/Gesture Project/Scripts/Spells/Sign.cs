using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {

    public List<SignPoint> points;
    [HideInInspector]
    public List<GameObject> lines;
    public int numLines;
    public GameObject linePrefab;

    public bool looped;
	
	// Update is called once per frame
	void Update () {

        if (lines.Count == numLines)
            DoSign();
	}

    void DoSign()
    {
        Debug.Log("Sign is complete");
    }

    public void DrawLine(Vector3 first, Vector3 second)
    {
        GameObject go = Instantiate(linePrefab);
        LineRenderer line = go.GetComponent<LineRenderer>();
        Vector3[] segments = new Vector3[2];
        segments[0] = first;
        segments[1] = second;
        line.SetPositions(segments);
        go.transform.position += new Vector3(0, 0, -1);

        lines.Add(go);
    }

    public void DrawSpecial()
    {
        GameObject go = Instantiate(linePrefab);
        LineRenderer line = go.GetComponent<LineRenderer>();
        Vector3[] segments = new Vector3[2];
        segments[0] = points[0].gameObject.transform.position;
        segments[1] = points[points.Count - 1].gameObject.transform.position;
        line.SetPositions(segments);
        go.transform.position += new Vector3(0, 0, -1);

        lines.Add(go);
    }
}
