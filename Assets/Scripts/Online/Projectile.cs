using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables
    public float speed;

    public GameObject muzzlePrefabs;
    public GameObject hitPrefabs;

    public AudioClip showSFX;
    public AudioClip hitSFX;

    private bool collided;
    private Rigidbody rigidbody;

    [HideInInspector]
    public AttackBehaviour attackBehaviour;

    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public GameObject target;

    #endregion Variables

    #region Unity Methods
    protected virtual void Start()
    {
        if (target != null)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }

        if (owner != null)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();

            foreach (Collider collider in ownerColliders)
            {
                Physics.IgnoreCollision(projectileCollider, collider);
            }
        }

        rigidbody = GetComponent<Rigidbody>();

        if (muzzlePrefabs != null)
        {
            GameObject muzzleVFX = Instantiate(muzzlePrefabs, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(muzzleVFX, particleSystem.main.duration);
            }
            else
            {
                for (int i = 0; i < muzzleVFX.transform.childCount; i++)
                {
                    ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                    if (childParticleSystem)
                    {
                        Destroy(muzzleVFX, particleSystem.main.duration);
                    }
                }
            }
        }

        if (showSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(showSFX);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (speed != 0 && rigidbody != null)
        {
            rigidbody.position += (transform.forward) * (speed * Time.deltaTime);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collided)
        {
            return;
        }

        collided = true;

        Collider projectileColiider = GetComponent<Collider>();
        projectileColiider.enabled = false;

        if (hitSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(hitSFX);
        }

        speed = 0f;
        rigidbody.isKinematic = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPosition = contact.point;

        if (hitPrefabs != null)
        {
            GameObject hitVFX = Instantiate(hitPrefabs, contactPosition, contactRotation);
            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(hitVFX, particleSystem.main.duration);
            }
            else
            {
                for (int i = 0; i < hitVFX.transform.childCount; i++)
                {
                    ParticleSystem childParticleSystem = hitVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
                    if (childParticleSystem != null)
                    {
                        Destroy(childParticleSystem.gameObject, childParticleSystem.main.duration);
                    }
                }
            }
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackBehaviour?.damage ?? 0, null);
        }

        StartCoroutine(DestroyParticle(5.0f));
    }

    #endregion Unity Methods

    #region Helper Methods
    public IEnumerator DestroyParticle(float waitTime)
    {
        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> childs = new List<Transform>();

            foreach (Transform t in transform)
            {
                childs.Add(t);
            }

            while (transform.localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);

                transform.localScale -= Vector3.one * 0.01f;
                for (int i = 0; i < childs.Count; i++)
                {
                    childs[i].localScale -= Vector3.one * 0.01f;
                }
            }

        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    #endregion Helper Methods
}
