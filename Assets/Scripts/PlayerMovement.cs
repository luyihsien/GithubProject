using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed=3f;
    [SerializeField] float jumpSpeed=10f;
    [SerializeField] float climbSpeed=5f;
    [SerializeField] Vector2 deathKick=new Vector2 (10f,10f);
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float gravityScaleAtStart;
    bool isAlive=true;
    
    void Start()

    {
        myRigidbody=GetComponent<Rigidbody2D>();
        myAnimator=GetComponent<Animator>();
        myCapsuleCollider=GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    void OnMove(InputValue value){
        if (!isAlive) { return; }
        moveInput=value.Get<Vector2>();

    }
    void Run(){
        Vector2 playerVelocity =new Vector2(moveInput.x*runSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity=playerVelocity;
        bool hasHorizontalSpeed=Mathf.Abs(myRigidbody.velocity.x)>0;
        myAnimator.SetBool("isRunning",hasHorizontalSpeed);
    }
    void FlipSprite(){
        bool hasHorizontalSpeed=Mathf.Abs(myRigidbody.velocity.x)>0;
        if(hasHorizontalSpeed==true){
            transform.localScale=new Vector3(Mathf.Sign(myRigidbody.velocity.x),1f,1f);
        }
    }
    void OnJump(InputValue value){
        if (!isAlive) { return; }
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))==false)return;
        if(value.isPressed){
            myRigidbody.velocity+=new Vector2(0f,jumpSpeed);
        }
    }
    void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        { 
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            //Debug.Log(123);
            return;
        }
        
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", true);
    }
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity=deathKick;
        }
    }
        

}










