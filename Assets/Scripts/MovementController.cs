using UnityEngine;
using TMPro;
using System;
using static Collectible;

public class MovementController : MonoBehaviour
{
    public bool a = true;
    public bool current = false;
    public float speed = 5.0f; 
    public float jumpForce = 5f; 
    public static int lv = 0;
    public int score;
    public int maxScore;
    public Rigidbody rb;
    public TMP_Text text1; 
    public TMP_Text text2;
    public TMP_Text text3;
    public TMP_Text text4;
    public GameObject manager;
    public GameObject button;
    public GameObject collisionImage;
    public AudioSource collisionAudio;
    public AudioSource levelCompleteAudio;
    private ManagerScript gameManagerScript;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        e_CoinCollection += ScoreUpdate;
        e_CoinCollection += WinPrompt;

        CollisionComponents();
        MaxScore();

        
        SetScoreTextAlignment();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.z = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }

        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);

        if (Input.GetKey(KeyCode.Space) && a == true)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.VelocityChange);
            a = false;
        }
    }

    private void MaxScore()
    {
        if (manager != null)
        {
            gameManagerScript = manager.GetComponent<ManagerScript>();
            if (gameManagerScript != null)
            {
                maxScore = gameManagerScript.GetMaxScore();
            }
        }
    }

    private void CollisionComponents()
    {
        if (collisionImage != null)
        {
            collisionImage.gameObject.SetActive(false);
        }

        if (collisionAudio == null)
        {
            collisionAudio = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        a = true;

        if (collision.gameObject.CompareTag("ResetPlane"))
        {
            if (collisionAudio != null)
            {
                collisionAudio.Play();
            }

            ResetPosition();
        }
    }

    private void ScoreUpdate(object o, EventArgs e)
    {
        if (text1 != null)
        {
            score++;
            text1.text = "PUNKTY: " + score;
            text4.text = "poziom: " + (lv + 1);
        }
    }

    private void WinPrompt(object o, EventArgs e)
    {
        if (score >= maxScore)
        {
            if (lv == 0)
            {
                if (text2 != null) text2.text = "im dalej tym gorzej";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
                lv++;
                score = 0;
            }
            else if (lv == 1)
            {
                if (text2 != null) text2.text = "KONIEC GRY!";
                if (button != null) button.SetActive(true);
                if (levelCompleteAudio != null) levelCompleteAudio.Play();
                if (text3 != null) text3.text = "Zakończ grę";
                lv = 0;
                score = 0;
            }
        }
    }

    private void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
    }

    
    private void SetScoreTextAlignment()
    {
        if (text1 != null)
        {
            
            text1.alignment = TextAlignmentOptions.Right;

            
            RectTransform rectTransform = text1.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(1, 1); 
                rectTransform.anchorMax = new Vector2(1, 1); 
                rectTransform.pivot = new Vector2(1, 1); 
                rectTransform.anchoredPosition = new Vector2(-10, -10); 
            }
        }
    }
}