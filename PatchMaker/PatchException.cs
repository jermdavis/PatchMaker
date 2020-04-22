using System;

namespace PatchMaker
{

    public class PatchException : ArgumentException
    {
        public string PatchOperation { get; }
        public string FailingXPath { get; }
        public string Parameter { get; }

        public PatchException(string patchOperation, string failingXPath, string parameter) : base(
                $"{patchOperation} failed: The xpath query '{failingXPath}' matched no nodes in the source document.", 
                parameter)
        {
            PatchOperation = patchOperation;
            FailingXPath = failingXPath;
            Parameter = parameter;
        }
    }

}
