using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length; // Position initiale et longueur du bg
    [SerializeField] private GameObject cam;
    [Range(0f, 1f)][SerializeField] private float parallaxEffect; // Vitesse � laquelle le bg doit se d�placer par rapport � la cam�ra


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calcule la distance � laquelle le background doit se d�placer en fonction de la cam�ra
        float distance = cam.transform.position.x * parallaxEffect; 
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // V�rifie si le background doit �tre boucl� pour �viter les "trous" visuels
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
