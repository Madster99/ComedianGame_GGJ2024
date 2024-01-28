using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageHazardManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hazardPrefab;
    public enum HazardDir { LEFT, CENTER, RIGHT };

    [SerializeField] private Transform hazard_L_spawn, hazard_L_telegraph, hazard_L_end;
    [SerializeField] private Transform hazard_C_spawn, hazard_C_telegraph, hazard_C_end;
    [SerializeField] private Transform hazard_R_spawn, hazard_R_telegraph, hazard_R_end;

    public float timer_toTelegraph, timer_toEnd, timer_toSpawn, timer_AttackDelay;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartStageHazard();
        }
    }

    public void StartStageHazard()
    {
        // generate hazard direction
        HazardDir hazardDirection = (HazardDir)Random.Range(0, 3);
        Debug.Log(hazardDirection + " HAZARD INCOMING");
        switch (hazardDirection)
        {
            case HazardDir.LEFT:
                StartCoroutine(StageHazardAttack(hazard_L_spawn, hazard_L_telegraph, hazard_L_end));
                break;
            case HazardDir.CENTER:
                StartCoroutine(StageHazardAttack(hazard_C_spawn, hazard_C_telegraph, hazard_C_end));
                break;
            case HazardDir.RIGHT:
                StartCoroutine(StageHazardAttack(hazard_R_spawn, hazard_R_telegraph, hazard_R_end));
                break;
        }
    }

    public IEnumerator StageHazardAttack(Transform spawn, Transform telegraph, Transform end)
    {
        GameObject currentHazard = Instantiate(hazardPrefab[Random.Range(0, hazardPrefab.Length)], spawn);
        currentHazard.transform.DOMove(telegraph.transform.position, timer_toTelegraph);
        yield return new WaitForSeconds(timer_AttackDelay);
        currentHazard.transform.DOMove(end.transform.position, timer_toEnd);
        yield return new WaitForSeconds(2.5f);
        currentHazard.transform.DOMove(spawn.transform.position, timer_toSpawn);
        yield return new WaitForSeconds(timer_toSpawn);
        Destroy(currentHazard);
        yield return null;
    }
}
