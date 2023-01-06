using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;


public class flyPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public SteamVR_Action_Vector2 moveY;
    public GameObject left_hand;
    public GameObject right_hand;
    private float ySpeed = 0.0f;
    private float yAccel;
    public float ySensitivity;
    private float ySign;
    private float rxSign;
    private float rySign;
    private float xAngle = 0.0f;
    private float yAngle = 0.0f;
    private bool xTurning = false;
    private bool yTurning = false;
    private Vector3 direction = new Vector3(0, 1, 0);
    private Quaternion target = Quaternion.Euler(0,0,0);
    void Start()
    {
        
    }

    void MoveY(){
        if(Mathf.Abs(moveY.axis.y)<0.50 && ySpeed!=0){
            yAccel = 0;
            ySpeed -= ySensitivity*Mathf.Abs(ySign)/(ySign*2);
            if(ySign!=Mathf.Abs(ySpeed)/ySpeed){
                ySpeed=0;
            }
        }
        else{
            if(yAccel==0){
                ySign=Mathf.Abs(moveY.axis.y)/moveY.axis.y;
            }
            yAccel = ySensitivity *(moveY.axis.y);
            ySpeed += yAccel;
            if(Mathf.Abs(ySpeed)>6){
                ySpeed=Mathf.Abs(ySpeed)/ySpeed*6;
            }
        }
        //transform.position += xSpeed * Time.deltaTime * Vector3.ProjectOnPlane(new Vector3(1, 0, 0), Vector3.up);
        transform.position += -ySpeed * Time.deltaTime * Vector3.ProjectOnPlane(target *direction, Vector3.forward);
    }

    void ResetNeutral() {
        xTurning = false;
        yTurning = false;
        xAngle=0;
        yAngle=0;
        target = Quaternion.Euler(yAngle,0, -xAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 0.8f);
        MoveY();
    }

    void SetAngle() {
        if(xAngle==0){
            rxSign=Mathf.Abs(moveY.axis.x)/moveY.axis.x;
        }
        xAngle += moveY.axis.x*5;
        //NEW
        if(yAngle==0){
            rySign=Mathf.Abs(moveY.axis.y)/moveY.axis.y;
        }
        yAngle += moveY.axis.y*5;
        if(Mathf.Abs(xAngle)>15) {
            xAngle=15*rxSign;
        }
        if(Mathf.Abs(yAngle)>15) {
            yAngle=15*rySign;
        }
    }

    void TurnVertical() {
        if(Mathf.Abs(xAngle)/xAngle != rxSign){
            xAngle=0;
            target = Quaternion.Euler(0,0, 0);
            xTurning=false;
        }
        //NEW
        if(Mathf.Abs(yAngle)/yAngle != rySign){
            yAngle=0;
            target = Quaternion.Euler(0,0, 0);
            yTurning=false;
        }
        //
        else{
            if(rxSign >=0){
                target = Quaternion.Euler(0,0, 360-xAngle);
            }
            else {
                target = Quaternion.Euler(0,0, -xAngle);
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 0.8f);
    }

    void TurnHorizontal() {
        if(Mathf.Abs(xAngle)/xAngle != rxSign){
            xAngle=0;
            //target = Quaternion.Euler(0,0, 0);
            xTurning=false;
        }
        //NEW
        if(Mathf.Abs(yAngle)/yAngle != rySign){
            yAngle=0;
            //target = Quaternion.Euler(yAngle,0, xAngle);
            yTurning=false;
        }
        //
        else{
            if(rxSign >=0){
                target = Quaternion.Euler(0,0, 360-xAngle);
            }
            else {
                target = Quaternion.Euler(0,0, -xAngle);
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 0.8f);
    }

    void Rotate() {
        if(Mathf.Abs(xAngle)/xAngle != rxSign){
            xAngle=0;
            //target = Quaternion.Euler(0,0, 0);
            xTurning=false;
        }
        //NEW
        if(Mathf.Abs(yAngle)/yAngle != rySign){
            yAngle=0;
            //target = Quaternion.Euler(yAngle,0, xAngle);
            yTurning=false;
        }
        //
        switch ((rxSign >=0, rySign >=0,xTurning,yTurning)) {
            case (true, true,true,true):
                target = Quaternion.Euler(yAngle,0, 360-xAngle);
                break;
            case (false, true,true,true):
                target = Quaternion.Euler(yAngle,0, -xAngle);
                break;
            case (true, false,true,true):
                target = Quaternion.Euler(yAngle,0, 360-xAngle);
                break;
            case (false, false,true,true):
                target = Quaternion.Euler(yAngle,0, -xAngle);
                break;
            case (true, true,true,false):
                target = Quaternion.Euler(0,0, 360-xAngle);
                break;
            case (false, true,true,false):
                target = Quaternion.Euler(0,0, -xAngle);
                break;
            case (true, false,true,false):
                target = Quaternion.Euler(0,0, 360-xAngle);
                break;
            case (false, false,true,false):
                target = Quaternion.Euler(0,0, -xAngle);
                break;
            case (true, true,false,true):
                target = Quaternion.Euler(yAngle,0, 0);
                break;
            case (false, true,false,true):
                target = Quaternion.Euler(yAngle,0, 0);
                break;
            case (true, false,false,true):
                target = Quaternion.Euler(yAngle,0, 0);
                break;
            case (false, false,false,true):
                target = Quaternion.Euler(yAngle,0, 0);
                break;
            default:
                target = Quaternion.Euler(0,0, 0);
                break;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 0.8f);
    }

    void SetHorizontal() {
        SetAngle();
        xTurning = true;
        Rotate();
        MoveY();
        /*
        if(yTurning == true){
            TurnVertical();
            MoveY();
        }
        else{
            TurnHorizontal();
            MoveY();
        }
        */
    }
    void SetVertical() {
        SetAngle();
        yTurning=true;

        //NEW
        Rotate();
        MoveY();/*
    }
    void SetVertical() {
        SetAngle();
        yTurning=true;
        xTurning=true;
        //NEW
        Rotate();
        MoveY();/*
        if(xTurning == true) {
            TurnHorizontal();
            MoveY();
        }
        else {
            //target = Quaternion.Euler(0,0, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1.5f);
            TurnVertical();
            MoveY();
        }
        */
    }

    void SetDiagonal() {
        SetAngle();
        yTurning=true;
        xTurning=true;
        //NEW
        Rotate();
        MoveY();
    }
    
    // Update is called once per frame
    void Update()
    {
        switch ((Math.Abs(moveY.axis.x)<0.50, Math.Abs(moveY.axis.y)< 0.50)) {
            case (true, true):
                ResetNeutral();
                break;
            case (false, true):
                SetHorizontal();
                break;
            case (true, false):
                SetVertical();
                break;
            case (false, false):
                SetDiagonal();
                break;
        }
        
    }
}
