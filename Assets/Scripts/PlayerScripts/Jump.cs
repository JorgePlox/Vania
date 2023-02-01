using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class Jump : Abilities
    {
        [SerializeField] protected bool limitAirJumps;
        [SerializeField] protected int maxJumps;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected float holdForce;
        [SerializeField] protected float buttonHoldTime;
        [SerializeField] protected float distanceToCollider;
        [SerializeField] protected float horizontalWallJumpForce;
        [SerializeField] protected float verticalWallJumpForce;
        [SerializeField] protected float wallJumpRestrictTime;
        [SerializeField] protected float maxJumpSpeed;
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float acceptedFallSpeed;
        [SerializeField] protected float glideTime;
        [SerializeField] [Range(-2,2)] protected float gravityMultiplier;
        public LayerMask collisionLayer;
        
        protected bool isJumping = false;
        private bool isWallJumping;
        private float jumpCountDown;
        private float fallCountDown;
        private int numberOfJumpsLeft;


        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
        }

        // Update is called once per frame
        protected void Update()
        {
            JumpCheck();
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            Gliding();
            GroundCheck();
            WallSliding();
            WallJump();
        }


        protected virtual bool JumpCheck()
        {
            if (playerInput.JumpPressed())
            {
                if (!character.isGrounded && numberOfJumpsLeft == maxJumps)
                {
                    isJumping = false;
                    return false;
                }
                if (limitAirJumps && Falling(acceptedFallSpeed))
                {

                    isJumping = false;
                    return false;
                }
                if (character.isWallSliding)
                {
                    isWallJumping = true;
                    return false;
                }

                numberOfJumpsLeft--;
                if (numberOfJumpsLeft >= 0)
                {
                    if (!anim.GetBool("IsJumping"))
                        anim.SetBool("IsJumping", true);
                    else if (anim.GetBool("IsJumping"))
                        anim.SetBool("DoubleJump", true);

                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                }

                return true;
            }

            else
            {
                return false;
            }
        }


        protected virtual void IsJumping()
        {
            if (isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
                AdditionalAir();
            }

            if (rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void Gliding()
        {
            if (Falling(0) && playerInput.JumpHeld() && !character.isGrounded)
            {
                fallCountDown -= Time.deltaTime;
                if (fallCountDown > 0 && rb.velocity.y > acceptedFallSpeed)
                {
                    anim.SetBool("IsGliding", true);
                    FallSpeed(gravityMultiplier);
                    return;
                }
            }
            anim.SetBool("IsGliding", false);
        }

        protected virtual void AdditionalAir()
        {
            if (playerInput.JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }

                else
                    rb.AddForce(Vector2.up * holdForce);
            }
            else 
                isJumping = false;
        }

        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("DoubleJump", false);
                character.isGrounded = true;
                numberOfJumpsLeft = maxJumps;
                fallCountDown = glideTime;
            }

            else
            {
                character.isGrounded = false;

                if (Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
        }

        protected virtual bool WallCheck()
        {
            if ((!character.isFacingLeft && CollisionCheck(Vector2.right, distanceToCollider, collisionLayer)) || (character.isFacingLeft && CollisionCheck(Vector2.left, distanceToCollider, collisionLayer)) && movement.MovementPressed() && !character.isGrounded)
            {
                return true;
            }
            return false;
        }

        protected virtual bool WallSliding()
        {
            if (WallCheck())
            {
                FallSpeed(gravityMultiplier);
                character.isWallSliding = true;
                anim.SetBool("IsWallSliding", true);
                return true;
            }
            else
            {
                character.isWallSliding = false;
                anim.SetBool("IsWallSliding", false);
                return false;
            }
        }

        protected virtual void WallJump()
        {
            if (isWallJumping)
            {
                rb.AddForce(Vector2.up * verticalWallJumpForce);

                if (!character.isFacingLeft)
                {
                    rb.AddForce(Vector2.left * horizontalWallJumpForce);
                }

                else if (character.isFacingLeft)
                {
                    rb.AddForce(Vector2.right * horizontalWallJumpForce);
                }

                StartCoroutine(RestrictHorizontalMovement());
            }
        }

        protected virtual IEnumerator RestrictHorizontalMovement()
        {
            movement.enabled = false;
            yield return new WaitForSeconds(wallJumpRestrictTime);
            movement.enabled = true;
            isWallJumping = false;
        }
    }
}
