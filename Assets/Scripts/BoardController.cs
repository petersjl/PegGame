using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject PegPrefab;
    public GameObject Background;
    public int BoardSize;
    public List<List<Peg>> Board;
    public Point StartingHole;


    private List<Peg> _Pegs;
    private Peg SelectedPeg;

    private const double PegDist = 1.5;

    // Start is called before the first frame update
    void Start()
    {
        SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PegClicked(Peg peg)
    {
        if (peg == SelectedPeg)
        {
            SelectedPeg = null;
            ClearStatuses();
        }
        else if(peg.IsOccupied)
        {
            SelectedPeg = null;
            ClearStatuses();
            SelectedPeg = peg;
            SelectedPeg.IsSelected = true;
            HighlightOptions();
        }
        else if(peg.IsHighlighted)
        {
            MovePeg(peg);
            ClearStatuses();
        }
    }

    private void SetupBoard()
    {
        Board= new List<List<Peg>>();
        _Pegs= new List<Peg>();
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var sideLength = (BoardSize - 1) * PegDist;
        var firstRowHeight = sideLength * Math.Sqrt(3) / 2 / 3; // 1/3 height of whole triangle
        var rowHeightDiff = Math.Sqrt(Math.Pow(PegDist, 2) - Math.Pow(PegDist / 2, 2));
        var evenOffset = BoardSize % 2 == 0 ? 1 : 0;

        var background = Instantiate(Background, transform);
        background.transform.localScale = new Vector3((float)sideLength + 3, (float)sideLength + 3);

        for(int i = 0; i < BoardSize; i++)
        {
            int rowSize = BoardSize - i;
            var rowHeight = firstRowHeight - (i * rowHeightDiff);
            var leftOffset = (-PegDist * Math.Floor((double)rowSize / 2)) + ((PegDist / 2) * ((i + evenOffset) % 2) );
            List<Peg> row = new List<Peg>();
            for(int j = 0; j < rowSize; j++)
            {
                var pegObject = Instantiate(PegPrefab, transform);
                pegObject.transform.localPosition = new Vector3((float)(leftOffset + (PegDist * j)), (float)rowHeight, 1);
                Peg p = pegObject.GetComponent<Peg>();
                row.Add(p);
                _Pegs.Add(p);
                p.board = this;
                p.row = i;
                p.col = j;
            }
            Board.Add(row);
        }
        if(StartingHole != null)
        {
            if(StartingHole.Y >= 0 && StartingHole.Y < Board.Count)
            {
                var row = Board[StartingHole.Y];
                if(StartingHole.X >= 0 && StartingHole.X < row.Count)
                {
                    row[StartingHole.X].IsOccupied = false;
                    return;
                }
            }
        }
        Board[0][0].IsOccupied = false;
    }

    private bool IsBoardOpenAt(int x, int y)
    {
        if(y < 0 || y >= BoardSize) return false;
        var row = Board[y];
        if(x < 0 || x >= row.Count) return false;
        return !row[x].IsOccupied;
    }

    private void ClearStatuses()
    {
        foreach(var peg in _Pegs)
        {
            peg.IsSelected= false;
            peg.IsHighlighted = false;
        }
    }

    private void MovePeg(Peg destination)
    {
        Debug.Log($"Moving {SelectedPeg.row}, {SelectedPeg.col} to {destination.row}, {destination.col}");
        var jumpRow = (destination.row - SelectedPeg.row) / 2 + SelectedPeg.row;
        var jumpCol = (destination.col - SelectedPeg.col) / 2 + SelectedPeg.col;

        SelectedPeg.IsOccupied = false;
        destination.IsOccupied = true;
        destination.IsHighlighted = false;
        Board[jumpRow][jumpCol].IsOccupied = false;
    }

    private void HighlightOptions()
    {
        var row = SelectedPeg.row;
        var col = SelectedPeg.col;

        if (IsBoardOpenAt(col, row - 2) && !IsBoardOpenAt(col, row - 1)) Board[row - 2][col].IsHighlighted = true; // check up
        if (IsBoardOpenAt(col, row + 2) && !IsBoardOpenAt(col, row + 1)) Board[row + 2][col].IsHighlighted = true; // check down
        if (IsBoardOpenAt(col - 2, row) && !IsBoardOpenAt(col - 1, row)) Board[row][col - 2].IsHighlighted = true; // check left
        if (IsBoardOpenAt(col + 2, row) && !IsBoardOpenAt(col + 1, row)) Board[row][col + 2].IsHighlighted = true; // check right
        if (IsBoardOpenAt(col + 2, row - 2) && !IsBoardOpenAt(col + 1, row - 1)) Board[row - 2][col + 2].IsHighlighted = true; // check right diag
        if (IsBoardOpenAt(col - 2, row + 2) && !IsBoardOpenAt(col - 1, row + 1)) Board[row + 2][col - 2].IsHighlighted = true; // check left diag
    }
}
