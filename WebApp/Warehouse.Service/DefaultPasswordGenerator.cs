using System;
using System.Linq;
using Warehouse.Service.Abstractions;

namespace Warehouse.Service
{
    public class DefaultPasswordGenerator : IDefaultPasswordGenerator
    {
        private static readonly Random Random = new Random();
        public string GetDefaultPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!?";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}