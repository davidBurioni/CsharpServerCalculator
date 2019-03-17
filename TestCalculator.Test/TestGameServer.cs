using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestCalculator.Test
{
    class TestGameServer
    {
        private FakeTransport transport;
        private GameServer server;

        [SetUp]
        public void SetupTests()
        {
            transport = new FakeTransport();
            server = new GameServer(transport);
        }

        //Check  Command Amount 
        [Test]
        public void GoodTestNumbersAmount()
        {
            Packet numbers = new Packet(0, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalAmount = transport.ClientDequeue();
            float fAmount = BitConverter.ToSingle(finalAmount.data, 1);
            Assert.That(fAmount, Is.EqualTo(10.0f));
        }
        [Test]
        public void BadTestNumbersAmount()
        {
            Packet numbers = new Packet(0, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalAmount = transport.ClientDequeue();
            float fAmount = BitConverter.ToSingle(finalAmount.data, 1);
            Assert.That(fAmount, Is.Not.EqualTo(20.0f));
        }
        //Check  Command Sub 
        [Test]
        public void GoodTestNumbersSub()
        {
            Packet numbers = new Packet(3, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalSub = transport.ClientDequeue();
            float fSub = BitConverter.ToSingle(finalSub.data, 1);
            Assert.That(fSub, Is.EqualTo(4.0f));
        }
        [Test]
        public void BadTestNumbersSub()
        {
            Packet numbers = new Packet(3, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalSub = transport.ClientDequeue();
            float fSub = BitConverter.ToSingle(finalSub.data, 1);
            Assert.That(fSub, Is.Not.EqualTo(2.0f));
        }
        //Check  Command Multiplication 
        [Test]
        public void GoodTestNumbersMultiplication()
        {
            Packet numbers = new Packet(6, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalMultiplication = transport.ClientDequeue();
            float fMultiplication = BitConverter.ToSingle(finalMultiplication.data, 1);
            Assert.That(fMultiplication, Is.EqualTo(21.0f));
        }

        [Test]
        public void BadTestNumbersMultiplication()
        {
            Packet numbers = new Packet(3, 7.0f, 3.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalMultiplication = transport.ClientDequeue();
            float fMultiplication = BitConverter.ToSingle(finalMultiplication.data, 1);
            Assert.That(fMultiplication, Is.Not.EqualTo(15.0f));
        }
        //Check  Command Division 
        [Test]
        public void GoodTestNumbersDivision()
        {
            Packet numbers = new Packet(9, 81.0f, 9.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalDivision = transport.ClientDequeue();
            float fDivision = BitConverter.ToSingle(finalDivision.data, 1);
            Assert.That(fDivision, Is.EqualTo(9.0f));
        }
        [Test]
        public void BadTestNumbersDivision()
        {
            Packet numbers = new Packet(9, 81.0f, 9.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalDivision = transport.ClientDequeue();
            float fDivision = BitConverter.ToSingle(finalDivision.data, 1);
            Assert.That(fDivision, Is.Not.EqualTo(5));
        }
        [Test]
        public void EvilTestNumbersDivisionForZero()
        {
            Packet numbers = new Packet(9, 81.0f, 0.0f);
            transport.ClientEnqueue(numbers, "numbers", 0);
            server.SingleStep();

            FakeData finalDivision = transport.ClientDequeue();
            float fDivision = BitConverter.ToSingle(finalDivision.data, 1);
            Assert.That(() => server.SingleStep(), Throws.Exception);
        }
    }
}
