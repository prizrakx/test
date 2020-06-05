using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class platform : NetworkBehaviour
{
    public GameObject dependent_platform = null;
    public bool can_control = false;
    public bool is_local_game = false;
    public float speed = 10f;
    public ui canvas = null;

    private Rigidbody2D rb = null;
    private Rigidbody2D dp_rb = null;
    private float dx = 0;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (dependent_platform != null) {
            dp_rb = dependent_platform.GetComponent<Rigidbody2D>();
        }
        dx = Screen.width / 2;
    }

    void Update()
    {
        if ((!isLocalPlayer) && (!is_local_game)) {
            return;
        }

        if ((can_control) && (!EventSystem.current.IsPointerOverGameObject())) {
            Vector2 vel = Vector2.zero;
            if (Input.GetMouseButton(0)) {
                if ((Input.mousePosition.x > dx) && (gameObject.transform.position.x < 5.5)) {
                    vel = new Vector2(1, 0) * speed;
                }
                if ((Input.mousePosition.x < dx) && (gameObject.transform.position.x > -5.5)) {
                    vel = new Vector2(-1, 0) * speed;
                }
            }
            if (Input.touchCount > 0) {
                if ((Input.GetTouch(0).position.x > dx) && (gameObject.transform.position.x < 5.5)) {
                    vel = new Vector2(1, 0) * speed;
                }
                if ((Input.GetTouch(0).position.x < dx) && (gameObject.transform.position.x > -5.5)) {
                    vel = new Vector2(-1, 0) * speed;
                }
            }

            rb.velocity = vel;
            if (dependent_platform != null) {
                dp_rb.velocity = vel;
            }
        }
    }

    public override void OnStartLocalPlayer() {
        can_control = true;
        base.OnStartLocalPlayer();
    }

    public void reInit() {
        can_control = false;
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(0, this.transform.position.y, 0);
        if (dependent_platform != null) {
            dp_rb.velocity = Vector2.zero;
            dependent_platform.transform.position = new Vector3(0, dependent_platform.transform.position.y, 0);
        }
    }

    private void OnDestroy() {
        if (canvas != null) {
            Destroy(canvas.GetBall());
        }
    }
}
