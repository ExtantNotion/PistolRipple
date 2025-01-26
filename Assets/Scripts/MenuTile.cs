using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTile : MonoBehaviour
{

    public bool showLevelPage = false;
    public bool showCreditsPage = false;
    public bool showOptionsPage = false;
    public bool returnToMainMenu = false;
    
    public bool restartLevel = false;
    public bool quitToMenu = false;

    public RectTransform thisRectTransform;
    Vector2 thisRectSize;


    // Start is called before the first frame update
    void Start()
    {
        thisRectSize = thisRectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverEffect(bool state)
    {
        if (state)
        {
            thisRectTransform.DOSizeDelta(thisRectSize + new Vector2(20f, 20f),0.25f,false).SetEase(Ease.OutBack);
        }
        else
        {
            thisRectTransform.DOSizeDelta(thisRectSize, 0.25f, false).SetEase(Ease.OutBack);
        }
    }

    public void ActivateHover()
    {
        HoverEffect(true);  
    }
    public void DisableHover()
    {
        HoverEffect(false);
    }

    public void Action()
    {
        if (showLevelPage)
            MenuHandler.Instance.UpdateCurrentPage("Level");
        else if (showCreditsPage)
            MenuHandler.Instance.UpdateCurrentPage("Credits");
        else if (showOptionsPage)
            MenuHandler.Instance.UpdateCurrentPage("Options");
        else if (returnToMainMenu)
            MenuHandler.Instance.UpdateCurrentPage("Main");
        else if (restartLevel)
        {
            Debug.Log("Restarrting");
            var num = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(num);
        }
            
        else if (quitToMenu)
            SceneManager.LoadScene(0);
    }
}
