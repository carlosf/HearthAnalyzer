using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HearthAnalyzer.Core.Cards;
using HearthAnalyzer.Core.Cards.Weapons;
using HearthAnalyzer.Core.Deathrattles;
using NUnit.Framework;

using HearthAnalyzer.Core.Cards.Minions;
using HearthAnalyzer.Core.Heroes;

namespace HearthAnalyzer.Core.Tests
{
    [TestFixture]
    public class DeathrattleTests : BaseTestSuite
    {
        private BasePlayer player;
        private BasePlayer opponent;

        [SetUp]
        public void Setup()
        {
            player = HearthEntityFactory.CreatePlayer<Warlock>();
            opponent = HearthEntityFactory.CreatePlayer<Warlock>();

            GameEngine.Initialize(player, opponent, null, 0, player);
        }

        [TearDown]
        public void Cleanup()
        {
            GameEngine.Uninitialize();
        }

        [Test]
        public void DeathrattleDamageAllMinions()
        {
            var yeti1 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var yeti2 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var yeti3 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var yeti4 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var abom1 = HearthEntityFactory.CreateCard<Abomination>();

            abom1.CurrentManaCost = 0;
            player.AddCardToHand(abom1);
            
            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti1;
            GameEngine.GameState.CurrentPlayerPlayZone[1] = yeti2;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = yeti3;
            GameEngine.GameState.WaitingPlayerPlayZone[1] = yeti4;

            player.PlayCard(abom1, null);

            GameEngine.EndTurn();

            // This should kill the abomination
            // The yeti should also die from the deathrattle
            yeti3.Attack(abom1);

            // The rest of the yetis should have taken 2 damage from the abom deathrattle
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(abom1));
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(yeti3));
            Assert.AreEqual(3, yeti1.CurrentHealth, "Verify that the other yetis are hurt from the deathrattle");
            Assert.AreEqual(3, yeti2.CurrentHealth, "Verify that the other yetis are hurt from the deathrattle");
            Assert.AreEqual(3, yeti4.CurrentHealth, "Verify that the other yetis are hurt from the deathrattle");
        }

        /// <summary>
        /// Verify a random friendly minion was returned to the hand
        /// </summary>
        [Test]
        public void DeathRattleReturnFriendlyMinion()
        {
            var ambusher = HearthEntityFactory.CreateCard<AnubarAmbusher>();
            ambusher.Owner = player;
            ambusher.CurrentManaCost = 0;

            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var faerie = HearthEntityFactory.CreateCard<FaerieDragon>();
            var giant = HearthEntityFactory.CreateCard<SeaGiant>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti;
            GameEngine.GameState.CurrentPlayerPlayZone[1] = faerie;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = giant;

            player.AddCardToHand(ambusher);

            player.PlayCard(ambusher, null);

            GameEngine.EndTurn();
            giant.Attack(ambusher);

            // Ambusher should die and return a random friendly minion back to the owner's hand
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(ambusher), "Verify ambusher died");
            Assert.IsTrue(player.Hand.Contains(yeti) || player.Hand.Contains(faerie), "Verify minion returned to hand");
        }

        /// <summary>
        /// Verfiies cards are drawn
        /// </summary>
        [Test]
        public void DeathrattleDrawCard()
        {
            var thalnos = HearthEntityFactory.CreateCard<BloodmageThalnos>();
            thalnos.CurrentManaCost = 0;

            player.AddCardToHand(thalnos);
            player.PlayCard(thalnos, null);

            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            GameEngine.GameState.WaitingPlayerPlayZone[0] = yeti;

            GameEngine.EndTurn();

            yeti.Attack(thalnos);

            Assert.AreEqual(29, player.Health, "Verify player drew one fatigue card");
        }

        /// <summary>
        /// Verifies summoning a minion from no card source
        /// </summary>
        [Test]
        public void DeathrattleSummonMinionNoCardSource()
        {
            var cairne = HearthEntityFactory.CreateCard<CairneBloodhoof>();
            cairne.CurrentManaCost = 0;

            player.AddCardToHand(cairne);
            player.PlayCard(cairne, null);

            var giant = HearthEntityFactory.CreateCard<SeaGiant>();
            GameEngine.GameState.WaitingPlayerPlayZone[0] = giant;

            GameEngine.EndTurn();

            giant.Attack(cairne);
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(cairne), "Verify cairne died");
            Assert.IsTrue(GameEngine.GameState.CurrentPlayerPlayZone.Any(card => card is BaineBloodhoof), "Verify Baine got summoned");
        }

        /// <summary>
        /// Verifies summoning a minion from a card source
        /// </summary>
        [Test]
        public void DeathrattleSummonMinionFromCardSource()
        {
            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            yeti.Owner = player;

            var rag = HearthEntityFactory.CreateCard<RagnarostheFirelord>();
            var giant = HearthEntityFactory.CreateCard<SeaGiant>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = giant;

            GameEngine.RegisterDeathrattle(yeti, new DeathrattleSummonMinion<RagnarostheFirelord>(yeti.Owner, player.hand));

            player.AddCardToHand(rag);

            GameEngine.EndTurn();

            giant.Attack(yeti);
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(yeti), "Verify yeti died");
            Assert.IsTrue(GameEngine.GameState.CurrentPlayerPlayZone.Contains(rag), "Verify rag got summoned");
            Assert.IsFalse(player.Hand.Contains(rag), "Verify rag is no longer in player's hand");
        }

        /// <summary>
        /// Verifies heals a target
        /// </summary>
        [Test]
        public void DeathrattleHealTarget()
        {
            var chow = HearthEntityFactory.CreateCard<ZombieChow>();
            chow.CurrentManaCost = 0;

            var giant = HearthEntityFactory.CreateCard<SeaGiant>();
            GameEngine.GameState.WaitingPlayerPlayZone[0] = giant;

            player.AddCardToHand(chow);
            player.PlayCard(chow, null);

            GameEngine.EndTurn();

            int startingHealth = 5;
            opponent.Health = startingHealth;

            giant.Attack(chow);

            Assert.AreEqual(startingHealth + ZombieChow.DEATHRATTLE_POWER, opponent.Health, "Verify opponent got healed");
        }

        /// <summary>
        /// Verifies heal does damage with auchenai
        /// </summary>
        [Test]
        public void DeathrattleHealTargetWithAuchenai()
        {
            var chow = HearthEntityFactory.CreateCard<ZombieChow>();
            chow.CurrentManaCost = 0;

            var auchenai = HearthEntityFactory.CreateCard<AuchenaiSoulpriest>();
            auchenai.CurrentManaCost = 0;

            var giant = HearthEntityFactory.CreateCard<SeaGiant>();
            GameEngine.GameState.WaitingPlayerPlayZone[0] = giant;

            player.AddCardToHand(chow);
            player.PlayCard(chow, null);

            player.AddCardToHand(auchenai);
            player.PlayCard(auchenai, null);

            GameEngine.EndTurn();

            int startingHealth = 10;
            opponent.Health = startingHealth;

            giant.Attack(chow);

            Assert.AreEqual(startingHealth - ZombieChow.DEATHRATTLE_POWER, opponent.Health, "Verify opponent got damaged");
        }

        /// <summary>
        /// Verify that if Death's Bite (weapon) dies or gets replaced, it triggers its death rattle
        /// </summary>
        [Test]
        public void DeathsBite()
        {
            var deathsbite = HearthEntityFactory.CreateCard<DeathsBite>();
            deathsbite.CurrentManaCost = 0;

            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti;

            player.AddCardToHand(deathsbite);
            player.ApplyStatusEffects(PlayerStatusEffects.WINDFURY);

            player.PlayCard(deathsbite, null);

            deathsbite.Attack(opponent);
            deathsbite.Attack(opponent);

            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(deathsbite), "Verify the weapon broke");
            Assert.AreEqual(yeti.MaxHealth - 1, yeti.CurrentHealth, "Verify yeti took damage due to deathrattle");

            // Now, if Death's Bite was replaced, it should also trigger the deathrattle
            deathsbite = HearthEntityFactory.CreateCard<DeathsBite>();
            deathsbite.CurrentManaCost = 0;

            player.AddCardToHand(deathsbite);
            player.PlayCard(deathsbite, null);

            var weaponsmith = HearthEntityFactory.CreateCard<ArathiWeaponsmith>();
            weaponsmith.CurrentManaCost = 0;
            weaponsmith.Owner = player;

            player.AddCardToHand(weaponsmith);
            player.PlayCard(weaponsmith, null);

            // death rattle should trigger, damaging the yeti and the weaponsmith
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(deathsbite), "Verify the weapon broke");
            Assert.AreEqual(yeti.MaxHealth - 2, yeti.CurrentHealth, "Verify yeti got hit again");
            Assert.AreEqual(weaponsmith.MaxHealth - 1, weaponsmith.CurrentHealth, "Verify weaponsmith got hit");
        }
    }
}
