using System;
using System.Collections.Generic;
using System.Linq;

namespace FiveSuitSame
{
    internal class Program
    {
        public static int PlayerCount { get; set; }

        public static List<Player> Players { get; set; }

        public static List<Card> Deck { get; set; }

        public static bool PlayerWon { get; set; } = false;

        public static Card DiscardedCard { get; set; }

        public static List<string> CardNumbers = new List<string>
        {
            "Ace",
            "Two",
           "Three",
            "Four",
            "Five",
            "Six",
            "Seven",
            "Eight",
            "Nine",
            "Ten",
            "Jack",
            "Queen",
            "King"
        };

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            Console.WriteLine("Rules :  Players Min = 2, Max = 4 \nPlayers should be dealt 5 cards each. \nFirst to have 5 cards of same suit wins. ");
            Console.WriteLine("\nEnjoy!");

            GenerateDeckCards();
            Shuffle();
           

            Console.WriteLine("\nEnter the number of players(2-4):");
            var userCount = Convert.ToInt32(Console.ReadLine());

            if (userCount > 4)
                userCount = 4;
            if (userCount < 2)
                userCount = 2;

            AddPlayers(userCount);
            DealCards();

            Console.WriteLine("\nLets begin.");

            RotatePlayerTurn();


        }

        public static void RotatePlayerTurn()
        {
            // Repeat while no player has won
            while (!PlayerWon)
            {
                foreach (var player in Players)
                {
                    PlayerTurn(player);
                }
            }
        }

        public static void PlayerTurn(Player player, string messageError = null)
        {
            if (messageError != null)
            {
                Console.WriteLine(messageError);
            }

            Console.WriteLine($"\n{player.Name}'s turn");

            ShowPlayerCard(player.Cards);
            var userInput = string.Empty;

            if (DiscardedCard != null)
            {
                Console.WriteLine($"\nDiscard Card ({DiscardedCard.Name})");
                Console.WriteLine($"\nDo you want to Draw (Press D) or Take players discarded card (Press T)?");
                userInput = Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"\nDo you want to Draw (Press D)?");
                userInput = Console.ReadLine();
            }

            var optionText = string.Empty;
            if (userInput.Contains("d") || userInput.Contains("D"))
            {
                optionText = "Which card do you want to Draw?";
            }
            else if (userInput.Contains("t") || userInput.Contains("T"))
            {
                optionText = "Which card do you want to replace it with?";
            }
            else
            {
                PlayerTurn(player, "\nInvalid Input. Lets try again.");
            }

            Console.WriteLine(optionText);
            var userInput2 = Convert.ToInt32(Console.ReadLine());

            if (userInput.Contains("t") || userInput.Contains("T"))
            {
                var discarded = new Card(DiscardedCard);
                var playerCard = new Card(player.Cards[userInput2 - 1]);
                player.Cards[userInput2 - 1] = discarded;
                DiscardedCard = playerCard;
            }
            else
            {
                DiscardedCard = new Card(player.Cards[userInput2 - 1]);
                player.Cards[userInput2 - 1] = Deck[0];
                Deck.RemoveAt(0);
            }

            ShowPlayerCard(player.Cards);

            CheckSuitForWin(player);

            Console.WriteLine($"\nEnd of {player.Name}'s turn");
        }

        public static void ShowPlayerCard(List<Card> cards)
        {
            var num = 1;
            foreach (var card in cards)
            {
                Console.WriteLine($"\n{num}. {card.Name}");
                num++;
            }
        }

        public static void AddPlayers(int count)
        {
            Players = new List<Player>();
            for (var i = 1; i <= count; i++)
            {
                Console.WriteLine($"\nPlease enter Player {i} Name.");
                var name = Console.ReadLine();

                Players.Add(new Player { Name = name });
            }

            Console.WriteLine($"\nPlayers {string.Join(", ", Players.Select(c => c.Name))} have been added successfully.");
        }

        public static void CheckSuitForWin(Player player)
        {
            if (player.Cards.Select(c => c.Suit).Distinct().Count() == 1)
            {
                Console.WriteLine($"\nCongratulations {player.Name}, You won!!!");

                PlayerWon = true;

                Environment.Exit(0);
            }
        }

        public static void GenerateDeckCards()
        {
            Deck = new List<Card>();

            foreach (var cardNumber in CardNumbers)
            {
                //var suits = Enum.GetNames(typeof(Suit)).ToList();
                var suits = (Suit[])Enum.GetValues(typeof(Suit));
                
                foreach (var suit in suits)
                {
                    Deck.Add(new Card
                    {
                        Name = $"{cardNumber} of {suit.ToString()}",
                        Suit = suit
                    });
                }
            }
        }

        public static void DealCards()
        {
            foreach (var player in Players)
            {
                player.Cards = new List<Card>(Deck.Take(5));
                Deck.RemoveRange(0, 5);
            }
        }

        public static void Shuffle()
        {
            if (Deck != null && Deck.Any())
            {
                var temp = new List<Card>();
                var random = new Random();

                for (int i = 0; i < 40; i++)
                {
                    var length = Deck.Count;
                    var n = random.Next(0, length - 1);
                    temp.Add(Deck[n]);
                    Deck.RemoveAt(n);
                }

                Deck = new List<Card>(temp);
            }
        }
    }

    public class Player
    {
        public string Name { get; set; }

        public List<Card> Cards { get; set; }
    }

    public class Card
    {
        public Card()
        {
        }

        public Card(Card card)
        {
            Name = card.Name;
            Suit = card.Suit;
        }

        public string Name { get; set; }

        public Suit Suit { get; set; }
    }

    public enum Suit
    {
        Clubs, Diamonds, Hearts, Spades
    }
}