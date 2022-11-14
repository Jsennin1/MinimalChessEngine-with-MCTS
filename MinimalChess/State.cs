using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MinimalChess
{
    public class State
    {
        Board board;
        Color playerColor;
        int visitCount;
        double winScore = 0;
        Move diffirenceWithRootBoard;
        
        public void CopyState(State state)
        {
            board = new Board(state.getBoard());
            playerColor = state.playerColor;
            visitCount = state.visitCount;
            winScore = state.winScore;

        }
        public void setBoard(Board rt)
        {
            board = rt;
        }
        public Move getMove()
        {
            return diffirenceWithRootBoard;
        }
        public Board getBoard()
        {
            return board;
        }
        public void setPlayerColor(Color plyclr)
        {
            playerColor = plyclr;
        }
        public Color getPlayerColor()
        {
            return playerColor;
        }
        public void togglePlayer()
        {
            playerColor = Pieces.Flip(playerColor);
        }
        public void addScore(double score)
        {
            winScore += score;
        }
        public void setWinScore(double score)
        {
            winScore = score;
        }
        public double getWinScore()
        {
            return winScore;
        }

        // copy constructor, getters, and setters
        private static List<Move> ListMoves(Board board, int depth = 0)
        {
            IterativeSearch search = new IterativeSearch(depth, board);
            Move[] line = search.PrincipalVariation;

            return new LegalMoves(board);
          
        }
        public bool CheckStatus() 
        {
            List<Move> possibleMoves = ListMoves(board);
            if(possibleMoves.Count < 5)
            Console.WriteLine("possibleMoves.Count " + possibleMoves.Count);

            if (possibleMoves.Count > 0) return true;
            else return false;
        }
        // constructs a list of all possible states from current state
        public List<State> getAllPossibleStates()
        {
            List<Move> possibleMoves = ListMoves(board);
            var possibleStates = new List<State>();
            
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                var state = new State();
                state.playerColor = playerColor;
                state.board = new Board(board);
                state.diffirenceWithRootBoard = possibleMoves[i];
                state.board.Play(possibleMoves[i]);
                possibleStates.Add(state);
            }
            return possibleStates;
        }
        public void randomPlay()
        {
            /* get a list of all possible positions on the board and 
               play a random move */
            var emptyPos = ListMoves(board);
            Random rnd = new Random();
            int randomChildNum = rnd.Next(emptyPos.Count);
            board.Play(emptyPos[randomChildNum]);
        }
        public Color getOpponent()
        {
            return Pieces.Flip(playerColor);
        }
        public void incrementVisit()
        {
            visitCount++;
        }
        public int getVisitCount()
        {
            return visitCount;
        }
    }
}
