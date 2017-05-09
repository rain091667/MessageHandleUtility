using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatMessageServerManager manager = new ChatMessageServerManager();
            manager.Start();
            Console.ReadKey();
        }
    }
}