using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum StageSection { LEFT, CENTER, RIGHT };
    public StageSection currentSection;

    [SerializeField] private PlayerControls controls;
    [SerializeField] private PlayerMovement movement;
    private bool isAlive; public bool GetIsAlive() { return isAlive; }
    public void SetIsAlive(bool isAlive) { this.isAlive = isAlive; }
    private Vector3 spawnPoint;
    private Vector3 stagePoint; public void SetStagePoint(Vector3 stagePoint) { this.stagePoint = stagePoint; }
    public Action OnForceMoveComplete;

    [Button()]
    private void GetMissingComponents()
    {
        if (controls == null)
            controls = GetComponent<PlayerControls>();

        if (movement == null)
            movement = GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetMissingComponents();
        spawnPoint = transform.position;
        isAlive = true;
        DisablePlayer();
        currentSection = StageSection.CENTER;
    }

    public void EnablePlayer()
    {
        movement.ForceMovePlayerToPosition(stagePoint, OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        controls.EnableControls();
        OnForceMoveComplete?.Invoke();
    }

    public void DisablePlayer()
    {
        controls.DisableControls();
        movement.ForceMovePlayerToPosition(spawnPoint);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone_L"))
        {
            currentSection = StageSection.LEFT;
            Debug.Log("STAGE - " + currentSection);
        }
        else if (other.CompareTag("Zone_C"))
        {
            currentSection = StageSection.CENTER;
            Debug.Log("STAGE - " + currentSection);
        }
        else if (other.CompareTag("Zone_R"))
        {
            currentSection = StageSection.RIGHT;
            Debug.Log("STAGE - " + currentSection);
        }
    }
}
