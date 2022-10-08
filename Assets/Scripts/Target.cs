using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    public int pointValue;
    public ParticleSystem explosionParticle;

    private Rigidbody targetRb;
    private GameManager gameManager;

    private float minSpeed = 14.0f;
    private float maxSpeed = 18.0f;
    private float torqueRange = 10.0f;
    private float positionRange = 4.0f;
    private float ySpawnPos = -6.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        targetRb = GetComponent<Rigidbody>();

        transform.position = RandomSpawnPosition();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(new Vector3(RandomTorque(), RandomTorque(), RandomTorque()), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad") && gameManager && gameManager.isGameActive) gameManager.LoseLife();
    }

    public void DestroyTarget()
    {
        if (gameManager && gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }

    Vector3 RandomSpawnPosition()
    {
        return new Vector3(Random.Range(-positionRange, positionRange), ySpawnPos);
    }
}
