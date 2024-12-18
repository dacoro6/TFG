
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;

    public float player_Speed;
    public float default_Speed;
    public float crouching_Speed;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    public Camera mainCamera;
    public CinemachineFreeLook fl;
    private Vector3 camForward;
    private Vector3 camRight;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideSpeed;
    public float slopeForceDown;


    // Variables animación
    public Animator playerAnimatorController;

    public AudioSource aud;
    public AudioClip cancion1,cancion2;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        crouching_Speed = player_Speed * 0.7f;
        default_Speed = player_Speed;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //Comenzar el listener
            fl.Priority = 1;
        }
        else 
        {
            fl.Priority = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        playerAnimatorController.SetFloat("PlayerWalkVelocity",playerInput.magnitude * player_Speed);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * player_Speed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();

        PlayerSkills();

        player.Move(movePlayer * Time.deltaTime);

        Debug.Log(player.velocity.magnitude);
    }



    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y=0;
        camRight.y=0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetTrigger("PlayerJump");
        }
        //Funcion de agacharse

        if (player.isGrounded && Input.GetKey("left ctrl"))
        {
            player_Speed = crouching_Speed;
            playerAnimatorController.SetBool("PlayerCrouch",true);
        }
        else {
            player_Speed = default_Speed;
            playerAnimatorController.SetBool("PlayerCrouch",false);
        }

        if (Input.GetKeyDown("1"))
        {
            PlayAnimation(1);
        }
        if (Input.GetKeyDown("2"))
        {
            PlayAnimation(2);
        }

        if (Input.GetKeyDown("3") ) {
            StopAnimation();
        }
    }



    public void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity= -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else 
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }

        playerAnimatorController.SetBool("isGrounded",player.isGrounded);
        SlideDown();
    }

    public void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) > player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f- hitNormal.y) * hitNormal.x) * slideSpeed;
            movePlayer.z += ((1f- hitNormal.y) * hitNormal.z) * slideSpeed;

            movePlayer.y += slopeForceDown;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void OnAnimatorMove(){
        
    }

    private void PlayAnimation(int cancion){
        StopAnimation();
        if (cancion == 1){
            aud.clip = cancion1;
            playerAnimatorController.SetBool("PlayerEmote1", true);
            PlayMusic();
        }
        if (cancion == 2){
            aud.clip = cancion2;
            playerAnimatorController.SetBool("PlayerEmote2", true);
            PlayMusic();
        }
    }
    private void StopAnimation(){
        playerAnimatorController.SetBool("PlayerEmote1", false);
        playerAnimatorController.SetBool("PlayerEmote2", false);
        StopMusic();
    }
    private void PlayMusic()
    {
       
        aud.Play();
    }

    private void StopMusic(){
        aud.Stop();
    }
}
