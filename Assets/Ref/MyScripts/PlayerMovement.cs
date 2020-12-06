using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float movementSpeed = 100f;
    [SerializeField]float turnSpeed = 60f;
    [SerializeField]float pitchSpeed = 60f;
    [SerializeField]float barrelrollSpeed = 500f;
    [SerializeField]float AccelarationSpeed = 300;


    Transform myT;
    private bool isdoingbarrelroll = false;

    //private Transform spaceshipmain;

    public ParticleSystem speedlines;
    public ParticleSystem StarsDust;

    GameObject spaceshipmain ; //main part of the space ship

    // Start is called before the first frame update
    void Start()
    {
    
    }

    //init myT
    void Awake()
    {
        myT = transform;
        spaceshipmain = GameObject.Find("MentisMainBody");
        //spaceshipmain = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {   
        Pitch();
        BarrelRoll();
        Turn();
        Thrust();
        Faster();
    }

    //Player space ship moves up or down
    void Pitch()
    {
        //r to move upwards and f to move downwars
        float p = pitchSpeed * Time.deltaTime * Input.GetAxis("Pitch"); 
        myT.Rotate(-p,0,0);
    }

    //Player ship performs a barrel roll
    void BarrelRoll()
    {   
        //a to do a left barrelroll and e to do a right barrelroll
        if (Input.GetAxis("BarrelRoll") > 0 || Input.GetAxis("BarrelRoll") < 0)
        {
            
            float barrelroll = barrelrollSpeed * Time.deltaTime * Input.GetAxis("BarrelRoll");
            //float dir = Input.GetAxis("BarrelRoll") ;
            //spaceshipmain.transform.Rotate(360 * -dir, 0,0 );
            spaceshipmain.transform.Rotate(barrelroll,0,0);
            
        }
        
    }

    //Turning Player ship y axis
    void Turn()
    {
        float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        myT.Rotate(0,yaw,0);  
    }

    //Moving Player ship forward
    void Thrust()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            myT.position += myT.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            //speedlines.Emit(1);
            //StarsDust.Emit(1);
            
        }
        else
        {
            speedlines.Stop();
            StarsDust.Stop();
        }
            
    }

    //More Speed
    void Faster()
    {
        if (Input.GetAxis("Faster") > 0)
        {
           myT.position += myT.forward * AccelarationSpeed * Time.deltaTime * Input.GetAxis("Faster");
           //ToggleSpeedLinesParticleSystem();
           //speedlines.Emit(5);
           //StarsDust.Emit(5);
        }
        else
        {
           speedlines.Stop();
           StarsDust.Stop();
        }
    
            
    }

    //turn speedlines on and off
    void ToggleSpeedLinesParticleSystem()
    {
        /*if (speedlines.isPlaying)
        {
            speedlines.Stop();
            StarsDust.Stop();
        }
        else
        {
            speedlines.Play();
            StarsDust.Play();
        }*/
    }

    public bool getisdoingbarrelroll()
    {
        return isdoingbarrelroll;
    }
}
