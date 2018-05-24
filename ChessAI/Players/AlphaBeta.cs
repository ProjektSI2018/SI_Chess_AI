using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessAI
{
    class AlphaBeta : Player
    {
        struct Node
        {
            public Point[] move;
            public double eval;
        }
        int depth;

        public AlphaBeta(Color color, int searchDepth = 4) : base(color)
        {
            depth = searchDepth;
        }

        private Node DoAlphaBeta(bool isMin, Board board, int depth, double alpha = 0, double beta = 1)
        {
            Node MinMax = new Node()
            {
                eval = isMin ? 1 : 0
            };
            if (depth == 0)
            {
                return new Node() { move = null, eval = board.Evaluate(color) };
            }
            List<Point> playerPieces = board.GetAllPiecesPositions(isMin ? Board.OppositeColor(color) : color);
            foreach (Point piecePos in playerPieces)
            {

                foreach (Point move in board.BoardTab[piecePos.x, piecePos.y].GetValidMoves(board, piecePos))
                {
                    Point[] tempMove = new Point[2] { piecePos, move };

                    board.Execute(tempMove);
                    
                    Node result = DoAlphaBeta(!isMin, board, depth - 1, alpha, beta);
                    
                    board.UndoMove(1);

                    if ((isMin && result.eval <= MinMax.eval) || (!isMin && result.eval >= MinMax.eval))
                    {
                        MinMax.eval = result.eval;
                        MinMax.move = tempMove;
                    }
                    if (isMin)
                        beta = Math.Min(result.eval, beta);
                    else
                        alpha = Math.Max(result.eval, alpha);
                    if (beta <= alpha)
                        return MinMax;
                }
            }
            return MinMax;
        }


        public override Point[] Decide(Board board)
        {
            Node alfaBeta = DoAlphaBeta(false, board, depth);
            return alfaBeta.move;
        }

        public override char PromotePawn(char[] options)
        {
            return 'Q';
        }
    }
}
