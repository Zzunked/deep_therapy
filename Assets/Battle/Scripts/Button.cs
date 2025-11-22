using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer _frameSpriteRenderer;
    [SerializeField] private bool _isMouseEntered = false;
    [SerializeField] private bool _isEnabled = true;
    private bool _isClicked = false;

    public bool IsClicked
    {
        get => _isClicked;
        private set => _isClicked = value;
    }

    void Awake()
    {   
        _frameSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetFrameInvisible();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isMouseEntered && _isEnabled)
        {
            Debug.Log(gameObject.name + " CLICKED!");
            IsClicked = true;
        }
    }

    public void Disable()
    {
        _isEnabled = false;
        _isMouseEntered = false;
        _isClicked = false;
    }

    public void Enable()
    {
        _isEnabled = true;
    }

    private void OnMouseEnter()
    {
        if (_isEnabled)
        {
            SetFrameVisible();
            _isMouseEntered = true;
        }
    }

    private void OnMouseExit()
    {
        if (_isEnabled)
        {
            SetFrameInvisible();
            _isMouseEntered = false;
        }
    }

    public void SetFrameVisible()
    {
        _frameSpriteRenderer.sortingOrder = 2;
    }

    public void SetFrameInvisible()
    {
        _frameSpriteRenderer.sortingOrder = -1;
    }
}