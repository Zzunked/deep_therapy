using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] public SpriteRenderer frameSpriteRenderer;
    private bool isMouseEntered = false;
    public bool isClicked = false;
    private bool isEnabled = false;

    void Start()
    {
        SetFrameInvisible();
    }

    public void Disable()
    {
        isEnabled = false;
        isMouseEntered = false;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    private void OnMouseEnter()
    {
        if (isEnabled)
        {
            SetFrameVisible();
            isMouseEntered = true;
        }
    }

    private void OnMouseExit()
    {
        if (isEnabled)
        {
            SetFrameInvisible();
            isMouseEntered = false;
        }
    }

    public void SetFrameVisible()
    {
        frameSpriteRenderer.sortingOrder = 2;
    }

    public void SetFrameInvisible()
    {
        frameSpriteRenderer.sortingOrder = -1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseEntered && isEnabled)
        {
            Debug.Log(gameObject.name + " CLICKED!");
            isClicked = true;
        }
    }
}