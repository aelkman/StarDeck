using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    // public SwordAttack swordAttack;

    Vector2 movementInput;
    // SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    // Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;
    bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {

        if (movementInput.x > 0 ) {
            flipped = false;
        }
        if (movementInput.x < 0) {
            flipped = true;
        }
        // bool flipped = movementInput.x < 0;

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f: 0f, 0f));
    }

    private void FixedUpdate() {
        if (canMove) {
            rb.velocity = movementInput * moveSpeed;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
