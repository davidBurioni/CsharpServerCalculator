using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TestCalculator
{
    public class GameServer
    {
        private delegate void GameCommand(byte[] data, EndPoint sender);
        private Dictionary<byte, GameCommand> commandsTable;

        private Dictionary<EndPoint, GameClient> clientsTable;

        IGameTransport transport;
        //Command Amount
        private void CalculateAmount(byte[] data, EndPoint sender)
        {
            GameClient newClient = new GameClient(this, sender);
            clientsTable[sender] = newClient;

            float firtNumb = BitConverter.ToSingle(data, 1);
            float secondNumb = BitConverter.ToSingle(data, 5);



            float amount = firtNumb + secondNumb;
            Packet numbersAmount = new Packet(0, amount);
            clientsTable[sender].Enqueue(numbersAmount);
        }
        //Command Sub
        private void CalculateSub(byte[] data, EndPoint sender)
        {
            GameClient newClient = new GameClient(this, sender);
            clientsTable[sender] = newClient;

            float firtNumb = BitConverter.ToSingle(data, 1);
            float secondNumb = BitConverter.ToSingle(data, 5);



            float sub = firtNumb - secondNumb;
            Packet numbersSub = new Packet(0, sub);
            clientsTable[sender].Enqueue(numbersSub);
        }
        //Command Multiplication
        private void CalculateMultiplication(byte[] data, EndPoint sender)
        {
            GameClient newClient = new GameClient(this, sender);
            clientsTable[sender] = newClient;

            float firtNumb = BitConverter.ToSingle(data, 1);
            float secondNumb = BitConverter.ToSingle(data, 5);



            float multiplication = firtNumb * secondNumb;
            Packet numbersMultiplication = new Packet(0, multiplication);
            clientsTable[sender].Enqueue(numbersMultiplication);
        }
        //Command Division
        private void CalculateDivision(byte[] data, EndPoint sender)
        {
            GameClient newClient = new GameClient(this, sender);
            clientsTable[sender] = newClient;

            float firtNumb = BitConverter.ToSingle(data, 1);
            float secondNumb = BitConverter.ToSingle(data, 5);
            float division;
            try
            {
                division = firtNumb / secondNumb;
            }
            catch
            {
                throw new Exception();
            }
            Packet numbersDivision = new Packet(0, division);
            clientsTable[sender].Enqueue(numbersDivision);
        }

        private void Ack(byte[] data, EndPoint sender)
        {
            if (!clientsTable.ContainsKey(sender))
            {
                return;
            }

            GameClient client = clientsTable[sender];
            uint packetId = BitConverter.ToUInt32(data, 1);
            client.Ack(packetId);
        }

        public GameServer(IGameTransport gameTransport)
        {
            transport = gameTransport;
            clientsTable = new Dictionary<EndPoint, GameClient>();
            commandsTable = new Dictionary<byte, GameCommand>();
            commandsTable[0] = CalculateAmount;
            commandsTable[3] = CalculateSub;
            commandsTable[6] = CalculateMultiplication;
            commandsTable[9] = CalculateDivision;


        }

        public void Run()
        {
            Console.WriteLine("server started");
            while (true)
            {
                SingleStep();
            }
        }

        public void SingleStep()
        {
            EndPoint sender = transport.CreateEndPoint();
            byte[] data = transport.Recv(256, ref sender);

            if (data != null)
            {
                byte gameCommand = data[0];
                if (commandsTable.ContainsKey(gameCommand))
                {
                    commandsTable[gameCommand](data, sender);
                }
            }

            foreach (GameClient client in clientsTable.Values)
            {
                client.Process();
            }

        }

        public bool Send(Packet packet, EndPoint endPoint)
        {
            return transport.Send(packet.GetData(), endPoint);
        }

        public GameClient GetClientFromEndPoint(EndPoint endPoint)
        {
            if (clientsTable.ContainsKey(endPoint))
            {
                return clientsTable[endPoint];
            }

            return null;
        }

    }
}