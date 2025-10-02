using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    [SerializeField] public float speed = 2f;
    public GameObject enemyObject;
    public GameObject playerObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirction = enemyObject.transform.position - playerObject.transform.position;
        dirction = -dirction.normalized;
        enemyObject.transform.position += dirction * Time.deltaTime * speed;
    }

    void FixedUpdate()
    {
        
    }
}
