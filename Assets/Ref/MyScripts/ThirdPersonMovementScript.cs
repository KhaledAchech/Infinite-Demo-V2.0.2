using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdPersonMovementScript : MonoBehaviour
{
    [Header("Public References")]
    public CharacterController controller;
    //public Transform Cam;
    
    [Header("Movement Parameters")]
    [SerializeField]float Forwardspeed = 100f;
    [SerializeField]float AccelarationSpeed = 300;
    [SerializeField]float pitchSpeed = 80f;
    [SerializeField]float pitchLimit = 80f;
    [SerializeField]float CircleRadius = 3f;
    [SerializeField]float turnSpeed = 80f;
    [SerializeField]float leanLimit = 80f;
    [SerializeField]float lerpTime = .01f;
    [SerializeField]float offset = 5f;
    
    // Keep a static reference for whether or not this is the player ship. It can be used
    // by various gameplay mechanics. Returns the player ship if possible, otherwise null.
    public static ThirdPersonMovementScript PlayerShip { get; private set; }
    
    [Space]

    [Header("Particles")]
    public ParticleSystem trail;
    public ParticleSystem circle;
    public ParticleSystem barrel;
    public ParticleSystem stars;

    //float turnSmoothVelocity;

    //init Variables 
    Transform spaceshipmain;// spaceship main in case the game has so many space ships so the model of the player ship is replaced accordingly.
    Transform myT; // init the transform of the player .
    Vector3 temp; // temp array might not be used. (purpose of it is to help add an offset while turning).
    bool isPitching;// checking if the spaceShip is going up or down
    bool PressedOnce;// check if the barrelRoll button is pressed once or not.
    float velocity = 0f;// init velocity for the smoothdump of turning might be not used.
    float timeOfFirstPress = 0f;// Used for saving the first time the barrel roll buttons are pressed ^^ .
    bool reset;// reset the barrelRoll buttons pressed or not state.
    bool isBoosting; // check if the player is boosting (purpose is that i don't want the player to be able to boost while pitching his ship up or down).

    void Awake()
    {   
        myT = transform;
        spaceshipmain = transform.GetChild(0);
        isPitching= false;
        PressedOnce= true;
        reset = false;
        isBoosting = false;
    }

    // Update is called once per frame
    void Update()
    {
    /********************************Basic Movement Script for 3D Person for testing and quick implementations*******************
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float up = Input.GetAxisRaw("Pitch");
        
        Vector3 direction = new Vector3(horizontal, up, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {   
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + -Cam.eulerAngles.y;
            float yangle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, yangle, 0f);
            
            
            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,targetAngle) * Vector3.forward;
            controller.Move(moveDir.normalized  * speed * Time.deltaTime);
        }
    *******************************************************************************************************************************/
        //Moving Forward
        float v = Input.GetAxisRaw("Vertical");
        if ( v > 0) {Thrust(v,Forwardspeed);}

        //Turning
        float h = Input.GetAxisRaw("Horizontal");
        Turn(spaceshipmain, h, leanLimit, lerpTime);
        
        /*if (h > 0 && !isTurning ) //************************************** to check try to figure out how to make a slight offset while turning
         {
             myT.localPosition += temp;
             isTurning = true;
         }
        if (h < 0 && !isTurning )
         {
             myT.localPosition -= temp;
             isTurning = true;
         }
        */

        //Up Or Down
        float up = Input.GetAxisRaw("Pitch");
        if(up != 0) isPitching = true; else isPitching = false;
        Pitch(spaceshipmain,up,pitchSpeed, pitchLimit, lerpTime);

        //perform a barrel roll
        BarrelRoll();

        //Perform a full circle 
        //CircularMotion();

        //Speed Boost
        Faster();


    }
   

    //Moving Player ship forward
    void Thrust(float z, float speed)
    {
       myT.localPosition += new Vector3(0f, 0f, z) * speed * Time.deltaTime;
    }
    
    //turn 
    void Turn(Transform target, float axis, float leanLimit, float lerpTime)
    {   
        float tParam = 0;
        Vector3 targetEulerAngels = target.localEulerAngles;
        tParam += Time.deltaTime * turnSpeed * lerpTime;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, axis * leanLimit, lerpTime));
        //target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.SmoothDampAngle(targetEulerAngels.z, axis * leanLimit, ref velocity, lerpTime));
        myT.localPosition += new Vector3(axis, 0f, 0f) * turnSpeed * Time.deltaTime;
        float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        myT.Rotate(0,-Mathf.LerpAngle(targetEulerAngels.y, yaw * leanLimit, lerpTime),0);  
    }
    
    // pitch up or down 
    void Pitch(Transform target,float angle, float speed, float pitchLimit,float lerpTime)
    {   
        if (!isBoosting)
        {
            Vector3 targetEulerAngels = target.localEulerAngles;
            float p = speed * Time.deltaTime; 
            //myT.Rotate(-p,0f,0f);
            //myT.position += new Vector3(0f, angle, 0f) ;//* speed * Time.deltaTime;
            target.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngels.x, -angle * pitchLimit, lerpTime),targetEulerAngels.y ,targetEulerAngels.z );
            myT.localPosition += new Vector3(0f, angle, 0f) *p;
        }
        
    }

     //performs a barrel roll
    void BarrelRoll()
    {   
        // checkign if the button is pressed once and the plane is not going up or down
        if(Input.GetButtonDown("BarrelRoll") && !isPitching && PressedOnce)
         {
             if(Time.time - timeOfFirstPress > 0.3f) {
                timeOfFirstPress = Time.time;
                int dir = Input.GetButtonDown("BarrelRoll") ? -1 : 1;
                    if (!DOTween.IsTweening(spaceshipmain))
                    {
                         myT.DOLocalRotate(new Vector3(spaceshipmain.localEulerAngles.x, spaceshipmain.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
                        //barrel.Play(); adding the effects
                    }
             } else 
             {
                Debug.Log("Too late");
             }
 
             reset = true;
         }
 
         if(Input.GetButtonDown("BarrelRoll") && !PressedOnce) {
             PressedOnce = true;
             timeOfFirstPress = Time.time;
         }
 
         if(reset) {
             PressedOnce = false;
             reset = false;
         }
        /*if (Input.GetButtonDown("BarrelRoll") && !isPitching)
        {
            int dir = Input.GetButtonDown("BarrelRoll") ? -1 : 1;
            if (!DOTween.IsTweening(spaceshipmain))
            {
                myT.DOLocalRotate(new Vector3(spaceshipmain.localEulerAngles.x, spaceshipmain.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
                //barrel.Play(); adding the effects
            }
        }*/
    }

    //causing too many bugs might have to just remove this feature of the game.
   /* void CircularMotion()
    {
        if (Input.GetButtonDown("CircularMotion"))
        {
            int dir = Input.GetButtonDown("CircularMotion") ? -1 : 1;
            if (!DOTween.IsTweening(spaceshipmain))
            {
                spaceshipmain.DOLocalRotate(new Vector3(360 * dir, spaceshipmain.localEulerAngles.y, spaceshipmain.localEulerAngles.z), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
                //barrel.Play(); adding the effects
            }
        }
    }
    */
    //More Speed
    void Faster()
    {
        if (Input.GetAxis("Faster") > 0 && !isPitching)
        {
           myT.position += myT.forward * AccelarationSpeed * Time.deltaTime * Input.GetAxis("Faster");
           isBoosting = true;
           //ToggleSpeedLinesParticleSystem();
           //speedlines.Emit(5);
           //StarsDust.Emit(5);
        }
        else
        {
            isBoosting = false;
           //speedlines.Stop();
           //StarsDust.Stop();
        }
    
            
    }

    

}
