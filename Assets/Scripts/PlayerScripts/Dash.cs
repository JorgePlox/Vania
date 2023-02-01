using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metroidvania
{
    public class Dash : Abilities
    {
        [SerializeField] protected float dashForce;
        [SerializeField] protected float dashCooldownTime;
        [SerializeField] protected float dashAmountTime;
        [SerializeField] protected LayerMask dashingLayers;

        private bool canDash;
        private float dashCountDown;



        // Update is called once per frame
        protected virtual void Update()
        {
            Dashing();
        }

        protected virtual void FixedUpdate()
        {
            DashMode();
            ResetDashCounter();
            
        }

        protected virtual void Dashing()
        {
            if (playerInput.DashPressed() && canDash)
            {
                dashCountDown = dashCooldownTime;
                character.isDashing = true;
                anim.SetBool("IsDashing", true);
                StartCoroutine(FinishedDashing());
            }
        }

        protected virtual void DashMode()
        {
            if (character.isDashing)
            {
                FallSpeed(0);
                movement.enabled = false;
                if (!character.isFacingLeft)
                {
                    DashCollision(Vector2.right, .5f, dashingLayers);
                    rb.AddForce(Vector2.right * dashForce);
                }
                else
                {
                    DashCollision(Vector2.left, .5f, dashingLayers);
                    rb.AddForce(Vector2.left * dashForce);
                }
            }
        }

        protected virtual void ResetDashCounter()
        {
            if (dashCountDown > 0)
            {
                canDash = false;
                dashCountDown -= Time.deltaTime;
            }
            else 
            {
                canDash = true;
            }
        }

        protected virtual IEnumerator FinishedDashing()
        {
            yield return new WaitForSeconds(dashAmountTime);
            anim.SetBool("IsDashing", false); //dfxgvdfbfgbfgbgvfb
            character.isDashing = false;
            FallSpeed(1);
            movement.enabled = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        protected virtual IEnumerator TurnColliderBackOn(GameObject obj)
        {
            yield return new WaitForSeconds(dashAmountTime);
            obj.GetComponent<Collider2D>().enabled = true;
        }

        protected virtual void DashCollision(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    hits[i].collider.enabled = false;
                    StartCoroutine(TurnColliderBackOn(hits[i].collider.gameObject));
                }
            }

        }


    }   
}