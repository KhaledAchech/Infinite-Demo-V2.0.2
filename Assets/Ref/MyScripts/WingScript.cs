using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingScript : MonoBehaviour
{
    [SerializeField]float rotationSpeed = 1f;
    Transform myT;

    //init myT
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
        RotateWing();
    }

    void RotateWing()
    {
        myT.Rotate(new Vector3(0f,0f,rotationSpeed) * Time.deltaTime);
    }
}
