using UnityEngine;

public class Peg : MonoBehaviour
{
    public BoardController board;
    public int row;
    public int col;

    public bool IsOccupied 
    {
        get => _isOccupied; 
        set 
        { 
            _isOccupied = value;
            UpdateColor();
        }
    }
    public bool IsSelected
    {
        get => _isSelected;
        set { 
            _isSelected = value;
            UpdateColor();
        }
    }
    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {
            _isHighlighted = value;
            UpdateColor();
        }
    }

    [Header("Colors")]
    public Color OccupiedColor;
    public Color SelectedColor;
    public Color HighLightedColor;
    private Color EmptyColor = Color.black;

    private bool _isOccupied = true;
    private bool _isSelected = false;
    private bool _isHighlighted = false;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateColor()
    {
        if (_isSelected) _renderer.color = SelectedColor;
        else if(_isHighlighted) _renderer.color = HighLightedColor;
        else if(_isOccupied) _renderer.color = OccupiedColor;
        else _renderer.color = EmptyColor;
    }

    public void Click()
    {
        board.PegClicked(this);
    }
}
