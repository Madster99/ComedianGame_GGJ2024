using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyToPress : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite;
    [SerializeField] private KeyCode keyCode; public KeyCode GetKey() { return keyCode; }
    private float timeToPress;
    public Action OnFail;
    private bool countDown;

    public KeyToPress(float timeToPress)
    {
        this.timeToPress = timeToPress;
        countDown = true;
    }

    private void Update()
    {
        if (!countDown)
            return;

        timeToPress -= Time.deltaTime;

        if (timeToPress <= 0)
        {
            countDown = false;
        }
    }

    public void OnKeyPressed()
    {

    }
}
