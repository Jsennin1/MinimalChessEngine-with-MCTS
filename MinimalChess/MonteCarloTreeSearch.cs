using System;
using System.Diagnostics;


namespace MinimalChess
{
    public class MonteCarloTreeSearch
    {
        static int WIN_SCORE = 10;
        int level;
        int opponent;

        public Board findNextMove(Board board, int playerNo, int endTime)
        {
            // define an end time which will act as a terminating condition

            opponent = 3 - playerNo;
            Tree tree = new Tree();
            Node rootNode = tree.getRoot();
            rootNode.getState().setBoard(board);
            rootNode.getState().setPlayerNo(opponent);

            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (timer.Elapsed.TotalSeconds < endTime)
            {
                Node promisingNode = selectPromisingNode(rootNode);
                if (promisingNode.getState().getBoard().checkStatus()
                  == Board.IN_PROGRESS)
                {
                    expandNode(promisingNode);
                }
                Node nodeToExplore = promisingNode;
                if (promisingNode.getChildArray().Count > 0)
                {
                    nodeToExplore = promisingNode.getRandomChildNode();
                }
                int playoutResult = simulateRandomPlayout(nodeToExplore);
                backPropogation(nodeToExplore, playoutResult);
            }
            timer.Stop();

            Node winnerNode = rootNode.getChildWithMaxScore();
            tree.setRoot(winnerNode);
            return winnerNode.getState().getBoard();
        }
        private Node selectPromisingNode(Node rootNode)
        {
            Node node = rootNode;
            while (node.getChildArray().Count != 0)
            {
                node = UCT.findBestNodeWithUCT(node);
            }
            return node;
        }
        private void expandNode(Node node)
        {
            List<State> possibleStates = node.getState().getAllPossibleStates();
            foreach (var state in possibleStates)
            {
                Node newNode = new Node();
                newNode.setState(state);
                newNode.setParent(node);
                newNode.getState().setPlayerNo(node.getState().getOpponent());
                node.getChildArray().Add(newNode);
            }
        }
        private void backPropogation(Node nodeToExplore, int playerNo)
        {
            Node tempNode = nodeToExplore;
            while (tempNode != null)
            {
                tempNode.getState().incrementVisit();
                if (tempNode.getState().getPlayerNo() == playerNo)
                {
                    tempNode.getState().addScore(WIN_SCORE);
                }
                tempNode = tempNode.getParent();
            }
        }
        private int simulateRandomPlayout(Node node)
        {
            
            Node tempNode = new Node();
            tempNode.copyStateOfNode(node);
            State tempState = tempNode.getState();
            int boardStatus = tempState.getBoard().checkStatus();
            if (boardStatus == opponent)
            {
                tempNode.getParent().getState().setWinScore(int.MinValue);
                return boardStatus;
            }
            while (boardStatus == Board.IN_PROGRESS)
            {
                tempState.togglePlayer();
                tempState.randomPlay();
                boardStatus = tempState.getBoard().checkStatus();
            }
            return boardStatus;
        }
    }

}