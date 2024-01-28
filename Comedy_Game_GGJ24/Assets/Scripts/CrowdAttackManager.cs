using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAttackManager : MonoBehaviour
{
    public enum AttackDir { LEFT, CENTER, RIGHT };

    [SerializeField] private GameObject launcher_L, launcher_C, launcher_R;
    [SerializeField] private GameObject crowdFX_L, crowdFX_C, crowdFX_R;

    private GameObject activeLauncher;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartAttack();
        }
    }

    public void StartAttack()
    {
        // generate attack direction
        AttackDir attackDirection = (AttackDir)Random.Range(0, 3);
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
    }
}
