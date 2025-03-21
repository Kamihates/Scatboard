using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos, length; // Position initiale et longueur du bg
    [SerializeField] private GameObject cam;
    [Range(0f, 1f)][SerializeField] private float parallaxEffect; // Vitesse à laquelle le bg doit se déplacer par rapport à la caméra


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calcule la distance à laquelle le background doit se déplacer en fonction de la caméra
        float distance = cam.transform.position.x * parallaxEffect; 
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Vérifie si le background doit être bouclé pour éviter les "trous" visuels
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
