using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance;

    public RectTransform mainPage;
    public RectTransform levelsPage;
    public RectTransform creditsPage;
    public RectTransform optionsPage;

    public RectTransform gameLogoRectTransform;
    private Vector2 gameLogoStartPos;

    public Image fader;

    private RectTransform currentSelectedPage;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Fader(true);

        gameLogoStartPos = gameLogoRectTransform.anchoredPosition;
        currentSelectedPage = mainPage;

        FloatingLogo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fader(bool state)
    {
        if (state)
        {
            fader.DOFade(0, 2f);
        }
        else
        {
            fader.DOFade(1, 2f);
        }
    }

    void FloatingLogo()
    {
        gameLogoRectTransform.DOAnchorPos(gameLogoStartPos + new Vector2(0, 50),1.25f,false).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    public void UpdateCurrentPage(string type)
    {
        currentSelectedPage.gameObject.SetActive(false);
        if (type == "Level")
        {
            //levelsPage.gameObject.SetActive(true);
            //currentSelectedPage = levelsPage;

            SceneManager.LoadScene(0);
        }
        else if (type == "Credits")
        {
            creditsPage.gameObject.SetActive(true);
            currentSelectedPage = creditsPage;
        }
        else if (type == "Options")
        {
            optionsPage.gameObject.SetActive(true);
            currentSelectedPage = optionsPage;
        }
        else if (type == "Main")
        {
            mainPage.gameObject.SetActive(true);
            currentSelectedPage = mainPage;
        }
    }
}
