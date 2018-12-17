using UnityEngine;

public class MyToggle : MonoBehaviour
{
	private void Update ()
    {
	    if(Input.GetKeyUp(KeyCode.Space))
        {
            var outlines = transform.parent.GetComponentsInChildren<Outline>();

            foreach (var outline in outlines)
            {
                outline.GetComponent<Outline>().enabled = !outline.GetComponent<Outline>().enabled;
            }
        }
	}
}
