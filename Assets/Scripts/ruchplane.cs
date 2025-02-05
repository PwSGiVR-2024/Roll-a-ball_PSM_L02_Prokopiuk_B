using UnityEngine;

public class WindaControl : MonoBehaviour
{
    public float speed = 2.5f; // Prędkość poruszania się windy
    private float range = 5.0f; // Zakres ruchu windy
    private int direction = 1; // Kierunek ruchu (1 = w prawo, -1 = w lewo)

    private float startPositionX; // Początkowa pozycja X

    void Start()
    {
        startPositionX = transform.position.x; // Zapisz początkową pozycję X
    }

    void Update()
    {
        // Poruszaj windę w osi X
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        // Sprawdź, czy winda osiągnęła granicę zakresu
        if (transform.position.x >= startPositionX + range || transform.position.x <= startPositionX - range)
        {
            direction *= -1; // Zmień kierunek ruchu
        }
    }
}