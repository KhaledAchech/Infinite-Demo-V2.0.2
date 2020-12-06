using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdPersonMovementScript : MonoBehaviour
{
    [Header("Public References")]
    public CharacterController controller;
    public Transform Cam;
    
    [Header("Parameters")]
    [SerializeField]float Forwardspeed = 100f;
    [SerializeField]float pitchSpeed = 80f;
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
        isTurning = Turn(spaceshipmain, h, leanLimit, lerpTime);
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
        float dir = Input.GetAxisRaw("BarrelRoll");
        BarrelRoll(dir);

        
    }

    //Moving Player ship forward
    void Thrust(float z, float speed)
    {
       myT.localPosition += new Vector3(0f, 0f, z) * speed * Time.deltaTime;
    }
    
    //turn 
    bool Turn(Transform target, float axis, float leanLimit, float lerpTime)
    {   
        temp = new Vector3(Mathf.LerpAngle(offset, axis * leanLimit, lerpTime), 0f, 0f);
        Vector3 targetEulerAngels = target.localEulerAngles ;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z , axis * leanLimit, lerpTime));
        return true;
    }
    
    // pitch up or down 
    void Pitch(float angle, float speed)
    {
        float p = speed * Time.deltaTime * angle; 
        myT.Rotate(-p,0,0);
    }

    //performs a barrel roll
    void BarrelRoll(float dir)
    {
        if (!DOTween.IsTweening(spaceshipmain))
        {
            spaceshipmain.DOLocalRotate(new Vector3(spaceshipmain.localEulerAngles.x, spaceshipmain.localEulerAngles.y, 360 * -dir), .4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
            //barrel.Play(); adding the effects
        }
    }

}
