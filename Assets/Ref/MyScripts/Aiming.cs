using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    
    
    private Vector3 target;
    Transform myT;

    public Transform ship;
    public float boresightDistance = 1000f;

    void Awake()
    {   
        myT = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position += Input.mousePosition;
        //crossHairs.transform.position =target ;

        //Vector3 difference = target - playerShip.transform.position;
        //float rotationZ = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        //playerShip.transform.rotation = Quaternion.Euler(0f,difference.x, 0f);
      
    }
}
