using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.View
{
    internal interface IView
    {
        void SendMessage(string message);
        void SendImage(string imageAdress);
        string GetMessage();
        void StartReceiving();
        void StopReceiving();
    }
}
