using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace gRPC_v2
{
    public class TelnetService : Telnet.TelnetBase
    {
        private List<string> _getMethodsName;

        public TelnetService()
        {
            _getMethodsName = new List<string>();
            _getMethodsName.Add(nameof(GetLabOwner));
            _getMethodsName.Add(nameof(ReturnRandomNumber));
            _getMethodsName.Add(nameof(ReturnMyMarkForThisLab));
        }

        private string GetLabOwner()
        {
            return "Я люблю старосту!";
        }

        private int ReturnRandomNumber()
        {
            return new Random().Next(100);
        }

        private int ReturnMyMarkForThisLab()
        {
            return 100;
        }

        private string CallMethod(string requiredMethod)
        {
            if (requiredMethod == nameof(GetLabOwner))
            {
                return GetLabOwner();
            }

            if (requiredMethod == nameof(ReturnRandomNumber))
            {
                return ReturnRandomNumber().ToString();
            }

            if (requiredMethod == nameof(ReturnMyMarkForThisLab))
            {
                return ReturnMyMarkForThisLab().ToString();
            }

            return
                "I know that it would be better to use polymorphism, but this lab is a demo of gRPC, not a CleanCode book.";
        }

        public override Task<Reply> SendRequest(Request request, ServerCallContext context)
        {
            var reply = new Reply();
            if (request.Message == "Give me methods names!")
            {
                reply.Message = string.Join("\n", _getMethodsName);
            }
            else
            {
                var result = CallMethod(request.Message);
                reply.Message = $"You've executed #{request.Message} and get result {result}";
            }

            return Task.FromResult(reply);
        }
    }
}