﻿using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static CameraShake instance;
    public Camera mainCam;

    float shakeAmount = 0f;

    private void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
        }
    }

    public void Shake(float amt, float duration)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0f, 0.01f);
        Invoke("StopShake", duration);
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;
            

            mainCam.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
