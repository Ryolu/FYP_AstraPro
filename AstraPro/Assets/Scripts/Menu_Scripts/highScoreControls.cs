using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highScoreControls : MonoBehaviour
{
    enum ViewMode { Preview, View };
    ViewMode currentViewMode = ViewMode.Preview;

    [Header("Visualization")]

    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Sprite[] spriteCollection;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject imageItemPrefab;
    
    [Header("Element Size")]
    [SerializeField] float elementX;
    [SerializeField] float elementY;
    [SerializeField] float focusX;
    [SerializeField] float focusY;

    int focus = 0;
    List<LayoutElement> generatedSprites;

    //Generate all score and text into the highscore menus
    void Start()
    {
        generatedSprites = new List<LayoutElement>();
        
        for (int i = 0; i < spriteCollection.Length; i++)
        {
            GameObject currentItem = Instantiate(imageItemPrefab);
            currentItem.transform.SetParent(content, false);
            HighScore.Instance.highestScoreText[i] = currentItem.transform.GetChild(0).GetComponent<Text>();

            LayoutElement element = currentItem.GetComponent<LayoutElement>();
            generatedSprites.Add(element);
            
            element.preferredWidth = elementX;
            element.preferredHeight = elementY;

            Image currentImage = currentItem.GetComponent<Image>();
            currentImage.sprite = spriteCollection[i];
        }
        HighScore.Instance.OnHighScore();

        content.sizeDelta = new Vector2(content.rect.width, (elementY + 25) * 10 + 900);

        NuitrackManager.onNewGesture += NuitrackManager_onNewGesture;        
    }

    private void OnDestroy()
    {
        NuitrackManager.onNewGesture -= NuitrackManager_onNewGesture;
    }

    //Swipe Gesture to move up and down
    private void NuitrackManager_onNewGesture(nuitrack.Gesture gesture)
    {
        if (gesture.Type == nuitrack.GestureType.GestureSwipeUp)
        {
            scrollRect.velocity = new Vector2(0, 1000);
        }
        
        if (gesture.Type == nuitrack.GestureType.GestureSwipeDown)
        {
            scrollRect.velocity = new Vector2(0, -1000);
        }
    }

    //Pin point to the middle of the target of score
    public void ChangeFocus()
    {
        focus = Mathf.FloorToInt(( 1 - scrollRect.verticalScrollbar.value ) * 10);

        if (focus == 10)
            focus = 9;
        
        for (int i = 0; i < generatedSprites.Count; i++)
        {
            if (i == focus)
            {
                generatedSprites[i].preferredWidth = focusX * 1.25f;
                generatedSprites[i].preferredHeight = focusY * 1.25f;
            }
            else
            {
                generatedSprites[i].preferredWidth = elementX;
                generatedSprites[i].preferredHeight = elementY;
            }
        }
    }
}
