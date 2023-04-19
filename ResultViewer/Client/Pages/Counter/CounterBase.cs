using Microsoft.AspNetCore.Components;

namespace ResultViewer.Client.Pages.Counter
{
    public class CounterBase : ComponentBase
    {
        public int CurrentCount { get; set; } = 0;

        public void IncrementCount()
        {
            CurrentCount++;
        }
    }
}
