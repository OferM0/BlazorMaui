using Microsoft.AspNetCore.Components;

namespace ResultViewer.Client.Core.Components.NavMenu
{
    public class NavMenuBase : ComponentBase
    {
        public async Task OpenFileDialog()
        {
            await FilePicker.PickAsync();
        }
    }
}
