using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    //Script being used to create projectile for arrow
    public class FireArrowProjectile : MonoBehaviour
    {
        [HideInInspector] public ThinkingPlaceable target;
        [HideInInspector] public ThinkingPlaceable spawningParent;
        float elapse_time;

        public float firingAngle = 45.0f;
        public float gravity = 9.8f;

        float target_Distance;
        float projectile_Velocity;

        float Vx, Vy;
        float flightDuration;
        float newHP;

        void Start()
        {
            _ = StartCoroutine(SimulateProjectile(() => { Destroy(gameObject); }));
        }


        IEnumerator SimulateProjectile(System.Action _actionCall)
        {
            // Short delay added before Projectile is thrown
            yield return new WaitForSeconds(0.25f);

            // Calculate distance to target
            target_Distance = Vector3.Distance(transform.position, target.transform.position);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X  Y componenent of the velocity
            Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                elapse_time += Time.deltaTime;
                yield return null;
            }

            // Manage target's Damage and reduce health of the target
            if ((spawningParent.state != ThinkingPlaceable.States.Dead) && (target.state != ThinkingPlaceable.States.Dead))
            {
                newHP = target.SufferDamage(target.damage);
                if (target.state != ThinkingPlaceable.States.Dead)
                    target.healthBar.SetHealth(newHP);
            }

            _actionCall?.Invoke();
        }
    }
}