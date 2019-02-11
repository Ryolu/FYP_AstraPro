using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBook : MonoBehaviour {

    private static GuideBook Instances;

    [SerializeField] private List<Sprite> images; // slot in image for up to 24 for the guidebook 
    private Image thisImage; //put image in
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

    //press to go to the next image
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

    //press to go to the previous image
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
	
}
