﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XBOXJoystickInput : PlayerInput
{

    [Header("-------key----------")]
    public string keyX = "joystick button 2";
    public string keyY = "joystick button 3";
    public string keyB = "joystick button 1";
    public string keyA = "joystick button 0";

    [Header("----------ex key---------")]
    public string keyR1 = "joystick button 5";
    public string keyR2 = "RT Axis";
    public string keyR3 = "joystick button 8";
    public string keyL1 = "joystick button 4";
    public string keyL2 = "LT Axis";
    public string keyL3 = "jotstick button 9";


    private float targetUpValue;
    private float upValueVelocity;

    private float targetRightValue;
    private float rightValueVelocity;

    private float targetCUpValue;
    private float cUpValueVelocity;

    private float targetCRightValue;
    private float cRightValueVelocity;

    void Update()
    {

        InputHandler();

        if (!inputEnable)
        {
            targetRightValue = 0f;
            targetUpValue = 0f;
        }


        UpValue = Mathf.SmoothDamp(UpValue, targetUpValue, ref upValueVelocity, 0.1f);
        RightValue = Mathf.SmoothDamp(RightValue, targetRightValue, ref rightValueVelocity, 0.1f);

        CUpValue = Mathf.SmoothDamp(CUpValue, targetCUpValue, ref cUpValueVelocity, 0.3f);
        CRightValue = Mathf.SmoothDamp(CRightValue, targetCRightValue, ref cRightValueVelocity, 0.3f);

        Vector2 temp = SquareToCircle(new Vector2(UpValue, RightValue));

        SignalValueMagic = Mathf.Sqrt(temp.x * temp.x + temp.y * temp.y);
        SignalVec = temp.x * this.transform.forward + temp.y * this.transform.right;


        
    }

    private void InputHandler()
    {
        targetUpValue = Input.GetAxis("Left Y Axis");
        targetRightValue = Input.GetAxis("Left X Axis");

        targetCRightValue = Input.GetAxis("Right X Axis");
        targetCUpValue = Input.GetAxis("Right Y Axis");

        Jump = Input.GetKey(keyA);
        Attack = Input.GetKeyDown(keyX);
        Run = Math.Abs(Input.GetAxis(keyR2)) > 0f;
        Magic = Input.GetKeyDown(keyY);

    }



}
