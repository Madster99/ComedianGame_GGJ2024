using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageHazardManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public HazardDir hazardDirection;

    private float timer;
    private float attackTimer;
    public Vector2 randomAttackRange;
    
    [SerializeField] private GameObject[] hazardPrefab;
    public enum HazardDir { LEFT, CENTER, RIGHT };
    private string hazardDir_String;

    [SerializeField] private Transform hazard_L_spawn, hazard_L_telegraph, hazard_L_end;
    [SerializeField] private Transform hazard_C_spawn, hazard_C_telegraph, hazard_C_end;
    [SerializeField] private Transform hazard_R_spawn, hazard_R_telegraph, hazard_R_end;

    public float timer_toTelegraph, timer_toEnd, timer_toSpawn, timer_AttackDelay;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        attackTimer = Random.Range(randomAttackRange.x, randomAttackRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStateActive)
        {
            timer += Time.deltaTime;
            if (timer > attackTimer)
            {
                StartStageHazard();
            }
        }
    }

    public void StartStageHazard()
    {
        timer = 0f;
        // generate hazard direction
        hazardDirection = (HazardDir)Random.Range(0, 3);
        hazardDir_String = hazardDirection.ToString();
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
        TryDamagingPlayer(0.4f);
        yield return new WaitForSeconds(1.5f);
        currentHazard.transform.DOMove(spawn.transform.position, timer_toSpawn);
        yield return new WaitForSeconds(timer_toSpawn);
        Destroy(currentHazard);
        attackTimer = Random.Range(randomAttackRange.x, randomAttackRange.y);
        yield return null;
    }

    public void TryDamagingPlayer(float damage)
    {
        if (gameManager.GetActivePlayer().currentSection.ToString() == hazardDir_String)
        {
            gameManager.endStateMeter += damage;
            gameManager.UpdateCane();
        }
    }
}
