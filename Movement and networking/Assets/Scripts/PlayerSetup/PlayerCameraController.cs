using Cinemachine;
using Mirror;
using UnityEngine;

// Camera follows the player in a third person view
public class PlayerCameraController : NetworkBehaviour
{
    #region Variables
    [Header("Camera")]
    [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
    
    private Controls controls;
    
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }
   
    private CinemachineTransposer transposer;
    #endregion
    
     ///<summary> If it's the client's player, then it will enable the camera on the player, otherwise all the camera's on the client are disabled.
     ///<para> Requires: virtualCamera object
     ///</summary>
    public override void OnStartAuthority()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        virtualCamera.gameObject.SetActive(true);
        
        enabled = true;
        
        controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }
    
    [ClientCallback]
    private void OnEnable() => Controls.Enable();
    
    [ClientCallback]
    private void OnDisable() => Controls.Disable();
    
     ///<summary> Method allows the camera to rotate and moved by the player
     ///<para> Requires: MaxFollowOffset Vector 2
     ///</summary>
    private void Look(Vector2 lookAxis)
    {
        float deltaTime = Time.deltaTime;
        
        transposer.m_FollowOffset.y = Mathf.Clamp( transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * deltaTime),
         maxFollowOffset.x,
         maxFollowOffset.y);
        
        playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
    }
}

