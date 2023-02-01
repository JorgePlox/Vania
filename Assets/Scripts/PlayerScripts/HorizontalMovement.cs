using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class HorizontalMovement : Abilities
    {
        [SerializeField] protected float timeTillMaxSpeed;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float sprintMultiplier;
        [SerializeField] protected float crouchMultiplier;
        private float acceleration;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;

        protected override void Initialization()
        {
            base.Initialization();
        }


        protected virtual void Update()
        {
            MovementPressed();
        }

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        public virtual bool MovementPressed()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                return true;
            }
            else 
            {
                return false;
            }
        }

        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                anim.SetBool("IsWalking", true);
                acceleration = maxSpeed / timeTillMaxSpeed;
                runTime += Time.deltaTime;
                currentSpeed = horizontalInput * acceleration * runTime;
                CheckDirection();
            }
            else 
            {
                anim.SetBool("IsWalking", false);
                acceleration = 0;
                runTime = 0;
                currentSpeed = 0;
            }
            SpeedMultiplier();
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        }

        protected virtual void CheckDirection()
        {
            if (currentSpeed > 0)
            {
                if (character.isFacingLeft)
                {
                    character.isFacingLeft = false;
                    Flip();
                }
                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
            }

            else if (currentSpeed < 0)
            {
                if (!character.isFacingLeft)
                {
                    character.isFacingLeft = true;
                    Flip();
                }

                if (currentSpeed < -maxSpeed)
                {
                    currentSpeed = -maxSpeed;
                }
            }
        }

        protected virtual void SpeedMultiplier()
        {
            if (playerInput.SprintHeld())
            {
                currentSpeed *= sprintMultiplier;
            }
            else if (character.isCrouching)
            {
                currentSpeed *= crouchMultiplier;
            }

            if ((!character.isFacingLeft && CollisionCheck(Vector2.right, 0.05f, jump.collisionLayer)) || (character.isFacingLeft && CollisionCheck(Vector2.left, 0.05f, jump.collisionLayer)))
            {
                currentSpeed = 0.01f;
            }

        }
    
    }

}