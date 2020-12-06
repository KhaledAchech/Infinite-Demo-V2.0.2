using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamraControl : MonoBehaviour
{   
    
    [SerializeField]float bias = 0.96f;
    [SerializeField]float CamForwardSpeed = 10.0f;
    [SerializeField]float CamUpSpeed = 5.0f;
    [SerializeField]float OffsetValue = 1.0f;
    [SerializeField]float FollowSpeed = 30.0f;
    [SerializeField]Transform target;

    //init myT
    Transform myT;
    void Awake()
    {
        myT = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowCam();
    }

    void FollowCam()
    {
        Vector3 MoveCamTo = target.position - target.forward * CamForwardSpeed + Vector3.up * CamUpSpeed;
        myT.position = myT.position * bias + MoveCamTo * (OffsetValue - bias);
        myT.LookAt(target.position + target.forward * FollowSpeed);
    }
}
