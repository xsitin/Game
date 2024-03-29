﻿using Game.Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void NamesAreDiffrent()
        {
            for (var i = 0; i < 500; i++)
            {
                var n1 = Helper.GetName();
                var n2 = Helper.GetName();
                if (n1.Equals(n2))
                    Assert.Fail();
            }

            Assert.Pass();
        }
    }
}