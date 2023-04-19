using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Threading;

namespace FolderAndFilePicker.Core.Components.SaveFile
{
    public class SaveFileBase : ComponentBase
    {
		[Inject]
		protected IFileSaver fileSaver { get; set; }
		public string Text { get; set; }

		public string Result { get; set; }

		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		public async Task SaveFileAsync()
		{
			try
			{
				if (string.IsNullOrEmpty(Text))
				{
					Result = "Text is empty or null.";
					return;
				}

				using var stream = new MemoryStream(Encoding.Default.GetBytes(Text));
				var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}.txt";
				var path = await fileSaver.SaveAsync(fileName, stream, cancellationTokenSource.Token);

				Result = $"File saved at {path.ToString()}";
			}
			catch (OperationCanceledException)
			{
				Result = "User canceled the operation.";
			}
			catch (Exception ex)
			{
				Result = $"Error: {ex.Message}";
			}
		}
	}
}
