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
    [SerializeField]float turnSpeed = 80f;
    [SerializeField]float leanLimit = 80f;
    [SerializeField]float lerpTime = .01f;
    [SerializeField]float offset = 5f;
    
    
    [Space]

    [Header("Particles")]
    public ParticleSystem trail;
    public ParticleSystem circle;
    public ParticleSystem barrel;
    public ParticleSystem stars;

    //float turnSmoothVelocity;
    Transform spaceshipmain;
    Transform myT;
    Vector3 temp;
    bool isTurning;
    float velocity = 0f;

    void Awake()
    {   
        myT = transform;
        spaceshipmain = transform.GetChild(0);
        isTurning = false;
        
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
        Pitch(up,pitchSpeed);

        //perform a barrel roll
        BarrelRoll();

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
    }
    
    // pitch up or down 
    void Pitch(float angle, float speed)
    {
        float p = speed * Time.deltaTime * angle; 
        myT.Rotate(-p,0,0);
    }

     //performs a barrel roll
    void BarrelRoll()
    {   
        if (Input.GetButtonDown("BarrelRoll"))
        {
            int dir = Input.GetButtonDown("BarrelRoll") ? -1 : 1;
            if (!DOTween.IsTweening(spaceshipmain))
            {
                spaceshipmain.DOLocalRotate(new Vector3(spaceshipmain.localEulerAngles.x, spaceshipmain.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
                //barrel.Play(); adding the effects
            }
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
           //speedlines.Stop();
           //StarsDust.Stop();
        }
    
            
    }

}
