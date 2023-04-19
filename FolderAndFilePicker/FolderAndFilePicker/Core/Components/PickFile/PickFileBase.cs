using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;
using Microsoft.AspNetCore.Components;

namespace FolderAndFilePicker.Core.Components.PickFile
{
    public class PickFileBase : ComponentBase
    {
        public List<FileInfo> pickedFiles = new List<FileInfo>();
        public string imageSrc;

        public async Task PickImage()
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick Image Please",
                FileTypes = FilePickerFileType.Images
            });

            if (file == null)
                imageSrc = "";

            pickedFiles.Add(new FileInfo { Name = file.FileName, Path = file.FullPath, Type = file.ContentType });

            var stream = await file.OpenReadAsync();

            imageSrc = ImageSource.FromStream(() => stream).ToString();
        }

        public async Task PickFiles()
        {
            // For custom file types            
            //var customFileType =
            //	new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            //	{
            //		 { DevicePlatform.iOS, new[] { "com.adobe.pdf" } }, // or general UTType values
            //       { DevicePlatform.Android, new[] { "application/pdf" } },
            //		 { DevicePlatform.WinUI, new[] { ".pdf" } },
            //		 { DevicePlatform.Tizen, new[] { "*/*" } },
            //		 { DevicePlatform.macOS, new[] { "pdf"} }, // or general UTType values
            //	});

            var files = await FilePicker.PickMultipleAsync(new PickOptions
            {
                //FileTypes = customFileType
            });

            foreach (var file in files)
            {
                pickedFiles.Add(new FileInfo { Name = file.FileName, Path = file.FullPath, Type=file.ContentType });
            }
        }
    }

    public class FileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
