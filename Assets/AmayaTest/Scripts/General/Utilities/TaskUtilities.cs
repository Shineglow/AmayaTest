using System;
using System.Threading.Tasks;

namespace AmayaTest.Scripts.General.Utilities
{
    public static class TaskUtilities
    {
        public static async Task WaitWhile(Func<bool> conditionFunction)
        {
            if (conditionFunction == null)
                throw new NullReferenceException($"{nameof(conditionFunction)} must be not null");
            
            while (conditionFunction())
            {
                await Task.Yield();
            }
        }
    }
}