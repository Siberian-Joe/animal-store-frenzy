using System;
using NewCore.Domain;

namespace NewCore.Data
{
    public interface IProxy
    {
        IModel ToModel();
    }
}