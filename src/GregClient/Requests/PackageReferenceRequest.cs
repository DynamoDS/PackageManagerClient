using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Greg.Requests
{
    public enum PackageReferenceStyle { Id, EngineAndName };

    public abstract class PackageReferenceRequest : Request
    {
        protected PackageReferenceStyle _type;
        protected string _id;
        protected string _name;
        protected string _engine;
    }
}
