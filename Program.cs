using System;
using System.Collections.Generic;
using System.Linq;

namespace kolkoikrzyzkyMINIMAX
{
    /*
     * TODO:
     * Uladnic kod (aktualnie jest to spageti XD)
     * Zmiejszyc kod
     * Dodac wybor gracza (X czy O)
     * Poprawic delikatnie chodzi o to ze czlowiek nie wygra ale pc z pc wygra zawsze 2 osoba wiec biblethump bo powinen byc remis
     */

    class Program
    {
        public static readonly char ZnakMiniMaxa = 'O';
        public static readonly char ZnakGracza = 'X';

        static void Main(string[] args)
        {
            // true - ruch gracza    X
            // false - ruch minimaxa O
            bool ruch = true;
            char[] plansza = new char[9];
            // Do testowanie ukladow trzeba usunac jeszcze pentle for ponizej <3
            //char[] plansza = { 'X', 'O', ' ', ' ', 'O', ' ', ' ', ' ', 'X' };

            for (int i = 0; i < plansza.Length; i++)
                plansza[i] = ' ';

            while (true)
            {
                Console.Clear();
                PokazPlansze(plansza);
                ZrobRuch(plansza, ruch);
                if (CzyWygrana(plansza, ruch))
                {
                    Console.Clear();
                    Console.WriteLine($"Koniec gry wygrało {(ruch ? 'X' : 'O')}!");
                    PokazPlansze(plansza);
                    Console.ReadKey();
                    return;
                }
                if(KoniecGry(plansza))
                {
                    Console.Clear();
                    Console.WriteLine($"Koniec gry remis!");
                    PokazPlansze(plansza);
                    Console.ReadKey();
                    return;
                }
                ruch = !ruch;
            }
        }

        static void PokazPlansze(char[] _plansza)
        {
            /*
            for (int i = 0; i < _plansza.Length; i++)
            {
                if (i % 3 == 0 && i != 0)
                    Console.WriteLine();
                Console.Write($"{_plansza[i]} ");
            }
            */

            Console.WriteLine($" {_plansza[0]} | {_plansza[1]} | {_plansza[2]}");
            Console.WriteLine(" --+---+--");
            Console.WriteLine($" {_plansza[3]} | {_plansza[4]} | {_plansza[5]}");
            Console.WriteLine(" --+---+--");
            Console.WriteLine($" {_plansza[6]} | {_plansza[7]} | {_plansza[8]}");
            Console.WriteLine("\n");
            Console.WriteLine(" 1 | 2 | 3");
            Console.WriteLine(" --+---+--");
            Console.WriteLine(" 4 | 5 | 6");
            Console.WriteLine(" --+---+--");
            Console.WriteLine(" 7 | 8 | 9");
        }

        static void ZrobRuch(char[] _plansza, bool _ruch)
        {
            if (_ruch)
            {
                int pozycja = int.Parse(Console.ReadLine()) - 1;
                while (pozycja < 0 || pozycja > 9 || _plansza[pozycja] != ' ')
                {
                    Console.WriteLine("Podaj cos w granicach od 1 do 9 ktore nie jest zajete: ");
                    pozycja = int.Parse(Console.ReadLine());
                }
                _plansza[pozycja] = 'X';
            }               
            else
            {
                int pozycja = NajlepszyRuch(_plansza);
                _plansza[pozycja] = 'O';
            }
        }

        static bool CzyWygrana(char[] _plansza, bool ruch)
        { 
            char o = ruch ? 'X' : 'O';
            if (//Pozime
                o == _plansza[0] && o == _plansza[1] && o == _plansza[2] ||
                o == _plansza[3] && o == _plansza[4] && o == _plansza[5] ||
                o == _plansza[6] && o == _plansza[7] && o == _plansza[8] ||
                //Na skos
                o == _plansza[0] && o == _plansza[4] && o == _plansza[8] ||
                o == _plansza[6] && o == _plansza[4] && o == _plansza[2] ||
                //Pionowe
                o == _plansza[0] && o == _plansza[3] && o == _plansza[6] ||
                o == _plansza[1] && o == _plansza[4] && o == _plansza[7] ||
                o == _plansza[2] && o == _plansza[5] && o == _plansza[8])
                return true;
            else
                return false;
        }

        static int MiniMax(char[] _plansza, bool maksymalizuj, int dalekosc)
        {
            char[] plansza = (char[])_plansza.Clone();

            if (SprawdzenieWygranejMiniMaxa(plansza))
                return 10;
            else if (SprawdzenieWygranejGracza(plansza))
                return -10;
            else if (KoniecGry(plansza))
                return 0;

            if(maksymalizuj)
            {
                int maks = int.MinValue;
                for (int i = 0; i < plansza.Length; i++)
                {
                    if (plansza[i] == ' ')
                    {
                        int zwrotna = MiniMax(ZrobRuchWMiejscu(plansza, false, i), false, dalekosc + 1);
                        maks = Math.Max(maks, zwrotna - dalekosc);
                    }
                }
                return maks;
            }
            else
            {
                int min = int.MaxValue;
                for (int i = 0; i < plansza.Length; i++)
                {
                    if (plansza[i] == ' ')
                    {
                        int zwrotna = MiniMax(ZrobRuchWMiejscu(plansza, true, i), true, dalekosc + 1);
                        min = Math.Min(min, zwrotna);
                    }
                }
                return min;
            }
        }

        static bool KoniecGry(char[] _plansza)
        {
            foreach (var item in _plansza)
                if (item == ' ')
                    return false;

            return true;
        }

        static bool SprawdzenieWygranejMiniMaxa(char[] _plansza)
        {
            char o = ZnakMiniMaxa;
            if (//Pozime
                o == _plansza[0] && o == _plansza[1] && o == _plansza[2] ||
                o == _plansza[3] && o == _plansza[4] && o == _plansza[5] ||
                o == _plansza[6] && o == _plansza[7] && o == _plansza[8] ||
                //Na skos
                o == _plansza[0] && o == _plansza[4] && o == _plansza[8] ||
                o == _plansza[6] && o == _plansza[4] && o == _plansza[2] ||
                //Pionowe
                o == _plansza[0] && o == _plansza[3] && o == _plansza[6] ||
                o == _plansza[1] && o == _plansza[4] && o == _plansza[7] ||
                o == _plansza[2] && o == _plansza[5] && o == _plansza[8])
                return true;
            else
                return false;
        }

        static bool SprawdzenieWygranejGracza(char[] _plansza)
        {
            char o = ZnakGracza;
            if (//Pozime
                o == _plansza[0] && o == _plansza[1] && o == _plansza[2] ||
                o == _plansza[3] && o == _plansza[4] && o == _plansza[5] ||
                o == _plansza[6] && o == _plansza[7] && o == _plansza[8] ||
                //Na skos
                o == _plansza[0] && o == _plansza[4] && o == _plansza[8] ||
                o == _plansza[6] && o == _plansza[4] && o == _plansza[2] ||
                //Pionowe
                o == _plansza[0] && o == _plansza[3] && o == _plansza[6] ||
                o == _plansza[1] && o == _plansza[4] && o == _plansza[7] ||
                o == _plansza[2] && o == _plansza[5] && o == _plansza[8])
                return true;
            else
                return false;
        }

        static char[] ZrobRuchWMiejscu(char[] _plansza, bool ruch, int miejsce)
        {
            char[] plansza = (char[])_plansza.Clone();
            plansza[miejsce] = ruch ? 'X' : 'O';
            return plansza;
        }

        static int NajlepszyRuch(char[] _plansza)
        {
            int maks = int.MinValue;
            int miejsce = int.MinValue;
            char[] plansza = (char[])_plansza.Clone();
            for (int i = 0; i < plansza.Length; i++)
            {
                if (plansza[i] == ' ')
                {
                    int zwrotna = MiniMax(ZrobRuchWMiejscu(plansza, false, i), false, 0);
                    if (zwrotna > maks)
                    {
                        maks = zwrotna;
                        miejsce = i;
                    }
                }
            }
            return miejsce;
        }
    }
}
