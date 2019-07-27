using System;
using System.IO;

namespace Suduko{

    class SudukoBoard{
        //Private
        private int[,] Grid;
        private int[,] Solution;
        private bool[,,] Allowed;

        private void RemovePossibilityFromColumn(int x, int value) {
            for (int y = 0; y < 9; y++)
                Allowed[x, y, value - 1] = false;
        }

        private void RemovePossibilityFromRow(int y, int value) {
            for (int x = 0; x < 9; x++)
                Allowed[x, y, value - 1] = false;
        }

        private void RemovePossibilityFromSubGrid(int x, int y, int value) {
            x = (x / 3) * 3;
            y = (y / 3) * 3;
            for (int i = 0; i < 3; i++)
                for (int k = 0; k < 3; k++)
                    Allowed[x + k, y + i, value - 1] = false;
        }

        private void RemovePossibility(int x, int y, int value) {
            RemovePossibilityFromColumn(x, value);
            RemovePossibilityFromRow(y, value);
            RemovePossibilityFromSubGrid(x, y, value);
        }

        private bool IsHiddenSingleInColumn(int x, int y, int value) {
            for (int i = 0; i < 9; i++)
                if (Allowed[x, y, value - 1] && i != x) return false;
            return true;
        }

        private bool IsHiddenSingleInRow(int x, int y, int value) {
            for (int i = 0; i < 9; i++)
                if (Allowed[x, y, value - 1] && i != x) return false;
            return true;
        }

        private bool IsHiddenSingleInSubGrid(int x, int y, int value) {
            x = (x / 3) * 3;
            y = (y / 3) * 3;
            for (int i = 0; i < 3; i++)
                for (int k = 0; k < 3; k++)
                    if (Allowed[x + k, y + i, value - 1] && i != y && k != x) return false;
            return true;
        }

        private bool IsHiddenSingle(int x, int y, int value) {
            return IsHiddenSingleInColumn(x, y, value) ||
                IsHiddenSingleInRow(x, y, value) ||
                IsHiddenSingleInSubGrid(x, y, value);
        }

        private bool IsSingle(int x, int y, int value) {
            for (int z = 0; z < 9; z++)
                if (Allowed[x, y, z] && z + 1 != value) return false;
            return true;
        }

        private (int, int, int) GetSingle() {
            int x = 0, y = 0, z = 0;

            return (x, y, z);
        }

        //Public
        public bool IsAllowed(int x, int y, int value) {
            return Allowed[x, y, (value - 1)];
        }

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
            Grid = new int[9, 9];
            Allowed = new bool[9, 9, 9];
            for (int y = 9; y < 9; y++)
                for (int x = 0; x < 9; x++) {
                    Grid[x, y] = 0;
                    for (int z = 0; z < 9; z++)
                        Allowed[x,y,z] = true;
                }
        }

        public SudukoBoard(int[] init) {
            Grid = new int[9,9];
            Solution = new int[9,9];
            Allowed = new bool[9, 9, 9];

            for (int y = 0, j = 0, k = 81; y < 9; y++)
                for (int x = 0 ; x < 9; x++, j++, k++) {
                    Solution[x, y] = init[k];
                    for (int z = 0; z < 9; z++)
                        Allowed[x, y, z] = true;
                }

            for (int y = 0, j = 0; y < 9; y++)
                for (int x = 0; x < 9; x++, j++) {
                    Grid[x, y] = init[j];
                    if (init[j] != 0)
                        RemovePossibility(x, y, init[j]);
                }
        }

        public string PossibilitiesToString() {
            string s = "";
            s += "    +-----------+-----------+-----------+\n" +
                "    | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |\n" +
                "    +===========+===========+===========+\n";               
            for (int y = 0; y < 9; y++) {
                s += "| " + y + " |";
                for (int zi = 0; zi < 3; zi++) {
                    s += zi == 0 ? "" : "    |";
                    for (int x = 0; x < 9; x++){
                        for (int z = 0; z < 3; z++)
                            s += Allowed[x, y, (z + (zi * 3))] ? (z + (zi * 3) + 1).ToString() : ".";
                        s += ((x + 1) % 3 == 0) ? "|" : "|";
                    }
                    s += "\n";
                }
                s += ((y + 1) % 3 == 0) ? "    +===========+===========+===========+\n" : "    +-----------+-----------+-----------+\n";
            }
            s += "\n";
            return s;
        }


        public override string ToString() {
            string s = "";
            s += "    +-----------+-----------+-----------+\n" + 
                "    | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |\n" +
                "    +-----------+-----------+-----------+\n";
            for (int y = 0; y < 9; y++) {
                s += "| " + y + " | ";
                for (int x = 0; x < 9; x++) {
                    s += Grid[x, y].ToString();
                    s += ((x + 1) % 3 == 0) ? " | " : "   ";
                }
                s += "\n";
                s += ((y + 1) % 3 == 0) ? "    +-----------+-----------+-----------+\n" : "";
            }
            return s;
        }



    }//Board

    class Program{

        static int[] GetInit(string path) {
            StreamReader sourcefile = File.OpenText(path);
            char[] delims = {'a','b','c','d','e','f','g','h','i','j','k',
                'l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
                'P','Q','R','S','T','U','V','W','X','Y','Z',
                '\n','\t','\r','\0','\'',':',';',' '};
            string[] input = sourcefile.ReadToEnd().Split(delims, StringSplitOptions.RemoveEmptyEntries);
            sourcefile.Close();

            int[] init = new int[162];
            for (int i = 0; i < input.Length; i++)
                init[i] = Convert.ToInt32(input[i]);
            return init;
        }

        static void Print(SudukoBoard board){
            Console.Clear();
            Console.WriteLine("Possible Values: \n");
            Console.WriteLine(board.PossibilitiesToString());
            Console.WriteLine("Current Board: \n");
            Console.WriteLine(board.ToString());
        }

        static (int, int, int) GetInput() {
            int x = -1, y = -1, value = -1;
            Console.WriteLine("Please enter x co-ordinate [0 - 8]");
            for (x = Convert.ToInt32(Console.ReadLine()); x < 0 || x > 8; x = Convert.ToInt32(Console.ReadLine())) 
                Console.WriteLine(x + " is an invalid x co-ordinate");
            Console.WriteLine("Please enter y co-ordinate [0 - 8]");
            for (y = Convert.ToInt32(Console.ReadLine()); y < 0 || y > 8; y = Convert.ToInt32(Console.ReadLine()))
                Console.WriteLine(y + " is an invalid y co-ordinate");
            Console.WriteLine("Please enter a value [1 - 9], [0] to forfiet");
            for (value = Convert.ToInt32(Console.ReadLine()); value < 0 || value > 9; value = Convert.ToInt32(Console.ReadLine()))
                Console.WriteLine("invalid value");

            return (x, y, value);
        }

        static void play(SudukoBoard board, int x, int y, int z) {
            if (board.IsAllowed(x, y, z)) {
                Console.WriteLine("Placing " + z + " at position [" + x + ", " + y + "]");
                board.Set(x, y, z);
            } else
                Console.WriteLine("Can't place " + z + " at position [" + x + " , " + y + "]");  
        }

        static void Main(string[] args) {

            int[] init;
            if (args.Length == 0)
                init = GetInit("s1");
            else
                init = GetInit(args[0]);

            SudukoBoard board = new SudukoBoard(init);

            int x, y, z; 
            while (!board.IsSolved()) {
                Print(board);

                (x,y,z) = GetInput();
                if (z == 0){
                    Console.WriteLine("EXIT");
                    break;
                }else
                    play(board, x, y, z);

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            Console.WriteLine(board.ToString());
            Console.ReadKey();
        }//Main
    }
}
