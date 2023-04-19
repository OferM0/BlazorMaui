using CommunityToolkit.Maui.Storage;
using Microsoft.AspNetCore.Components;

namespace FolderAndFilePicker.Core.Components.PickFolder
{
    public class PickFolderBase : ComponentBase
    {
        public List<FolderInfo> pickedFolders = new List<FolderInfo>();

        public async Task OpenFolderPicker()
        {
            try
            {
                var folder = await FolderPicker.PickAsync(default);

                if (folder != null)
                {
                    pickedFolders.Add(new FolderInfo { Name = folder.Folder.Name, Path = folder.Folder.Path });
                }
            }
            catch
            {
            }
        }

        public async Task<string> OpenFileDialog()
        {

            try
            {
                var folder = await FolderPicker.PickAsync(default);

                return $"Name: {folder.Folder.Name} Path: {folder.Folder.Path}";
            }
            catch
            {
                return null;
            }
        }
    }

    public class FolderInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}