using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float impactForce;

    private BoxCollider cd;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;

    [SerializeField] private GameObject bulletImpactFX;
    [SerializeField] private ScoreMng scoreMng; // Reference to the ScoreMng script

    private Vector3 startPosition;
    private float flyDistance;
    private bool bulletDisabled;

    // New variable to track bullet ownership
    public bool isPlayerBullet = false;

    void Start()
    {
        if (scoreMng == null)
        {
            scoreMng = FindObjectOfType<ScoreMng>();
            if (scoreMng == null)
            {
                Debug.LogError("ScoreMng not found in the scene.");
            }
        }
    }

    protected virtual void Awake()
    {
        cd = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void BulletSetup(float flyDistance = 100, float impactForce = 100, bool isPlayerBullet = false)
    {
        this.impactForce = impactForce;

        bulletDisabled = false;
        cd.enabled = true;
        meshRenderer.enabled = true;

        trailRenderer.Clear();
        trailRenderer.time = .25f;
        startPosition = transform.position;
        this.flyDistance = flyDistance + .5f; // magic number .5f is a length of tip of the laser
        this.isPlayerBullet = isPlayerBullet; // Set bullet ownership
    }

    protected virtual void Update()
    {
        FadeTrailIfNeeded();
        DisableBulletIfNeeded();
        ReturnToPoolIfNeeded();
    }

    protected void ReturnToPoolIfNeeded()
    {
        if (trailRenderer.time < 0)
            ReturnBulletToPool();
    }

    protected void DisableBulletIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance && !bulletDisabled)
        {
            cd.enabled = false;
            meshRenderer.enabled = false;
            bulletDisabled = true;
        }
    }

    protected void FadeTrailIfNeeded()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            trailRenderer.time -= 2 * Time.deltaTime; // magic number 2 is chosen through testing
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx();

        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        Enemy_Shield shield = collision.gameObject.GetComponent<Enemy_Shield>();

        // Only increase score if the bullet belongs to the player and hits an enemy
        if (isPlayerBullet && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")))
        {
            if (scoreMng != null)
            {
                scoreMng.IncreaseScore(10); // Increase score by 10
                Debug.Log("Player's bullet hit an Enemy or Boss. Increasing score!");
            }
        }

        if (shield != null)
        {
            shield.ReduceDurability();
            return;
        }

        if (enemy != null)
        {
            Vector3 force = rb.linearVelocity.normalized * impactForce;
            Rigidbody hitRigidbody = collision.collider.attachedRigidbody;

            enemy.GetHit();
            enemy.DeathImpact(force, collision.contacts[0].point, hitRigidbody);
        }

        ReturnBulletToPool();
    }

    protected void CreateImpactFx()
    {
        GameObject newImpactFx = ObjectPool.instance.GetObject(bulletImpactFX, transform);
        ObjectPool.instance.ReturnObject(newImpactFx, 1);

        // Check if the bullet is a player's bullet
        if (isPlayerBullet && scoreMng != null)
        {
            scoreMng.IncreaseScore(5); // Increase score by 5 when impact VFX is created
            Debug.Log("Score increased for impact VFX creation!");
        }
    }

    protected void ReturnBulletToPool() => ObjectPool.instance.ReturnObject(gameObject);
}
