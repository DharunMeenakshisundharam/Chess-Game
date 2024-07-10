using UnityEngine;

namespace Chess.Scripts.Core {
    public class ChessPlayerPlacementHandler : MonoBehaviour {
        [SerializeField] public int row, column;

        private void Start() {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
        }

        private void OnMouseDown() {
            ChessBoardPlacementHandler.Instance.ClearHighlights();

            if (gameObject.tag == "Pawn") 
            {
                HandlePawnMovement();
            }
            else if (gameObject.tag == "Rook")
            {
                HandleRookMovement();
            } 
            else if (gameObject.tag == "Bishop") 
            {
                HandleBishopMovement();
            } 
            else if (gameObject.tag == "Queen") 
            {
                HandleQueenMovement();
            } 
            else if (gameObject.tag == "Knight") 
            {
                HandleKnightMovement();
            } 
            else if (gameObject.tag == "King") 
            {
                HandleKingMovement();
            }
        }

        private void HandlePawnMovement() 
        {
            if (row == 1) 
            {
                bool pass = false;
                
                pass=HighlightTileIfEmpty(row + 1, column);
                if(pass)
                {
                    HighlightTileIfEmpty(row + 2, column);
                }
            } 
            else 
            {
                HighlightTileIfEmpty(row + 1, column);
            }
        }

        private void HandleRookMovement() {
            HighlightDirection(row, column, 1, 0);  // Up
            HighlightDirection(row, column, -1, 0); // Down
            HighlightDirection(row, column, 0, 1);  // Right
            HighlightDirection(row, column, 0, -1); // Left
        }

        private void HandleBishopMovement() {
            HighlightDirection(row, column, 1, 1);   // Down-right
            HighlightDirection(row, column, 1, -1);  // Down-left
            HighlightDirection(row, column, -1, 1);  // Up-right
            HighlightDirection(row, column, -1, -1); // Up-left
        }

        private void HandleQueenMovement() {
            HandleRookMovement();
            HandleBishopMovement();
        }

        private void HandleKnightMovement() {
            int[,] moves= {
                {2, 1},{2, -1},{-2, 1},{-2, -1},
                {1, 2},{1, -2},{-1, 2},{-1, -2}
            };

            for (int i = 0;i<moves.GetLength(0);i++)
            {
                int newRow = row + moves[i, 0];
                int newColumn = column + moves[i, 1];

                // Check if the new position is within the board boundaries and not occupied
                if (IsWithinBoard(newRow, newColumn) && IsTileEmpty(newRow, newColumn)) {
                    ChessBoardPlacementHandler.Instance.Highlight(newRow, newColumn);
                    Debug.Log($"Highlight tile at row {newRow}, column {newColumn}");
                }
            }
        }

       private void HandleKingMovement()
{
    int[,] moves = {
        {1, 0},{-1, 0}, {0, 1},{0, -1}, // Horizontal and vertical moves
        {1, 1}, {1, -1}, {-1, 1}, {-1, -1} // Diagonal moves
    };

    for (int i = 0; i < moves.GetLength(0); i++)
    {
        int newRow = row + moves[i, 0];
        int newColumn = column + moves[i, 1];

        // Check if the new position is within the board boundaries and not occupied
        if (IsWithinBoard(newRow, newColumn) && IsTileEmpty(newRow, newColumn))
        {
            ChessBoardPlacementHandler.Instance.Highlight(newRow, newColumn);
            Debug.Log($"Highlight tile at row {newRow}, column {newColumn}");
        }
    }
}

        private void HighlightDirection(int startRow, int startColumn, int rowDirection, int colDirection) {
            int currentRow = startRow;
            int currentColumn = startColumn;
            bool blocked = false;

            while (true) {
                currentRow += rowDirection;
                currentColumn += colDirection;

                if (IsWithinBoard(currentRow, currentColumn)) {
                    if (IsTileEmpty(currentRow, currentColumn)) {
                        if (!blocked) {
                            ChessBoardPlacementHandler.Instance.Highlight(currentRow, currentColumn);
                            Debug.Log($"Highlight tile at row {currentRow}, column {currentColumn}");
                        }
                    } else {
                        if (!blocked) {
                            //ChessBoardPlacementHandler.Instance.ClearHighlights();
                            blocked = true;
                        }
                    }
                } else {
                    break; // Stop if the move goes out of the board
                }
            }
        }

        private bool IsWithinBoard(int row, int column) {
            return row >= 0 && row < 8 && column >= 0 && column < 8;
        }

       private bool IsTileEmpty(int row, int column) {
    Vector3 tilePosition = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(tilePosition, 0.1f);
    foreach (Collider2D collider in colliders) {
        if (collider.gameObject != gameObject && 
            (collider.CompareTag("King") || collider.CompareTag("Queen") || 
             collider.CompareTag("Knight") || collider.CompareTag("Pawn") || 
             collider.CompareTag("Bishop") || collider.CompareTag("Rook"))) {
            return false;
        }
    }
    return true;
}


        private bool HighlightTileIfEmpty(int row, int column) {
            if (IsWithinBoard(row, column) && IsTileEmpty(row, column)) 
            {
                ChessBoardPlacementHandler.Instance.Highlight(row, column);
                Debug.Log($"Highlight tile at row {row}, column {column}");
                return true;
            }
            return false;
        }
    }
}
