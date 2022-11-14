using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static System.Formats.Asn1.AsnWriter;


namespace MinimalChess
{
    public class MonteCarloTreeSearch
    {
        static int WIN_SCORE = 10;
        Color opponent,player;
        Tree tree;
        Node rootNode;
        int maxPlayOutCount = 300;
        public MonteCarloTreeSearch(Board board)
        {
            player = board.SideToMove;
            opponent = Pieces.Flip(board.SideToMove);
            tree = new Tree();
            rootNode = tree.getRoot();
            rootNode.getState().setBoard(board);
            rootNode.getState().setPlayerColor(opponent);
        }
        public Move ResultOfMTCStree()
        {
            // define an end time which will act as a terminating condition

            Node winnerNode = rootNode.getChildWithMaxScore();
            tree.setRoot(winnerNode);
            return winnerNode.getState().getMove();
        }

        public void CreatingMCTStree()
        {
            Console.WriteLine("mcts");
            Node promisingNode = selectPromisingNode(rootNode);
            Console.WriteLine("mcts1");

            if (promisingNode.getState().CheckStatus())
            {
                expandNode(promisingNode);
                Console.WriteLine("mcts2");

            }
            Node nodeToExplore = promisingNode;
            if (promisingNode.getChildArray().Count > 0)
            {
                Console.WriteLine("mcts3");

                nodeToExplore = promisingNode.getRandomChildNode();
            }
            Console.WriteLine("mcts4");

            Color playoutResult = simulateRandomPlayout(nodeToExplore);
            Console.WriteLine("mcts5");

            backPropogation(nodeToExplore, playoutResult);
            Console.WriteLine("mcts6");

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
                newNode.getState().setPlayerColor(node.getState().getOpponent());
                node.getChildArray().Add(newNode);
            }
        }
        private void backPropogation(Node nodeToExplore, Color playercolor)
        {
            Node tempNode = nodeToExplore;
            while (tempNode != null)
            {
                tempNode.getState().incrementVisit();
                if (tempNode.getState().getPlayerColor() == playercolor)
                {
                    tempNode.getState().addScore(WIN_SCORE);
                }
                tempNode = tempNode.getParent();
            }
        }
        //if player wins return 1, lose -1,draw 0
        private Color simulateRandomPlayout(Node node)
        {
            int playoutCount= 0;
            Node tempNode = new Node();
            tempNode.copyStateOfNode(node);
            State tempState = tempNode.getState();
            Color boardStatus = tempState.getBoard().SideToMove;
            if (!tempState.CheckStatus() && boardStatus != player)
            {
                tempNode.getParent().getState().setWinScore(int.MinValue);
                return player;
            }
            while (tempState.CheckStatus() && playoutCount < maxPlayOutCount)
            {
                playoutCount++;
                //tempState.togglePlayer();
                tempState.randomPlay();
            }
            //dusman hareket edemiyorsa draw olabilir
            if(playoutCount < maxPlayOutCount)
                return tempState.getBoard().SideToMove == player ? opponent : player;
            int whitePoint=0, blackPoint =0;

            (whitePoint, blackPoint) = tempState.getBoard().GiveTotalPiecePoints();
            if (whitePoint == blackPoint)
                return 0;
            else if (whitePoint > blackPoint)
                return player == Color.White ? player : opponent;
            else
                return player == Color.Black ? player : opponent;

        }
    }

}