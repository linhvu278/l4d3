using UnityEngine;

public class ProjectileMolotov : Throwable
{
    // private bool hasExploded = false;

    private float explosionRadius, explosionDamage, fuseTime;

    [SerializeField] GameObject explosionFx;
    [SerializeField] AudioClip explosionSound;

    [SerializeField] GameObject firePrefab;
    private float fireDuration;
    void Explode(){
        // explosionSound.Play();
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 0.25f);
        Instantiate(explosionFx, transform.position, transform.rotation);
        
        Collider[] damageColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyTargets in damageColliders){
            if (nearbyTargets.TryGetComponent(out IFire fire)){
                if (!fire.IsOnFire){
                    fire.IsOnFire = true;
                    fire.FireDamage = explosionDamage;
                    GameObject fireObject = Instantiate(firePrefab, nearbyTargets.transform.position, nearbyTargets.transform.rotation);
                    fireObject.transform.SetParent(nearbyTargets.transform);
                }
            }
        }
        // Rigidbody rb = GetComponent<Rigidbody>();
        // rb.isKinematic = false;

        // GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) => Explode();

    void Update(){
        fuseTime -= Time.deltaTime;
        if (fuseTime < 0) Explode();
    }
    void Start(){
        explosionDamage = throwable.damage;
        explosionRadius = throwable.explosionRadius;
        fuseTime = throwable.fuseTime;
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}