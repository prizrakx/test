using UnityEngine;
using UnityEngine.Networking;

public class ball : NetworkBehaviour
{
    private Rigidbody2D rb = null;
    private float speed  = 10f;
    public ui canvas;
    public float min_speed = 5f;
    public float max_speed = 15f;
    public float min_size = 0.5f;
    public float max_size = 1.5f;
    public platform racket = null;

    private Vector2 direction;
    private float top_y = 6;
    private float bottom_y = -6;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        changeDirection(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));

        rb.position = Vector2.zero;

        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        switch (PlayerPrefs.GetInt("ball_color"))
        {
            case 0:
                sr.color = Color.blue;
                break;
            case 1:
                sr.color = Color.red;
                break;
            case 2:
                sr.color = Color.yellow;
                break;
            case 3:
                sr.color = Color.green;
                break;
            default:
                sr.color = Color.blue;
                break;
        }

        speed = Random.Range(min_speed, max_speed);

        float scale = Random.Range(min_size, max_size);
        transform.localScale = new Vector3(scale, scale, 1);

        /* top_y = 7;
        bottom_y = -7; */
    }

    void Update()
    {
        if (transform.position.y > top_y) {
            restartGame();
        }
        if (transform.position.y < bottom_y) {
            restartGame();
        }
    }

    private void restartGame() {
        if (canvas.main_platform.gameObject.activeSelf) {
            Destroy(gameObject);
            canvas.onStartClick();
        } else {
            //canvas.createBall();
            Start();
        }
    }

    private void changeDirection(Vector2 dir) {
        rb.velocity = dir.normalized * speed;
        this.direction = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bool do_reflect = false;

        if (collision.gameObject.CompareTag("collision")) {
            do_reflect = true;
        }
        if (collision.gameObject.CompareTag("racket_up")) {
            do_reflect = true;
            if (canvas != null) {
                if (canvas.main_platform.gameObject.activeSelf) {
                    canvas.addScore();
                }
            }
        }
        if (collision.gameObject.CompareTag("racket_down")) {
            do_reflect = true;
            if (canvas != null) {
                canvas.addScore();
            }
        }

        if (do_reflect) {
            changeDirection(Vector2.Reflect(direction, collision.contacts[0].normal));
        }
    }
}
