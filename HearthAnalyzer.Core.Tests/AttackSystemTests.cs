﻿using System;
using HearthAnalyzer.Core.Cards.Weapons;
using HearthAnalyzer.Core.Heroes;

using HearthAnalyzer.Core;
using HearthAnalyzer.Core.Cards;
using HearthAnalyzer.Core.Cards.Minions;

using NUnit.Framework;

namespace HearthAnalyzer.Core.Tests
{
    [TestFixture]
    public class AttackSystemTests : BaseTestSuite
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
        public void BasicMinionsAttacking()
        {
            var yeti1 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var yeti2 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var raptor = HearthEntityFactory.CreateCard<BloodfenRaptor>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti1;
            GameEngine.GameState.CurrentPlayerPlayZone[1] = raptor;
            GameEngine.GameState.WaitingPlayerPlayZone[0] = yeti2;

            // 4/5 attacking into another 4/5 should yeild two 4/1s
            yeti1.Attack(yeti2);

            Assert.AreEqual(1, yeti1.CurrentHealth, "Verify Yeti_1 is at 1 health.");
            Assert.AreEqual(1, yeti2.CurrentHealth, "Verify Yeti_2 is at 1 health.");

            // Now, kill yeti 2 with raptor 1. This should kill both the yeti and the raptor
            raptor.Attack(yeti2);

            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(yeti2), "Verify Yeti_2 is dead");
            Assert.IsTrue(GameEngine.DeadCardsThisTurn.Contains(raptor), "Verify Raptor_1 is dead");
            Assert.AreEqual(-2, yeti2.CurrentHealth, "Verify Yeti_2 is at -2 health");
            Assert.AreEqual(-2, raptor.CurrentHealth, "Verify Raptor_2 is at -2 health");
        }

        [Test]
        public void BasicWeaponsAttacking()
        {
            var warAxe = HearthEntityFactory.CreateCard<FieryWarAxe>();
            player.Weapon = warAxe;
            warAxe.WeaponOwner = player;

            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();

            GameEngine.GameState.WaitingPlayerPlayZone[0] = yeti;

            player.Attack(yeti);
            Assert.AreEqual(2, yeti.CurrentHealth, "Verify Yeti_1 is at 2 health");
            Assert.AreEqual(26, player.Health, "Verify the player is now at 26 health");
            Assert.AreEqual(1, warAxe.Durability, "Verify the war axe is now at 1 durability");

            player.RemoveStatusEffects(PlayerStatusEffects.EXHAUSTED);

            player.Attack(yeti);
            Assert.AreEqual(-1, yeti.CurrentHealth, "Verify Yeti_1 is at -1 health");
            Assert.AreEqual(22, player.Health, "Verify the player is now at 22 health");
            Assert.AreEqual(0, warAxe.Durability, "Verify the war axe is now at 0 durability");

            Assert.IsNull(player.Weapon, "Verify that the player no longer has the weapon equipped");
            Assert.IsTrue(player.Graveyard.Contains(warAxe), "Verify that the war axe is now in the graveyard");
        }

        [Test]
        public void Gorehowl()
        {
            var yeti1 = HearthEntityFactory.CreateCard<ChillwindYeti>();
            var gorehowl = HearthEntityFactory.CreateCard<Gorehowl>();

            GameEngine.GameState.WaitingPlayerPlayZone[0] = yeti1;

            // Make a super 4/28 yeti
            yeti1.TakeBuff(0, 23);

            player.Weapon = gorehowl;
            gorehowl.WeaponOwner = player;

            int gorehowlAttack = 7;
            int yetiHealth = 28;
            int playerHealth = 30;

            for (int i = 0; i < 7; i++)
            {
                player.Attack(yeti1);

                yetiHealth -= gorehowlAttack;
                gorehowlAttack--;
                playerHealth -= yeti1.CurrentAttackPower;

                Assert.AreEqual(yetiHealth, yeti1.CurrentHealth, "Verify that the yeti's health is {0}", yetiHealth);
                Assert.AreEqual(gorehowlAttack, gorehowl.CurrentAttackPower, "Verify that gorehowl's current attack power is {0}", gorehowlAttack);
                Assert.AreEqual(playerHealth, player.Health, "Verify that the player is at {0} health", playerHealth);

                // Unexhaust the player
                player.RemoveStatusEffects(PlayerStatusEffects.EXHAUSTED);
            }

            Assert.IsNull(player.Weapon, "Verify that gorehowl broke");
            Assert.IsTrue(player.Graveyard.Contains(gorehowl));
        }

        [Test]
        public void MinionsAttackingHero()
        {
            var yeti1 = HearthEntityFactory.CreateCard<ChillwindYeti>();

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti1;

            yeti1.Attack(player);

            Assert.AreEqual(5, yeti1.CurrentHealth, "Verify yeti is at full health");
            Assert.AreEqual(26, player.Health, "Verify the player took damage");
        }

        /// <summary>
        /// Verify minions are exhausted after attacking
        /// </summary>
        [Test]
        public void MinionExhaustion()
        {
            var yeti = HearthEntityFactory.CreateCard<ChillwindYeti>();
            yeti.ApplyStatusEffects(MinionStatusEffects.CHARGE);

            GameEngine.GameState.CurrentPlayerPlayZone[0] = yeti;
            yeti.Attack(opponent);

            Assert.IsTrue(yeti.IsExhausted, "Verify the yeti is now exhausted");
            Assert.IsFalse(yeti.CanAttack, "Verify the yeti can't attack");

            yeti.ApplyStatusEffects(MinionStatusEffects.WINDFURY);
            Assert.IsFalse(yeti.IsExhausted, "Verify the minion is no longer exhausted");

            yeti.Attack(opponent);
            Assert.IsTrue(yeti.IsExhausted, "Verify the yeti is now exhausted again");
            Assert.IsFalse(yeti.CanAttack, "Verify the yeti can't attack");

            GameEngine.EndTurn();
            Assert.IsFalse(yeti.IsExhausted, "Verify the yeti is unexhausted after the turn ends");
        }

        /// <summary>
        /// Verify the player gets exhausted after attacking
        /// </summary>
        [Test]
        public void PlayerExhaustion()
        {
            var fieryWarAxe = HearthEntityFactory.CreateCard<FieryWarAxe>();
            fieryWarAxe.CurrentManaCost = 0;

            player.AddCardToHand(fieryWarAxe);
            player.PlayCard(fieryWarAxe, null);

            player.Attack(opponent);
            Assert.IsTrue(player.IsExhausted, "Verify the player is exhausted");
            Assert.IsFalse(player.CanAttack, "Verify the player can't attack");

            player.ApplyStatusEffects(PlayerStatusEffects.WINDFURY);
            Assert.IsFalse(player.IsExhausted, "Verify the player is unexhausted now");

            player.Attack(opponent);
            Assert.IsTrue(player.IsExhausted, "Verify the player is exhausted");
            Assert.IsFalse(player.CanAttack, "Verify the player can't attack");

            GameEngine.EndTurn();
            Assert.IsFalse(player.IsExhausted, "Verify the player is unexhausted after the turn ends");
        }

        /// <summary>
        /// Verify minions and players can't attack when they are frozen
        /// </summary>
        [Test]
        public void CantAttackWhenFrozen()
        {
            var fieryWarAxe = HearthEntityFactory.CreateCard<FieryWarAxe>();
            fieryWarAxe.CurrentManaCost = 0;

            var golem = HearthEntityFactory.CreateCard<ArcaneGolem>();
            GameEngine.GameState.CurrentPlayerPlayZone[0] = golem;

            player.AddCardToHand(fieryWarAxe);
            player.PlayCard(fieryWarAxe, null);

            // Freeze the player and the minion
            player.ApplyStatusEffects(PlayerStatusEffects.FROZEN);
            golem.ApplyStatusEffects(MinionStatusEffects.FROZEN);

            Assert.IsFalse(player.CanAttack, "Verify player can't attack");
            Assert.IsFalse(golem.CanAttack, "Verify golem can't attack");

            try
            {
                player.Attack(opponent);
                Assert.Fail("Player shouldn't be able to attack!");
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                golem.Attack(opponent);
                Assert.Fail("Player shouldn't be able to attack!");
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
