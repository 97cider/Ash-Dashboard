using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Ashapi.Detection;
using NetMQ;
using AsyncIO;
using NetMQ.Sockets;
//using ZeroMQ;
using ElectronCgi.DotNet;
namespace nodeBackend
{
    public class NetMqListener
    {
        private readonly Thread _listenerWorker;

        private bool _listenerCancelled;

        public delegate void MessageDelegate(string message);

        private readonly MessageDelegate _messageDelegate;

        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

        private void ListenerWork()
        {
            ////Console.Write("Starting listener thread");
            //System.Diagnostics.Debug.Write("STARTING LISTENER THREAD");
            AsyncIO.ForceDotNet.Force();
            using (var subSocket = new SubscriberSocket())
            {
                //System.Diagnostics.Debug.Write("conn");
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Connect("tcp://127.0.0.1:1339");
                subSocket.Subscribe("");
                while (!_listenerCancelled)
                {
                    string frameString;
                    List<string> strs = new List<string>();
                    //if (!subSocket.TryReceive(out frameString)) continue;
                    byte[] arr;
                    if (!subSocket.TryReceiveFrameBytes(out arr)) continue;
                    //Console.Write("REC");
                    Face fc = Face.Parser.ParseFrom(arr);
                    _messageQueue.Enqueue(fc.ToString());
                }
                subSocket.Close();
            }
            NetMQConfig.Cleanup();
        }
        public NetMqListener(MessageDelegate messageDelegate)
        {
            _messageDelegate = messageDelegate;
            _listenerWorker = new Thread(ListenerWork);
        }
        public void Start()
        {
            _listenerCancelled = false;
            _listenerWorker.Start();
            //Console.Write("Passed listener worker");
        }

        public void Stop()
        {
            _listenerCancelled = true;
            _listenerWorker.Join();
        }
        public void Update()
        {
            while (!_messageQueue.IsEmpty)
            {
                string message;
                if (_messageQueue.TryDequeue(out message))
                {
                    _messageDelegate(message);
                }
                else
                {
                    break;
                }
            }
        }
    }
    //class NetMqReader {
    //    public static void PathoSub()
    //    {
    //        //
    //        // Pathological subscriber
    //        // Subscribes to one random topic and prints received messages
    //        //
    //        // Author: metadings
    //        //

    //        string end = "tcp://127.0.0.1:1339";
    //        using (var context = new ZContext())
    //        using (var subscriber = new ZSocket(context, ZSocketType.SUB))
    //        {
    //            subscriber.Connect(end);

    //            var rnd = new Random();
    //            var subscription = "";
    //            subscriber.Subscribe(subscription);

    //            ZMessage msg;
    //            ZError error;
    //            while (true)
    //            {
    //                if (null == (msg = subscriber.ReceiveMessage(out error)))
    //                {
    //                    if (error == ZError.ETERM)
    //                        break;    // Interrupted
    //                    throw new ZException(error);
    //                }
    //                using (msg)
    //                {

    //                    //Console.WriteLine(msg[1].ReadString());
    //                }
    //            }
    //        }
    //    }
    //}

    class NetMqReal
    {
        public static ConcurrentQueue<string> isFaceRes;
        public static void listenNode()
        {
            var connection = new ConnectionBuilder().WithLogging().Build();
            string isFace;
            try
            {
                isFaceRes.TryDequeue(out isFace);
                connection.On<string, string>("greeting", name => "Face: " + isFace);
                connection.Listen();
            }
            catch (Exception e)
            {
                //Console.Write(e);
            }
        }
        public static void Read()
        {
            isFaceRes = new ConcurrentQueue<string>();
            string topic = "";
            //Console.WriteLine("Subscriber started for Topic : {0}", topic);
            
            using (var subSocket = new SubscriberSocket())
            {
            
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Connect("tcp://127.0.0.1:1339");
                subSocket.Subscribe(topic);
                while (true)
                {
                    byte[] arr;
                    Face fc;
                    if(!subSocket.TryReceiveFrameBytes(out arr)) continue;
                    //subSocket.ReceiveFrameString();
                    fc = Face.Parser.ParseFrom(arr);
                    
                    ////Console.WriteLine(messageTopicReceived    );
                    //string messageReceived = subSocket.ReceiveFrameString();

                    //Console.WriteLine(fc.IsFace);
                    isFaceRes.Enqueue(fc.IsFace.ToString());
                }
            }
        }
    }

    class Program
    {
        //public static NetMqListener.MessageDelegate HandleMessage { get; private set; }
        static void Main(string[] args)
        {

            Thread t = new Thread(NetMqReal.Read);
            t.Start();
            var connection = new ConnectionBuilder().WithLogging().Build();
            string face;
            NetMqReal.isFaceRes.TryDequeue(out face);
            
            connection.On<string, string>("greeting", name => "Face: " + NetMqReal.isFaceRes.Count.ToString());
            connection.Listen();
            
        }

        private static void HandleMessage(string message)
        {
            //Console.Write(message);
        }
    }
    
}
