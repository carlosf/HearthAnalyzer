﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthAnalyzer.Core.Cards
{
    /// <summary>
    /// Represents a deck of cards
    /// </summary>
    public class Deck
    {
        internal int topDeckIndex;
        internal int fatigueDamage;

        public List<BaseCard> Cards;

        /// <summary>
        /// Initialize a new empty deck
        /// </summary>
        public Deck()
        {
            this.Cards = new List<BaseCard>();
            this.topDeckIndex = -1;
            this.fatigueDamage = 0;
        }
        
        /// <summary>
        /// Initialize a deck with a list of cards
        /// </summary>
        /// <param name="cards">The cards to fill the deck with</param>
        public Deck(List<BaseCard> cards)
        {
            this.Cards = cards;
            this.topDeckIndex = this.Cards.Count - 1;
            this.fatigueDamage = 0;
        }

        public BaseCard DrawCard()
        {
            if (topDeckIndex < 0)
            {
                fatigueDamage++;
                return new FatigueCard(fatigueDamage);
            }

            BaseCard card = this.Cards[topDeckIndex];
            this.Cards.RemoveAt(topDeckIndex);
            this.topDeckIndex--;
            return card;
        }

        /// <summary>
        /// Adds a card to the current deck
        /// </summary>
        /// <param name="card">The card to add</param>
        /// <remarks>Adds to the end of the deck by default</remarks>
        public void AddCard(BaseCard card)
        {
            this.Cards.Add(card);
            this.topDeckIndex++;
        }

        /// <summary>
        /// Adds a list of cards to the current deck
        /// </summary>
        /// <param name="cards">The list of cards to add</param>
        /// <remarks>Adds to the end of the deck by default</remarks>
        public void AddCards(List<BaseCard> cards)
        {
            this.Cards.AddRange(cards);
            this.topDeckIndex = this.Cards.Count - 1;
        }

        /// <summary>
        /// Draws n cards
        /// </summary>
        /// <param name="n">The number of cards to draw</param>
        /// <returns>The list of cards drawn.</returns>
        public List<BaseCard> DrawCards(int n = 1)
        {
            List<BaseCard> cards = new List<BaseCard>();
            for (int i = 0; i < n; i++)
            {
                cards.Add(DrawCard());
            }

            return cards;
        }

        /// <summary>
        /// Shuffle the deck
        /// </summary>
        /// <param name="n">The number of times to shuffle the deck</param>
        public void Shuffle(int n = 5)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < this.Cards.Count; j++)
                {
                    var swapIndex = GameEngine.Random.Next(this.Cards.Count);
                    BaseCard temp = this.Cards[swapIndex];
                    this.Cards[swapIndex] = this.Cards[j];
                    this.Cards[j] = temp;
                }
            }
        }
    }
}
