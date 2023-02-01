using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    [RequireComponent (typeof(CapsuleCollider2D))]
    public class Crouch : Abilities
    {
        [SerializeField] [Range(0, 1)] protected float colliderMultiplier;
        [SerializeField] LayerMask collisionLayers;
        private CapsuleCollider2D playerCollider;
        private Vector2 originalColliderSize;
        private Vector2 crouchingColliderSize;
        private Vector2 originalOffset;
        private Vector2 crouchingOffset;

        protected override void Initialization()
        {
            base.Initialization();
            playerCollider = GetComponent<CapsuleCollider2D>();
            originalColliderSize = playerCollider.size;
            crouchingColliderSize = new Vector2((playerCollider.size.x * colliderMultiplier), (playerCollider.size.y * colliderMultiplier));
            originalOffset = playerCollider.offset;
            crouchingOffset = new Vector2(playerCollider.offset.x, (crouchingColliderSize.y * -0.5f));
        }

        private void FixedUpdate()
        {
            Crouching();

        }


        protected virtual void Crouching()
        {
            if (playerInput.CrouchHeld() && character.isGrounded)
            {
                character.isCrouching = true;
                anim.SetBool("IsCrouching", true);
                playerCollider.size = crouchingColliderSize;
                playerCollider.offset = crouchingOffset;
                
            }
            else 
            {   if (character.isCrouching)
                {
                    if (CollisionCheck(Vector2.up, playerCollider.size.y * 0.25f, collisionLayers))
                    {
                        return;
                    }
                    StartCoroutine(CrouchDisabled());
                }
            }
        }

        protected virtual IEnumerator CrouchDisabled()
        {
            playerCollider.offset = originalOffset;
            yield return new WaitForSeconds(0.01f);
            playerCollider.size = originalColliderSize;
            yield return new WaitForSeconds(0.15f);
            anim.SetBool("IsCrouching", false);
            character.isCrouching = false;
        }
    }
}
