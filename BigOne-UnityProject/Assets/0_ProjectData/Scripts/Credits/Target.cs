using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject explosionParticles;

    private GameObject myExplosion;
    private ParticleSystem myParticles;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            myExplosion = Instantiate(explosionParticles, pos, rot);
            myParticles = myExplosion.GetComponent<ParticleSystem>();
            myParticles.Play();
            Destroy(collision.gameObject);
            Destroy(myExplosion, 1.0f);
        }
    }
}
