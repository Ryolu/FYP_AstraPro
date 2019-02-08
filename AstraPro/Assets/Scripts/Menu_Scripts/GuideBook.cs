using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBook : MonoBehaviour {

    private static GuideBook Instances;

    [SerializeField] private List<Sprite> images;
    private Image thisImage;
    private int index = 0;

    private void Awake()
    {
        Instances = this;
    }


    // Use this for initialization
    void Start () {
        thisImage = GetComponent<Image>();
        thisImage.sprite = images[index];
    }

    public void NextImage()
    {
        index += 1;
        if (index >= images.Count)
        {
            index = images.Count;
            return;
        }
        thisImage.sprite = images[index];
    }

    public void PreviousImage()
    {
        index -= 1;
        if (index < 0)
        {
            index = 0;
            return;
        }
        thisImage.sprite = images[index];
    }
	// Update is called once per frame
	void Update () {
		
	}
}
