using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleships;
using FluentAssertions;
using Xunit;

namespace Battleships.Test
{
    public class GameTest
    {
        [Fact]
        public void TestPlay()
        {
            var ships = new[] { "3:2,3:5" };
            var guesses = new[] { "7:0", "3:3" };
            Game.Play(ships, guesses).Should().Be(0);
        }

        [Fact]
        public void TestPlay_SunkOneShip()
        {
            var ships = new[] { "3:2,3:5" };
            var guesses = new[] { "7:0", "3:3", "3:4", "3:5", "3:2" };
            Game.Play(ships, guesses).Should().Be(1);
        }

        [Fact]
        public void TestPlay_Sunk2Ship()
        {
            var ships = new[] { "3:2,3:5", "7:0,6:0"};
            var guesses = new[] { "7:0", "3:3", "3:4", "3:5", "3:2", "6:0" };
            Game.Play(ships, guesses).Should().Be(2);
        }

        [Fact]
        public void TestPlay_WithOneInvalidShip()
        {
            //Invalid ship
            var ships = new[] { "3:2,3:5", "3:6,4:7" };
            var guesses = new[] { "7:0", "3:3" };
            Game.Play(ships, guesses).Should().Be(0);
        }

        [Fact]
        public void TestPlay_ShipWithInvalidLength()
        {
            //Invalid Ship Length
            var ships = new[] { "3:2,3:6", "7:0,6:0" };
            var guesses = new[] { "7:0", "3:3", "3:4", "3:5", "3:2", "3:6", "6:0" };
            Game.Play(ships, guesses).Should().Be(1);
        }
    }
}
