using UnityEngine;


public class Heart : MonoBehaviour
{
    [SerializeField] private Sprite _heart_100;
    [SerializeField] private Sprite _heart_75;
    [SerializeField] private Sprite _heart_50;
    [SerializeField] private Sprite _heart_25;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _renderer.sprite = _heart_100;
    }

    public void UpdateHeart(float heartPercent)
    {
        if(heartPercent > 0.75)
        {
            _renderer.sprite = _heart_100;
        }
        else if(heartPercent > 0.5)
        {
            _renderer.sprite = _heart_75;
        }
        else if(heartPercent > 0.25)
        {
            _renderer.sprite = _heart_50;
        }
        else if(heartPercent > 0)
        {
            _renderer.sprite = _heart_25;
        }
        else
        {
            _renderer.sprite = null;
        }
    }
}
