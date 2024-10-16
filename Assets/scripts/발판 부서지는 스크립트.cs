using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public Sprite 백퍼, 칠십퍼, 삼십퍼, 부서졌을때;
    private SpriteRenderer spriteRenderer;
    private PlatformManager platformManager;

    public void Initialize(PlatformManager manager)
    {
        platformManager = manager;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
