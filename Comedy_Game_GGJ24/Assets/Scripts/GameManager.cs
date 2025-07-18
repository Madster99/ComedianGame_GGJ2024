using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int GetNumberOfPlayers() { return numberOfPlayers; }
    public static GameManager instance;

    [SerializeField] private Transform stagePoint;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform playerContainer;
    [SerializeField, Range(1, 4)] private int numberOfPlayers = 1;
    private int currentPlayer;
    private List<Player> playerList = new List<Player>();
    public UnityEvent<Player> HecklePlayer;

    public bool gameStateActive = false;
    [Range(0, 1)] public float endStateMeter; // if you hit 1, game over
    public Cane_Controller cane;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            Destroy(instance);
    }

    [Button()]
    public void SpawnPlayers()
    {
        DestroyPlayers();
        Player temp;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            temp = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity, playerContainer);
            temp.name = "Player " + (i + 1);
            temp.SetStagePoint(stagePoint.position);
            playerList.Add(temp);
        }
    }

    private void DestroyPlayers()
    {
        Player temp;

        while (playerList.Count > 0)
        {
            temp = playerList[0];
            playerList.RemoveAt(0);
            Destroy(temp.gameObject);
        }
    }

    [Button()]
    public void StartGame()
    {
        currentPlayer = 0;
        PlayerStart();
        gameStateActive = true;
    }

    public void SetNumberOfPlayers(int i) { numberOfPlayers = i; }

    public Player GetActivePlayer()
    {
        return playerList[currentPlayer];
    }

    public void RemoveActivePLayerFromGame()
    {
        RemovePlayerFromGame(GetActivePlayer());
    }

    public void RemovePlayerFromGame(Player player)
    {
        if (playerList.Contains(player))
        {
            player.DisablePlayer();
            playerList.Remove(player);

            //Check For Lose Condition
            //Could be if last player alive or go until last player screws up
        }
    }

    private void StartHeckle()
    {
        GetActivePlayer().OnForceMoveComplete -= StartHeckle;

        //Start the heckling
        HecklePlayer?.Invoke(GetActivePlayer());
        Debug.Log("Heckle now");
    }

    [Button()]
    public void EnableNextPlayer()
    {
        if (playerList.Count == 1)
            return;

        GetActivePlayer().DisablePlayer();
        currentPlayer++;

        if (currentPlayer >= playerList.Count)
            currentPlayer = 0;

        PlayerStart();
    }

    private void PlayerStart()
    {
        GetActivePlayer().OnForceMoveComplete += StartHeckle;
        GetActivePlayer().EnablePlayer();
    }

    public void UpdateCane()
    {
        cane.caneAggro = endStateMeter;
        if (endStateMeter >= 1f)
        {
            StartCoroutine(Restart());
        }
    }

    public IEnumerator Restart()
    {
        GetActivePlayer().transform.parent = cane.transform.GetChild(0);
        GetActivePlayer().transform.position = Vector3.zero;
        GetActivePlayer().DisablePlayer();
        yield return new WaitForSeconds(3.0f);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
