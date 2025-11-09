using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer _frameSpriteRenderer;
    private bool _isMouseEntered = false;
    private bool _isClicked = false;
    private bool _isEnabled = false;

    public bool IsClicked
    {
        get { return _isClicked; }
        set { _isClicked = value; }
    }

    void Start()
    {
        _frameSpriteRenderer = GetComponent<SpriteRenderer>();
        SetFrameInvisible();
    }

    public void Disable()
    {
        _isEnabled = false;
        _isMouseEntered = false;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isMouseEntered && _isEnabled)
        {
            Debug.Log(gameObject.name + " CLICKED!");
            _isClicked = true;
        }
    }
}