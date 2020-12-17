using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
  [Header("Public References")]
  public Transform target; //target is going to be our space ship

  [Header("Camera Settings")]
  [SerializeField]float smoothSpeed = 10f; //the higher the value gets the faster the camera is going to lock on our target.
  [SerializeField]Vector3 offset; //this vector will help us tweek how far we want the camera to be from our target.

    private Vector3 velocity = Vector3.zero;

    void LateUpdate() 
    {
      Vector3 desiredPosition = target.position + offset;
      Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity ,smoothSpeed*Time.deltaTime);  
      transform.position = smoothedPosition;
      
      transform.LookAt(target);
    }

}
