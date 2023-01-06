using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;

public class turnPlayer : MonoBehaviour
{
    public SteamVR_Action_Vector2 moveX;
    public GameObject left_hand;
    public GameObject right_hand;
    private float xSpeed;
    private float xAccel;
    public float xSensitivity;
    private float xSign;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if(moveX.axis.x==0 && xSpeed!=0){
            xAccel = 0;
            xSpeed -= xSensitivity*Mathf.Abs(xSign)/(xSign*2);
            if(xSign!=Mathf.Abs(xSpeed)/xSpeed){
                xSpeed=0;
            }
        }
        else{
            if(xAccel==0){
                xSign=Mathf.Abs(moveX.axis.x)/moveX.axis.x;
            }
            xAccel = xSensitivity *(moveX.axis.x);
            xSpeed += xAccel;
            if(Mathf.Abs(xSpeed)>6){
                xSpeed=Mathf.Abs(xSpeed)/xSpeed*6;
            }
        }
        transform.position += xSpeed * Time.deltaTime * Vector3.ProjectOnPlane(new Vector3(1, 0, 0), Vector3.up); 



        transform.position += 9 * Time.deltaTime * Vector3.ProjectOnPlane(new Vector3(0, 0, 1), Vector3.up);
    }
}