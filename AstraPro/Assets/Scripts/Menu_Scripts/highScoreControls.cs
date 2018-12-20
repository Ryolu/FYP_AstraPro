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

    void Start()
    {
        //pageSize = new Vector2(Screen.width, Screen.height);
        //defaultSize = new Vector2(Screen.width / colsNumber, Screen.height / rowsNumber);
        //
        //Vector2 halfAdd = new Vector2(defaultSize.x / 2, -defaultSize.y / 2);
        //
        //int imagesOnPage = rowsNumber * colsNumber;
        //numberOfPages = (int)Mathf.Ceil((float)spriteCollection.Length / imagesOnPage);
        //
        //int imageIndex = 0;
        //
        //for (int p = 0; p < numberOfPages; p++)
        //{
        //    int imagesOnCurrentPage = Mathf.Min(spriteCollection.Length - p * imagesOnPage, imagesOnPage);
        //
        //    for (int i = 0; i < imagesOnCurrentPage; i++)
        //    {
        //        GameObject currentItem = Instantiate(imageItemPrefab);
        //        currentItem.transform.SetParent(content, false);
        //
        //        RectTransform currentRect = currentItem.GetComponent<RectTransform>();
        //        currentRect.sizeDelta = defaultSize;
        //
        //        float X = pageSize.x * p + defaultSize.x * (i % colsNumber);
        //        float Y = defaultSize.y * (i / colsNumber);
        //
        //        currentRect.anchoredPosition = new Vector2(X, -Y) + halfAdd;
        //
        //        Image currentImage = currentItem.GetComponent<Image>();
        //        currentImage.sprite = spriteCollection[imageIndex];
        //        imageIndex++;
        //
        //        //ImageItem currentImageItem = currentItem.GetComponent<ImageItem>();
        //        //currentImageItem.OnClick += CurrentImageItem_OnClick;
        //    }
        //}
        //
        //content.sizeDelta = new Vector2(content.rect.width, -defaultSize.y * 11);
        ////content.sizeDelta = new Vector2(1000, 1000);
        //
        //if (numberOfPages > 1)
        //    scrollStep = 1f / (numberOfPages - 1);

        generatedSprites = new List<LayoutElement>();
        
        for (int i = 0; i < spriteCollection.Length; i++)
        {
            GameObject currentItem = Instantiate(imageItemPrefab);
            currentItem.transform.SetParent(content, false);
            HighScore.instance.highestScoreText[i] = currentItem.transform.GetChild(0).GetComponent<Text>();

            LayoutElement element = currentItem.GetComponent<LayoutElement>();
            generatedSprites.Add(element);
            
            element.preferredWidth = elementX;
            element.preferredHeight = elementY;

            Image currentImage = currentItem.GetComponent<Image>();
            currentImage.sprite = spriteCollection[i];
        }
        HighScore.instance.OnHighScore();

        content.sizeDelta = new Vector2(content.rect.width, (elementY + 25) * 10 + 900);

        //ImageItem currentImageItem = currentItem.GetComponent<ImageItem>();
        //currentImageItem.OnClick += CurrentImageItem_OnClick;

        NuitrackManager.onNewGesture += NuitrackManager_onNewGesture;        
    }

    private void OnDestroy()
    {
        NuitrackManager.onNewGesture -= NuitrackManager_onNewGesture;
    }

    //private void CurrentImageItem_OnClick(ImageItem currentItem)
    //{
    //    if (currentViewMode == ViewMode.Preview)
    //    {
    //        t = 0;
    //        currentViewMode = ViewMode.View;
    //        selectedItem = currentItem;

    //        canvasGroup.interactable = false;
    //        selectedItem.interactable = false;

    //        selectedItem.transform.SetParent(viewRect, true);
    //        defaultPosition = selectedItem.transform.localPosition;
    //    }
    //}

    private void Update()
    {
        //scrollRect.verticalScrollbar.value = Mathf.Lerp(scrollRect.verticalScrollbar.value, scrollStep, Time.deltaTime * scrollSpeed);
        //switch (currentViewMode)
        //{
        //    case ViewMode.View:

        //        if (t < 1)
        //        {
        //            t += Time.deltaTime * animationSpeed;

        //            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, t);

        //            selectedItem.image.rectTransform.sizeDelta = Vector2.Lerp(selectedItem.image.rectTransform.sizeDelta, pageSize, t);
        //            selectedItem.transform.localPosition = Vector2.Lerp(selectedItem.transform.localPosition, Vector2.zero, t);
        //        }

        //        break;

        //    case ViewMode.Preview:

        //        if (animated)
        //        {
        //            if (t > 0)
        //            {
        //                t -= Time.deltaTime * animationSpeed;

        //                canvasGroup.alpha = Mathf.Lerp(1, canvasGroup.alpha, t);

        //                selectedItem.image.rectTransform.sizeDelta = Vector2.Lerp(defaultSize, selectedItem.image.rectTransform.sizeDelta, t);

        //                selectedItem.transform.localPosition = Vector2.Lerp(defaultPosition, selectedItem.transform.localPosition, t);
        //                selectedItem.transform.localRotation = Quaternion.Lerp(Quaternion.identity, selectedItem.transform.localRotation, t);
        //                selectedItem.transform.localScale = Vector3.Lerp(Vector3.one, selectedItem.transform.localScale, t);
        //            }
        //            else
        //            {
        //                selectedItem.transform.SetParent(content, true);
        //                selectedItem.interactable = true;
        //                canvasGroup.interactable = true;
        //                selectedItem = null;
        //                animated = false;
        //            }
        //        }
        //        else
        //            scrollRect.verticalScrollbar.value = Mathf.Lerp(scrollRect.verticalScrollbar.value, scrollStep * currentPage, Time.deltaTime * scrollSpeed);

        //        break;
        //}


    }

    private void NuitrackManager_onNewGesture(nuitrack.Gesture gesture)
    {
        Debug.Log("YOU JIAO");
        if (gesture.Type == nuitrack.GestureType.GestureSwipeUp)
        {
            Debug.Log("please work");
            scrollRect.velocity = new Vector2(0, 1000);
        }
        
        //    currentPage = Mathf.Clamp(++currentPage, 0, numberOfPages);
        //
        if (gesture.Type == nuitrack.GestureType.GestureSwipeDown)
        {
            Debug.Log("please work down");
            scrollRect.velocity = new Vector2(0, -1000);
        }

        //    currentPage = Mathf.Clamp(--currentPage, 0, numberOfPages);

    }

    public void ChangeFocus()
    {
        focus = Mathf.FloorToInt(( 1 - scrollRect.verticalScrollbar.value ) * 10);
        Debug.Log(focus);

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
