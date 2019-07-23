using System;
using System.IO;
using System.Collections.Generic;

namespace Suduko{

    class SudukoBoard{
        //Private
        private int[,] Grid;
        private bool[,,] Possibles;

        private bool SubGridContains (int x, int y, int value) {
            x /= 3;
            y /= 3;
            for (int i = 0; i < 3; i++)
                for (int k = 0; k < 3; k++)
                    if (Grid[(x + i), (y + k)] == value)
                        return true;
            return false;
        }

        private bool ColumnContains(int x, int value) {
            for (int i = 0; i < 9; i++)
                if (Grid[x, i] == value)
                    return true;
            return false;
        }

        private bool RowContains(int y, int value) {
            for (int i = 0; i < 9; i++)
                if (Grid[i, y] == value)
                    return true;
            return false;
        }

        private bool ValueIsAllowed (int x, int y, int value) {
            return (!SubGridContains(x, y, value) && !ColumnContains(x, value) && !RowContains(y, value));
            //return (value <= 9) && (Possibles[x,y,value])
        }

        private void RemovePossibilityFromColumn(int x, int value) {
            for (int i = 0; i < 9; i++)
                Possibles[x, i, value] = false;
        }

        private void RemovePossibilityFromRow(int y, int value) {
            for (int i = 0; i < 9; i++)
                Possibles[i, y, value] = false;
        }

        private void RemovePossibilityFromSubGrid(int x, int y, int value) {
            x /= 3;
            y /= 3;
            for (int i = 0; i < 3; i++)
                for (int k = 0; k < 3; k++)
                    Possibles[x + i, y + k, value] = false;
        }

        private void RemovePossibility(int x, int y, int value) {
            RemovePossibilityFromColumn(x, value);
            RemovePossibilityFromRow(y, value);
            RemovePossibilityFromSubGrid(x, y, value);
        }

        private void Start(int diff) {
            diff = 80 - ((diff /10) * 8);
            Random rnd = new Random();
            int x = rnd.Next(9);
            int y = rnd.Next(9);
            int z;
            for (int i = 0; i < diff; i++) {
                while (Grid[x,y] != 0) {
                    x = rnd.Next(9);
                    y = rnd.Next(9);
                }
                List<int> h = new List<int>();
                for (int k = 0; k < 9; k++)
                    if (Possibles[x, y, k])
                        h.Add(k);

                z = h[rnd.Next(h.Count)];
                Grid[x, y] = z;
                RemovePossibility(x, y, z);
            }
        }

        //Public
        public int Get(int x, int y) {
            return Grid[x, y];
        }

        public void Set(int x, int y, int value) {
            Grid[x, y] = value;
            RemovePossibility(x, y, value);
        }

        public SudukoBoard() {
            Grid = new int[9, 9];
            Possibles = new bool[9, 9, 9];
            for (int i = 0; i < 9; i++) {
                for (int j = 0; j < 9; j++) {
                    Grid[i, j] = 0;
                    for (int k = 0; k< 9; k++) {
                        Possibles[i, j, k] = true;
                    } 
                }
            }
        }

        public void Start() {
            Start(50);
        }

    }//Board

    class Program{

        static void Main(string[] args){
            SudukoBoard board = new SudukoBoard();
            board.Start();
            Console.ReadKey();
        }//Main
    }
}
