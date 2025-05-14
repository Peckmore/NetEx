# OnDisposeStream

The abstract base class [Stream](xref:System.IO.Stream) supports reading and writing bytes. All classes that represent streams inherit from the [Stream](xref:System.IO.Stream) class. The [Stream](xref:System.IO.Stream) class and its derived classes provide a common view of data sources and repositories, and isolate the programmer from the specific details of the operating system and underlying devices.

[OnDisposeStream](xref:NetEx.IO.OnDisposeStream) enables you to wrap a stream and supply an action to be carried out when the underlying stream is closed. This is useful for situations whereby a [Stream](xref:System.IO.Stream) is created from a resource, and you want to be able to clean up that resource when the [Stream](xref:System.IO.Stream) is closed.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.IO
```

Use the [OnDisposeStream](xref:NetEx.IO.OnDisposeStream) constructor to create a [OnDisposeStream](xref:NetEx.IO.OnDisposeStream) instance that wraps another [Stream](xref:System.IO.Stream), supplying an action to be invoked when that stream is closed. You can then use the [OnDisposeStream](xref:NetEx.IO.OnDisposeStream) instance wherever you can use a [Stream](xref:System.IO.Stream).

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to create a [ZipArchive](xref:System.IO.Compression.ZipArchive), which is saved in a [MemoryStream](xref:System.IO.MemoryStream), and contains a single text file.

The [ZipArchive](xref:System.IO.Compression.ZipArchive) is then opened in a separate `OpenZip` method, and the compressed file returned as an [OnDisposeStream](xref:NetEx.IO.OnDisposeStream). The contents of the compressed file are displayed in a [MessageBox](xref:System.Windows.Forms.MessageBox), and the [OnDisposeStream](xref:NetEx.IO.OnDisposeStream) is then closed. When the [OnDisposeStream](xref:NetEx.IO.OnDisposeStream) is closed, it will also close the [ZipArchive](xref:System.IO.Compression.ZipArchive) and show a [MessageBox](xref:System.Windows.Forms.MessageBox).

> [!NOTE]
> In the below example, the method to open a compressed file within the zip file (`OpenZip`) is within the same class as the code that consumes the compressed file. So in this scenario it would be easy to close the zip file directly.
>
> However, in a real-world scenario the method to return the compressed file may be contained in a library, and as a maintainer of the library you may wish to ensure that all resources you use are cleaned up. To do so you could return an `OnDisposeStream` instance, which would ensure that, when the consumer of your library is finished with the compressed file, you are able to clean up any resources you used.

```csharp
using NetEx.IO;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

public class OnDisposeStreamForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new OnDisposeStreamForm());
    }

    private Button selectButton;

    public OnDisposeStreamForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Read OnDisposeStream"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
    }

    private void SelectButton_Click(object sender, EventArgs e)
    {
        // Store data in a byte array instead of creating temporary files
        var data = new byte[256];

        // Create a zip file containing a "Hello World!" text file
        using (var zipFileStream = new MemoryStream(data))
        {
            using (var zipFile = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
            {
                var compressedFile = zipFile.CreateEntry("TextFile.txt");
                using (var compressedFileStream = compressedFile.Open())
                {
                    using (var compressedFileStreamWriter = new StreamWriter(compressedFileStream))
                    {
                        compressedFileStreamWriter.Write("Hello World!");
                    }
                }
            }
        }

        // Now open the zip file and read the text file
        using (var zipFileStream = new MemoryStream(data))
        {
            using (var streamReader = new StreamReader(OpenZip(zipFileStream)))
            {
                MessageBox.Show(streamReader.ReadLine());
            }
        }
    }

    private Stream OpenZip(Stream zipFileStream)
    {
        var zipFile = new ZipArchive(zipFileStream, ZipArchiveMode.Read);
        var compressedFile = zipFile.GetEntry("TextFile.txt");
        return new OnDisposeStream(compressedFile!.Open(), b =>
        {
            zipFile.Dispose();
            MessageBox.Show("Zip file disposed!");
        });
    }
}
```

## Applies to

Product            | Versions
-------------------|---------
**.Net**           | 5, 6, 7, 8, 9
**.Net Framework** | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
**.Net Standard**  | 2.0, 2.1

## See also

[Files and Stream I/O](https://learn.microsoft.com/en-us/dotnet/standard/io/#streams)

## Acknowledgements

Credit goes to [craftworkgames](https://stackoverflow.com/a/34079296) for the original idea and implementation of `OnDisposeStream`.