/*플랫폼 매니저로 통합
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float speed;
    private bool moveOnContact;

    private bool playerOnPlatform = false;

    public void Initialize(Vector2 movementOffset, float speed, bool moveOnPlayerContact)
    {
        this.speed = speed;
        this.moveOnContact = moveOnPlayerContact;
        startPosition = transform.position;
        endPosition = startPosition + (Vector3)movementOffset;
    }

    void Update()
    {
        if (moveOnContact && !playerOnPlatform) return;

        // 왕복하는 이동 로직
        float time = Mathf.PingPong(Time.deltaTime * speed, 1);
        transform.position = Vector3.Lerp(startPosition, endPosition, time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (moveOnContact && collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (moveOnContact && collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}*/
