using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAttackManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private float timer;
    private float attackTimer;
    public Vector2 randomAttackRange;

    public enum AttackDir { LEFT, CENTER, RIGHT };
    AttackDir attackDirection;
    private string attackDir_String;

    [SerializeField] private GameObject launcher_L, launcher_C, launcher_R;
    [SerializeField] private GameObject crowdFX_L, crowdFX_C, crowdFX_R;

    private GameObject activeLauncher;

    private void Start()
    {
        attackTimer = Random.Range(randomAttackRange.x, randomAttackRange.y);
    }

    void Update()
    {
        if (gameManager.gameStateActive)
        {
            timer += Time.deltaTime;
            if (timer > attackTimer)
            {
                StartAttack();
            }
        }
    }

    public void StartAttack()
    {
        timer = 0f;
        // generate attack direction
        attackDirection = (AttackDir)Random.Range(0, 3);
        attackDir_String = attackDirection.ToString();
        Debug.Log(attackDirection + " ATTACK INCOMING");
        switch (attackDirection)
        {
            case AttackDir.LEFT:
                launcher_L.SetActive(true);
                launcher_C.SetActive(false);
                launcher_R.SetActive(false);
                crowdFX_L.SetActive(true);
                crowdFX_C.SetActive(false);
                crowdFX_R.SetActive(false);
                activeLauncher = launcher_L;
                StartCoroutine(AttackTimer(2.5f));
                break;
            case AttackDir.CENTER:
                launcher_L.SetActive(false);
                launcher_C.SetActive(true);
                launcher_R.SetActive(false);
                crowdFX_L.SetActive(false);
                crowdFX_C.SetActive(true);
                crowdFX_R.SetActive(false);
                activeLauncher = launcher_C;
                StartCoroutine(AttackTimer(2.5f));
                break;
            case AttackDir.RIGHT:
                launcher_L.SetActive(false);
                launcher_C.SetActive(false);
                launcher_R.SetActive(true);
                crowdFX_L.SetActive(false);
                crowdFX_C.SetActive(false);
                crowdFX_R.SetActive(true);
                activeLauncher = launcher_R;
                StartCoroutine(AttackTimer(2.5f));
                break;
        }
    }

    public IEnumerator AttackTimer(float t)
    {
        yield return new WaitForSeconds(t);
        activeLauncher.GetComponent<Launcher>().Launch();
        crowdFX_L.SetActive(false);
        crowdFX_C.SetActive(false);
        crowdFX_R.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        TryDamagingPlayer(0.4f);
        attackTimer = Random.Range(randomAttackRange.x, randomAttackRange.y);
    }

    public void TryDamagingPlayer(float damage)
    {
        if (gameManager.GetActivePlayer().currentSection.ToString() == attackDir_String)
        {
            gameManager.endStateMeter += damage;
            gameManager.UpdateCane();
        }
    }

}
