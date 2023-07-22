using System;
using System.Collections.Generic;
using System.Linq;
using ChessChallenge.API;

namespace Chess_Challenge.My_Bot;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        Random random = new Random();
        var moves = board.GetLegalMoves();
        
        Dictionary<Move, int> moveStrengthMapper = new();
  
        foreach (var move in moves)
        {
            if (MoveIsCheckmate(board, move)) return move;
            moveStrengthMapper[move] = CalculatePoints(board, move);
        }

        var strongestMoveKeyValuePair = moveStrengthMapper.MaxBy(x => x.Value);
        return strongestMoveKeyValuePair.Value == 0 ? moves[random.Next(0, moves.Length)] : strongestMoveKeyValuePair.Key;
    }

    private bool CanBeCaptured(Board board, Square square, Move move)
    {
        board.MakeMove(move);
        bool result = board.GetLegalMoves().Any(x => x.TargetSquare == square);
        board.UndoMove(move);
        return result;
    }

    private List<PieceType> GetCaptureAblePieces(Board board)
    {
        return board.GetLegalMoves(true).Select(x => x.MovePieceType).ToList();
    }

    int CalculatePoints(Board board, Move move)
    {
        int capturePoints = CanBeCaptured(board, move.TargetSquare, move) ? (int)move.MovePieceType : 0;
        int points = (int)move.CapturePieceType + ((int)move.MovePieceType * -1 + 6) / 2 + (move.IsPromotion ? 10 : 0) - capturePoints;
        return points;
    }
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }
}