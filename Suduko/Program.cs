using System;
using System.IO;
using System.Collections.Generic;

namespace Suduko{

    class SudukoBoard{
        //Private
        private int[,] Grid;
        private int[,] Solution;
        private bool[,,] Allowed;

        private bool IsAllowed (int x, int y, int value) {
            return Allowed[x, y, value];
        }

        private void RemovePossibilityFromColumn(int x, int value) {
            for (int i = 0; i < 9; i++)
                Allowed[x, i, value - 1] = false;
        }

        private void RemovePossibilityFromRow(int y, int value) {
            for (int i = 0; i < 9; i++)
                Allowed[i, y, value - 1] = false;
        }

        private void RemovePossibilityFromSubGrid(int x, int y, int value) {
            x = (x / 3) * 3;
            y = (y / 3) * 3;
            for (int i = 0; i < 3; i++)
                for (int k = 0; k < 3; k++)
                    Allowed[x + i, y + k, value - 1] = false;
        }

        private void RemovePossibility(int x, int y, int value) {
            RemovePossibilityFromColumn(x, value);
            RemovePossibilityFromRow(y, value);
            RemovePossibilityFromSubGrid(x, y, value);
        }

        //Public
        public int Get(int x, int y) {
            return Grid[x, y];
        }

        public void Set(int x, int y, int value) {
            Grid[x, y] = value;
            RemovePossibility(x, y, value);
        }

        public bool IsSolved() {
            return (Grid == Solution);
        }

        public SudukoBoard() {

        }

        public override string ToString() {
            string s = "";
            s += "|   ------------------------------------|\n" + 
                "|   | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |\n" +
                "|   ------------------------------------|\n";
            for (int i = 0; i < 9; i++) {
                s += "| " + i + " | ";
                for (int j = 0; j < 9; j++) {
                    s += Grid[i, j].ToString();
                    s += ((j + 1) % 3 == 0) ? " | " : "   ";
                }
                s += "\n";
                s += ((i + 1) % 3 == 0) ? "|   |-----------------------------------|\n" : "";
            }
            return s;
        }

    }//Board

    class Program{

        static bool Contains<T>(T[] input, T x) where T : IComparable{
            for (int i = 0; i < input.Length; i++) 
                if (input[i].CompareTo(x) == 0)
                    return true;
            return false;
        }

        static void Main(string[] args) {
            StreamReader source = File.OpenText("s2");
            string input = source.ReadToEnd();
            source.Close();
            char[] delims = {'a','b','c','d','e','f','g','h','i','j','k',
                'l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
                'P','Q','R','S','T','U','V','W','X','Y','Z',
                '\n','\t','\0',':',';',' '};
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string[] nums = input.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            int[] init = new int[162];
            for (int i = 0, j = 0; i < input.Length; i++) {
                if (Contains(digits, input[i])) { 
                    init[j] = Convert.ToInt32(input[i]);
                    j++;
                }
            }

            foreach (int i in init) {
                Console.WriteLine(i);
            }

            SudukoBoard board = new SudukoBoard();
            Console.ReadKey();
        }//Main
    }
}
