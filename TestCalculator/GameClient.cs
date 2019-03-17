using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TestCalculator
{
    public class GameClient
    {
        private EndPoint endPoint;

        private Queue<Packet> sendQueue;

        private Dictionary<uint, Packet> ackTable;

        public uint acktablecount { get { return (uint)ackTable.Count; } }

        private GameServer server;

        public GameClient(GameServer server, EndPoint endPoint)
        {
            this.server = server;
            this.endPoint = endPoint;
            sendQueue = new Queue<Packet>();
            ackTable = new Dictionary<uint, Packet>();
        }

        public void Process()
        {
            int packetsInQueue = sendQueue.Count;
            for (int i = 0; i < packetsInQueue; i++)
            {
                Packet packet = sendQueue.Dequeue();
                // check if the packet con be sent

                if (server.Send(packet, endPoint))
                {
                    // all fine
                    if (packet.needAck)
                    {
                        ackTable[packet.Id] = packet;
                    }
                }
                // on error, retry sending only if NOT OneShot

            }
        }


        public void Ack(uint packetId)
        {
            if (ackTable.ContainsKey(packetId))
            {
                ackTable.Remove(packetId);
            }
            // else, increase malus
        }

        public void Enqueue(Packet packet)
        {
            sendQueue.Enqueue(packet);
        }

        public override string ToString()
        {
            return endPoint.ToString();
        }
    }
}
