using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashSystem : MonoBehaviour 
{

    public float dashDistance;
    public float dashDuration;
    private Rigidbody2D rb;
    private Vector2 dashDirection;
    public bool isDash;
    private float dashInterval;
    public bool canDash = true;

    public float tempDashForce;


    void Start() {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        isDash = false;
    }
    public void Dash(Vector2 movement) {
        if (canDash && !isDash) {
            isDash = true;
            dashDirection = movement.normalized;
            dashInterval = dashDuration;
            Debug.Log("Dashing direction: " + dashDirection);
            this.gameObject.GetComponent<PlayerStateManager>().canControlMovement = false;
        }
    }

    void Update() {
        if (isDash) {
            Vector2 dashVelocity = dashDirection * (tempDashForce);
            rb.AddForce(dashVelocity, 0);
        }
        
    }

    void FixedUpdate() {
        if (isDash && dashInterval > 0) {
            dashInterval--;
        }
        if (isDash && dashInterval <= 0) {
            isDash = false;
            this.gameObject.GetComponent<PlayerStateManager>().canControlMovement = true;
            rb.velocity = Vector2.zero;
            Debug.Log("Dash End");
        }
        
    }
}