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
        public static int licznikMozliwosci = 0;

        static void Main(string[] args)
        {
            // true - ruch gracza    X
            // false - ruch minimaxa O
            bool ruch = true;
            char[] plansza = { 
                ' ', ' ', ' ', 
                ' ', ' ', ' ', 
                ' ', ' ', ' ' };

            while (true)
            {
                Console.Clear();
                PokazPlansze(plansza);
                ZrobRuch(plansza, ruch);
                if (CzyGraSieSkonczyla(plansza, ruch))
                    return;
                ruch = !ruch;
            }
        }

        static bool CzyGraSieSkonczyla(char[] _plansza, bool _ruch)
        {
            bool czyRemis = KoniecGry(_plansza);
            bool czyWygrana = CzyWygrana(_plansza, _ruch);
            if (czyWygrana || czyRemis)
            {
                Console.Clear();
                if(czyWygrana)
                    Console.WriteLine($"Koniec gry wygrało {(_ruch ? 'X' : 'O')}!");
                else
                    Console.WriteLine($"Koniec gry remis!");
                PokazPlansze(_plansza);
                Console.ReadKey();
                return true;
            }

            return false;
        }

        static void PokazPlansze(char[] _plansza)
        {
            // Byla tu pentla ale po prostu tak jest czytelniej i latwiej do edycji PepoLove <3
            Console.WriteLine($"Obliczono {licznikMozliwosci} mozliwosci!");
            Console.WriteLine("\n");
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
            Console.Write("Wprowadz pozycje: ");
        }

        static void ZrobRuch(char[] _plansza, bool _ruch)
        {
            if (_ruch)
            {
                int pozycja;
                while (!int.TryParse(Console.ReadLine(), out pozycja) || pozycja < 0 || pozycja > 9 || _plansza[pozycja - 1] != ' ')
                    Console.WriteLine("Podaj cos w granicach od 1 do 9 ktore nie jest zajete: ");
                _plansza[pozycja - 1] = 'X';
            }               
            else
            {
                licznikMozliwosci = 0;
                int pozycja = NajlepszyRuch(_plansza);
                _plansza[pozycja] = 'O';
            }
        }

        static bool CzyWygrana(char[] _plansza, bool ruch)
        { 
            if (SprawdzenieWygranejDanegoZnaku(_plansza, ruch ? 'X' : 'O'))
                return true;
            else
                return false;
        }

        static int MiniMax(char[] _plansza, bool maksymalizuj, int dalekosc)
        {
            char[] plansza = (char[])_plansza.Clone();

            licznikMozliwosci++;

            if (SprawdzenieWygranejDanegoZnaku(plansza, ZnakMiniMaxa))
                return 10;
            else if (SprawdzenieWygranejDanegoZnaku(plansza, ZnakGracza))
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

        static bool SprawdzenieWygranejDanegoZnaku(char[] _plansza, char znak)
        {
            char o = znak;
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
                if (plansza[i] == ' ')
                {
                    int zwrotna = MiniMax(ZrobRuchWMiejscu(plansza, false, i), false, 0);
                    if (zwrotna > maks)
                    {
                        maks = zwrotna;
                        miejsce = i;
                    }
                }
            return miejsce;
        }
    }
}