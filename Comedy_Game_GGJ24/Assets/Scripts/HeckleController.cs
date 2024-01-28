using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeckleController : MonoBehaviour
{
    private List<Transform> crowdList = new List<Transform>();
    private List<KeyToPress> keyList = new List<KeyToPress>();
    [SerializeField] private GameObject textBubble;
    [SerializeField] private Vector3 bubbleOffset = new Vector3(0, 1, 0);
    private Camera camera;
    [SerializeField] private float timeBetweenKeys = 1;
    [SerializeField] private int keysToSpawn = 3;
    private Queue<KeyCode> keyCodeQueue = new Queue<KeyCode>();
    private int keysSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }


    private void StartHeckle()
    {
        OpenBubble();
        StartCoroutine(DoTheHeckle());


    }

    private IEnumerator WaitForKeyPress()
    {
        while (keyCodeQueue.Count > 0 || keysSpawned < keysToSpawn)
        {
            if (keyCodeQueue.Count > 0) 
            {
                yield return WaitForKeyPress(keyCodeQueue.Peek());
            }

            yield return null;
        }
    }

    private IEnumerator WaitForKeyPress(KeyCode keyPress)
    {
        while(!Input.GetKey(keyPress))
        {
            yield return null;
        }

        keyCodeQueue.Dequeue();

    }

    private IEnumerator DoTheHeckle()
    {
        float time = 0;

        while (keysSpawned < keysToSpawn)
        {
            SpawnKeyAfterTime(time);
            yield return null;
        }

        CloseBubble();
    }

    private void OpenBubble()
    {
        Transform t = GetRandomHeckler();
        textBubble.transform.position = camera.WorldToScreenPoint(t.position + bubbleOffset);
        textBubble.SetActive(true);
    }

    private void CloseBubble()
    {
        textBubble.SetActive(false);
    }

    private void SpawnKeyAfterTime(float time)
    {
        time += Time.deltaTime;

        if (time >= timeBetweenKeys)
        {
            SpawnKey();
            time = 0;
        }
    }

    private void SpawnKey()
    {
        keyCodeQueue.Enqueue(Instantiate(GetRandomKey(), textBubble.transform, false).GetKey());
        keysToSpawn++;
    }

    private KeyToPress GetRandomKey()
    {
        return GetRandomIndex<KeyToPress>.GetRandomIndexFromList(keyList);
    }

    private Transform GetRandomHeckler()
    {
        return GetRandomIndex<Transform>.GetRandomIndexFromList(crowdList);
    }
}
