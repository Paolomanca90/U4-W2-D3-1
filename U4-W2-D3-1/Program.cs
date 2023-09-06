using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace U1_W2_D3_Esercitazione_ContoCorrente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Banca.Start();
        }
    }

    class ContoCorrente
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataAperturaConto { get; set; }
        public int NumeroConto { get; set; }
        public double Saldo { get; set; }

        public ContoCorrente() { }


        public ContoCorrente(string nome, string cognome, DateTime dataAperturaConto, int numeroConto)
        {
            Nome = nome;
            Cognome = cognome;
            DataAperturaConto = dataAperturaConto;
            NumeroConto = numeroConto;
            Saldo = 0;
        }

        public ContoCorrente(string nome, string cognome, DateTime dataAperturaConto, int numeroConto, double saldo)
        {
            Nome = nome;
            Cognome = cognome;
            DataAperturaConto = dataAperturaConto;
            NumeroConto = numeroConto;
            Saldo = saldo;
        }

        public static List<Movimentazioni> movimentazioniList = new List<Movimentazioni>();

        public void NuovaMovimentazione(double importo, string movimentazione)
        {
            if (movimentazione == "Versamento")
            {
                NuovoVersamento(importo);
                Console.WriteLine("Versamento effettuato con successo.");
            }
            else if (movimentazione == "Prelievo")
            {
                NuovoPrelievo(importo);
                Console.WriteLine("Prelievo effettuato con successo.");
            }
            else
            {
                Console.WriteLine("Scegliere un'opzione valida");
            }
        }

         void NuovoVersamento(double importo)
        {
            Saldo += importo;
            movimentazioniList.Add(new Movimentazioni(importo, "Versamento"));
        }

         void NuovoPrelievo(double importo)
        {
            if (Saldo >= importo)
            {
                Saldo -= importo;
                movimentazioniList.Add(new Movimentazioni(importo, "Prelievo"));
            }
            else
            {
                Console.WriteLine("Vai a farti un prestito straccione!!");
            }
        }

        public void ListaMovimentazioni()
        {
            Console.WriteLine("===== Estratto conto n. " + NumeroConto + " =====");
            foreach (Movimentazioni mov in movimentazioniList)
            {
                Console.WriteLine($"Movimentazione: {mov.Movimentazione} - importo: {mov.Importo:N}");
            }
            Console.WriteLine($"Saldo totale: {Saldo:N}");
        }


    }

    class Movimentazioni
    {
        public double Importo { get; set; }
        public string Movimentazione { get; set; }

        public Movimentazioni() { }
        public Movimentazioni(double importo, string movimentazione)
        {
            Importo = importo;
            Movimentazione = movimentazione;
        }
    }

    class Banca
    {

        static Random random = new Random();


        static List<ContoCorrente> contoCorrenteList = new List<ContoCorrente>()
        {
            new ContoCorrente("Paolo", "Manca", new DateTime(2023, 5, 21), GeneraNumConto(), 21000),
            new ContoCorrente("Sebastiano", "Castorina", new DateTime(2023, 4, 15), GeneraNumConto(), 20000),
            new ContoCorrente("Yanina", "Aguero", new DateTime(2022, 12, 15), GeneraNumConto(), 25000),
            new ContoCorrente("Gabriele", "Colombo", new DateTime(2022, 4, 21), GeneraNumConto(), 23000),
            new ContoCorrente("Simone", "Potenza", new DateTime(2022, 6, 10), GeneraNumConto(), 23000),
        };


        static int GeneraNumConto()
        {
            int i = random.Next(100000, 999999);
            return i;
        }

        static void NuovoContoCorrente()
        {
            Console.WriteLine("Inserisci il nome del correntista:");
            string nome = Console.ReadLine();
            Console.WriteLine("Inserisci il cognome del correntista:");
            string cognome = Console.ReadLine();
            DateTime dateTime = DateTime.Now;
            int numeroConto = GeneraNumConto();

            ContoCorrente newConto = new ContoCorrente(nome, cognome, dateTime, numeroConto);
            contoCorrenteList.Add(newConto);
            Console.WriteLine($"Operazione di creazione del c/c n. {numeroConto} completata!");
        }

        static void Lista()
        {
            Console.WriteLine("============ Lista C/c aperti ============");
            Console.WriteLine();
            foreach (ContoCorrente conto in contoCorrenteList)
            {
                Console.WriteLine($"C/c n. {conto.NumeroConto} creato in data {conto.DataAperturaConto.ToShortDateString()} - saldo: {conto.Saldo:N}");
            }
        }

        static void MostraMovimentazioni()
        {
            Console.Write("Inserisci il numero del conto corrente: ");
            int numeroConto = Int32.Parse(Console.ReadLine());
            ContoCorrente newConto = null;
            foreach (ContoCorrente conto in contoCorrenteList)
            {
                if (conto.NumeroConto == numeroConto)
                {
                    newConto = conto;
                    break;
                }
            }
            if (newConto != null)
            {
                newConto.ListaMovimentazioni();
            }
            else
            {
                Console.WriteLine("C/c non trovato nell'archivio.");
            }
        }

        static void NuovaOperazione()
        {
            Console.Write("Inserisci il numero del conto corrente: ");
            int numeroConto = Int32.Parse(Console.ReadLine());
            Console.Write("É un Prelievo o un Versamento? ");
            string movimentazione = Console.ReadLine();
            Console.Write("Inserisci l'importo: ");
            double importo = double.Parse(Console.ReadLine());

            ContoCorrente newConto = null;

            foreach (ContoCorrente conto in contoCorrenteList)
            {
                if (conto.NumeroConto == numeroConto)
                {
                    newConto = conto;
                    break;
                }
            }
            if(newConto != null)
            {
                newConto.NuovaMovimentazione(importo, movimentazione);
            }
            else
            {
                Console.WriteLine("C/c non trovato nell'archivio.");
            }
        }

        public static void Start()
        {
            Console.WriteLine("========= BANCA PUCCIO =========");
            Console.WriteLine("1. Apri un nuovo Conto Corrente");
            Console.WriteLine("2. Effettua un'operazione");
            Console.WriteLine("3. Esamina la lista delle movimentazioni del tuo conto");
            Console.WriteLine("4. Esamina lista Conti Correnti");
            Console.WriteLine("0. Esci");

            string scelta = Console.ReadLine();
            Console.WriteLine();
            if (scelta == "1")
            {
                NuovoContoCorrente();
                Console.WriteLine();
                Start();
            }
            else if (scelta == "2")
            {
                NuovaOperazione();
                Console.WriteLine();
                Start();
            }
            else if (scelta == "3")
            {
                MostraMovimentazioni();
                Console.WriteLine();
                Start();
            }
            else if (scelta == "4")
            {
                Lista();
                Console.WriteLine();
                Start();
            }
            else if (scelta == "0")
            {
                Console.WriteLine("Arrivedrci");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Scelta non valida. Riprovare.");
                Console.WriteLine();
                Start();
            }
            Console.ReadLine();
        }

    }
}