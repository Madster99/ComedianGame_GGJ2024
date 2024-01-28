using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Vector3 groundOffsetPoint = new Vector3(0,-2,0);
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private float gravityMultiplier = 3;
    private bool isGrounded;
    private Vector3 jumpVelocity;
    private const float GRAVITY = -9.81f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float acceleration = 5;
    private Vector3 inputVelocity;
    private Vector3 playerVelocity;
    private float currentSpeed;
    private bool forceMoving;
    private IEnumerator moveCoroutine;
    public delegate void OnMoveComplete();

    #region Components
    [Foldout("Component")]
    [SerializeField] private CharacterController controller;
    #endregion
    
    [Button]
    private void GetMissingComponents()
    {
        if(controller == null)
            controller = GetComponent<CharacterController>();
    }
    
        
    public void ForceMovePlayerToPosition(Vector3 position, OnMoveComplete onMoveComplete = null)
    {
        if(forceMoving)
            StopCoroutine(moveCoroutine);

        moveCoroutine = MovePlayer(position, onMoveComplete);
        StartCoroutine(moveCoroutine);
    }
    
    private void DisableForceMove()
    {
        if(forceMoving)
            StopCoroutine(moveCoroutine);

        forceMoving = false;
    }
    
    IEnumerator MovePlayer(Vector3 position, OnMoveComplete onMoveComplete)
    {
        forceMoving = true;

        while(Vector3.Distance(position, transform.position) > 0.1f)
        {
            playerVelocity = (position - transform.position).normalized * moveSpeed;
            DoGravityPhysics();
            
            controller.Move(playerVelocity + jumpVelocity);

            yield return null;
        }

        onMoveComplete?.Invoke();
        playerVelocity = Vector3.zero;
        forceMoving = false;
    }

    public void Move(Vector2 moveAxis)
    {
        if (forceMoving)
            return;
        
        inputVelocity = Vector3.right * (moveAxis.x * moveSpeed);
        inputVelocity += Vector3.forward * (moveAxis.y * moveSpeed);
        
        playerVelocity = Vector3.Lerp(playerVelocity, inputVelocity, Time.deltaTime * acceleration);
        DoGravityPhysics();

        controller.Move(playerVelocity + jumpVelocity);
    }

    private void DoGravityPhysics()
    {
        isGrounded = Physics.CheckSphere(transform.position + groundOffsetPoint, groundRadius, layerMask);

        if(isGrounded)
            ResetGravity();
        else
            jumpVelocity += Vector3.up * (GetGravity() * Time.deltaTime);
    }

    private float GetGravity()
    {
        return GRAVITY * gravityMultiplier;
    }
    
    private void ResetGravity()
    {
        jumpVelocity = Vector3.zero;
    }
    
    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundOffsetPoint, groundRadius);
    }
}
