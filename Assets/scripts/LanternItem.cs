using UnityEngine;

public class LanternItem : MonoBehaviour
{
    public float restoreAmount = 1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10f);  // 2초 후에 아이템 파괴
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger Entered with: " + other.gameObject.name);  // 충돌 확인

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player collided with the item!");  // 충돌 메시지 확인

            LanternLightController lanternLight = other.GetComponentInChildren<LanternLightController>();
            if (lanternLight != null)
            {
                lanternLight.RestoreLanternRange(restoreAmount);
            }
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("Collided with ground");  // 바닥 충돌 확인

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}