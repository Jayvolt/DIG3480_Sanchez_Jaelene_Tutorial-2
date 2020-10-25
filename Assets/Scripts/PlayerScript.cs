using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    
private bool isOnGround;
public Transform groundcheck;
Animator anim;
public float checkRadius;
public LayerMask allGround;
private Rigidbody2D rd2d;
public float speed;
public Text lives;
private int livesValue;
public Text outcomeText;
public Text score;
public static int scoreValue;
private bool facingRight = true;

public AudioClip musicClipOne;
public AudioClip musicClipTwo;
public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        scoreValue = 0;
        livesValue = 3;
        outcomeText.text = "";

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

        SetLivesText();
        SetScoreText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    
    if (facingRight == false && hozMovement > 0)
   {
     Flip();
   }
else if (facingRight == true && hozMovement < 0)
   {
     Flip();
   }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetScoreText();
            Destroy(collision.collider.gameObject);
        }
         if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("State", 2);
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
            else
            {
            anim.SetInteger("State", 0);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
            anim.SetInteger("State", 1);
            }
             if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
            anim.SetInteger("State", 0);
            }
        }
        else{
            anim.SetInteger("State", 2);
        }
    }
    void SetScoreText()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4)
        {
            transform.position = new Vector3(46.0f, 0.0f, 0.0f);
        }
        if (scoreValue == 8)
        {
                Debug.Log("You got the 4 coins");
        musicSource.loop = false;
        musicSource.Stop();

        musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = true;
            outcomeText.text = "You Win! Game created by Jaelene Sanchez";
        }
    }
        void SetLivesText()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0)
        {
            Destroy(gameObject);
            outcomeText.text = "You Lose!";
        }
    }
    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
 