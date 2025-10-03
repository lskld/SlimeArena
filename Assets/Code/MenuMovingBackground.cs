using UnityEngine;

public class MenuMovingBackground : MonoBehaviour
{

    [SerializeField]public float speed;
    private Renderer bgRend;
    void Start()
    {
        bgRend = this.GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        bgRend.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
