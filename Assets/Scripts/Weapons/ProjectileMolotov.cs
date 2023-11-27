using UnityEngine;

public class ProjectileMolotov : Throwable
{
    private bool hasExploded = false;

    private float explosionRadius;
    private float explosionDamage;

    [SerializeField] GameObject explosionFx;
    [SerializeField] AudioClip explosionSound;

    [SerializeField] GameObject firePrefab;
    private float fireDuration;

    void Start(){
        explosionDamage = throwable.damage;
        explosionRadius = throwable.explosionRadius;
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag != "Enemy") Explode();
    }
    void Explode(){
        // explosionSound.Play();
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 0.25f);
        Instantiate(explosionFx, transform.position, transform.rotation);
        
        Collider[] damageColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyTargets in damageColliders){
            if (nearbyTargets.TryGetComponent(out IDamage dmg)){
                GameObject fireObject;
                fireObject = Instantiate(firePrefab, nearbyTargets.transform.position, nearbyTargets.transform.rotation);
                fireObject.transform.SetParent(nearbyTargets.transform);
            }
        }
        // Rigidbody rb = GetComponent<Rigidbody>();
        // rb.isKinematic = false;

        // GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }
}