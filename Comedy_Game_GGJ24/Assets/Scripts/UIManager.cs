using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float timeToFade = 0.2f;
    
    [Button()]
    private void GetMissingComponents()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        
        if (canvasGroup == null)
            canvasGroup = FindObjectOfType<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetMissingComponents();
        canvasGroup.alpha = 1;
        OpenUI();
    }
    
    public void PlayerSelect(int playerCount)
    {
        gameManager.SetNumberOfPlayers(playerCount);
        gameManager.SpawnPlayers();
        CloseUI();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void OpenUI()
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.DOFade(1, timeToFade);
    }

    private void CloseUI()
    {
        canvasGroup.DOFade(0, timeToFade).OnComplete(OnCloseComplete);
    }
    
    private void OnCloseComplete()
    {
        canvasGroup.interactable = false;
        canvasGroup.gameObject.SetActive(false);
        gameManager.StartGame();
    }
}
