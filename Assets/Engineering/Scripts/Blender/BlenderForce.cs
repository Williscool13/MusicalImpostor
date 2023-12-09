using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderForce : MonoBehaviour
{
    [SerializeField] float blenderForceMultiplier = 2f;
    [SerializeField] AudioSource pulseSource;
    [SerializeField] ScriptableGameEvent_Null blenderHit;
    private void OnCollisionEnter(Collision collision) {
        ShootOut(collision);
    }

    private void OnCollisionStay(Collision collision) {
        ShootOut(collision);
    }

    void ShootOut(Collision collision) {
        // if the object's tag is "Fruit" then add a force to it
        if (collision.gameObject.CompareTag("Fruit")) {
            // calculate an upward force with a slight angle
            Vector2 randomHorizontal = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Vector3 blenderForce = new(randomHorizontal.x, 1, randomHorizontal.y);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            blenderHit.Raise(null);
            rb.AddForce(1000 * blenderForceMultiplier * blenderForce);
            rb.AddTorque(blenderForce);
            pulseSource.pitch = Random.Range(0.8f, 1.2f);
            pulseSource.PlayOneShot(pulseSource.clip);
        }
    }

}
