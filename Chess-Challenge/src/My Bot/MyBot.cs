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
        Move[] moves = board.GetLegalMoves();

        Dictionary<Move, int> moveStrengthMapper = new();
  
        foreach (var move in moves)
        {
            if (MoveIsCheckmate(board, move)) return move;
            
            moveStrengthMapper[move] = (int)move.CapturePieceType + ((int)move.MovePieceType * -1 + 6) / 2 + (move.IsPromotion ? 10 : 0);
        }

        var strongestMoveKeyValuePair = moveStrengthMapper.MaxBy(x => x.Value);
        return strongestMoveKeyValuePair.Value == 0 ? moves[random.Next(0, moves.Length)] : strongestMoveKeyValuePair.Key;
    }
    
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }
}