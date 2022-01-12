using UnityEngine;
using Mirror;

//Player is allowed to move in all directions and jump
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float playerMaxSpeed = 30f;
    [SerializeField] private float jumpSpeed = 0.5f;
    [SerializeField] private float characterHeight = 2f;
    [SerializeField] private Rigidbody controller = null;
    [SerializeField] private LayerMask mask;
    
    private Vector2 previousInput;      //Previous movement needs to be called as movements are only called once rather than once every frame.
                                        // So it needs to know when to change its movement by knowing when to reset.
    private Vector3 jumpDirection;
    private Vector3 externalDirection;

     private bool playerJump = false;
     private bool isGrounded = false;

     public void SetisGrounded (bool _isGrounded)
     {
         isGrounded = _isGrounded;
     }
    
    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }
    
     ///<summary> Everytime when a movement key is pressed, it either continues its current path or resets it.
     ///<para> Requires: SetMovement method
     ///<para> Requires: ResetMovement method
     ///</summary>
    public override void OnStartAuthority()
    {
        enabled = true;
        controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => ResetMovement();
        controls.Player.Jump.started += ContextMenu => Jump();
        controls.Player.Jump.canceled += ContextMenu => CancelJump();
    }

     ///<summary> Only allows the player to jump if they are one the ground, and applies gravity once its above the ground.
     ///<para> Requires: jumpDirection variable
     ///<para> Requires: gravity variable
     ///<para> Requires: controller object
     ///</summary>
    
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    
     //Reads changes to the movement
    [ClientCallback]
    private void FixedUpdate()
    {
        UpdateJump();
        Move();
    } 
    
     //Sets movement to what its previous direction is
    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;
    
    //Resets movement if there is a change in direction or sudden stop
    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;
    
    [Client]
    private void Jump() => playerJump = true;

    [Client]
    private void CancelJump() => playerJump = false;
    
    [Client]
    private void UpdateJump()
    {
        if(playerJump == true && isGrounded)
        { 
            jumpDirection.y = jumpSpeed;
        }
        else
        {
            jumpDirection.y = 0f;
        }
    }
     ///<summary> Reads what the player is currently pressing and moves the character base on previous input and time.
     ///</summary>
    [Client]
    private void Move()
    {
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        Vector3 flatMovement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        Vector3 moveDirection = new Vector3(flatMovement.x, jumpDirection.y,flatMovement.z);

        if(controller.velocity.magnitude < playerMaxSpeed)
        controller.AddForce((moveDirection * movementSpeed * Time.deltaTime),ForceMode.Impulse);
        Debug.Log("JumpHeight: " + jumpDirection.y);
    }

     ///<summary> Checks what the player is in contact with, if it's in contact with a moving platform, then the velocity of the platform will be added to the player's movement.
     ///<para> Requires: externalDirection variable
     ///<para> Requires: MoveWithMovingPlatform script
     ///</summary>

    [Client]
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Floor" || other.transform.tag == "Moving Floor")
        {
            isGrounded = true;
        } 
        Debug.Log("IsGrounded: " + isGrounded);
    }
    
    [Client]
    void OnCollisionExit(Collision other)
    {
        if((other.transform.tag == "Floor" || other.transform.tag == "Moving Floor") && playerJump == true)
        {
            isGrounded = false;
        }
        Debug.Log("IsGrounded: " + isGrounded);
    }
}
