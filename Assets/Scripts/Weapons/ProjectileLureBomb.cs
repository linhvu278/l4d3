using UnityEngine;

public class ProjectileLureBomb : Throwable
{
    private float fuseTime; // = 4f;
    private float explosionRadius; // = 5f;
    private float explosionForce; // = 1000f;
    private float explosionDamage; // = 100f;
    private float lureRadius = 7.5f;

    [SerializeField] GameObject explosionFx;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioSource lureSound;

    private LayerMask lureMask;

    void Start(){
        lureMask = LayerMask.GetMask("Lure");
        fuseTime = throwable.fuseTime;
        explosionDamage = throwable.damage;
        // explosionForce = throwable.explosionForce;
        explosionRadius = throwable.explosionRadius;
        lureSound.Play();
        Lure();
    }
    void Lure(){
        Collider[] lureColliders = Physics.OverlapSphere(transform.position, lureRadius);
        foreach (Collider nearbyTargets in lureColliders){
            EnemyController ec = nearbyTargets.GetComponent<EnemyController>();
            if (ec != null){
                ec.SetTarget(transform, lureMask);
            }
        }
        Invoke(nameof(Explode), fuseTime);
    }
    void Explode(){
        Instantiate(explosionFx, transform.position, transform.rotation);

        Collider[] damageColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyTargets in damageColliders){
            IDamage dmg = nearbyTargets.GetComponent<IDamage>();
            if (dmg != null){
                dmg.TakeDamage(explosionDamage);
            }
        }
        GetComponent<MeshRenderer>().enabled = false;
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lureRadius);
    }
}
