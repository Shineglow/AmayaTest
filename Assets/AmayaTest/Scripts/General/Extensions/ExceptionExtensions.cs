using System;

namespace AmayaTest.Scripts.General.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"{name} is not initialized");
            }
        }
    }
}