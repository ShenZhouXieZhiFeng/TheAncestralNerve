using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActions : MonoBehaviour {

    public Button UpButton, DownButton, ResetButton;
    public float AddPreTimes = 0.5f;

    float currentTimeSpeed = 1;
    public float CurrentTimeSpeed
    {
        get
        {
            return currentTimeSpeed;
        }
        set
        {
            currentTimeSpeed = value;
            Time.timeScale = currentTimeSpeed;
        }
    }

    void Start ()
    {
        UpButton.onClick.AddListener(() =>
        {
            CurrentTimeSpeed += AddPreTimes;
        });
        DownButton.onClick.AddListener(() =>
        {
            CurrentTimeSpeed -= AddPreTimes;
        });
        ResetButton.onClick.AddListener(() =>
        {
            CurrentTimeSpeed = 1;
        });
    }

}
