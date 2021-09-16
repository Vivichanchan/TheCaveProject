using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Transform cam;
    public CharacterController controller;
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    public float playerSpeed = 1.5f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public Animator anim;
    private float smoothios = 0.1f;
    private float turnSmoothval;
    private float smoothenedSpeed;
    private float modi;
    private float motionRando = 0.2f;
    private void Start()
    {
        // controller = gameObject.AddComponent<CharacterController>();
        controller = gameObject.GetComponent<CharacterController>();

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
        if (Input.GetKey(KeyCode.LeftShift)){
            modi = Mathf.Lerp(modi,1.3f, Time.deltaTime * 10f);
        }else if(Input.GetKey(KeyCode.LeftControl)){
            modi = Mathf.Lerp(modi, -1.0f , Time.deltaTime * 10f);
        }else{
            modi = Mathf.Lerp(modi, 0 , Time.deltaTime * 15f);
        }
        

        if(move.magnitude > 0.1f){
            motionRando = Mathf.Lerp(motionRando,0f, Time.deltaTime * 0.1f);
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothval, smoothios);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDir = Quaternion.Euler(0, targetAngle , 0) * Vector3.forward;
            controller.Move(moveDir.normalized * Time.deltaTime * (playerSpeed + modi));
            groundedPlayer = controller.isGrounded;
        }else{
            motionRando = Mathf.Lerp(motionRando,2.5f, Time.deltaTime * 0.005f);
            if(motionRando > 2){
                motionRando = Mathf.Lerp(motionRando,0f, Time.deltaTime * 5f);
            }
        }
        smoothenedSpeed = Mathf.Lerp(smoothenedSpeed, controller.velocity.magnitude, Time.deltaTime * 20f);
        anim.SetFloat("Speed", smoothenedSpeed);
        anim.SetFloat("modi", modi);
        anim.SetFloat("movement", motionRando);
        
        
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        groundedPlayer = controller.isGrounded;
        
        
    }
}
