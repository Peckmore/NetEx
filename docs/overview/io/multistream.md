# MultiStream

The abstract base class [Stream](xref:System.IO.Stream) supports reading and writing bytes. All classes that represent streams inherit from the [Stream](xref:System.IO.Stream) class. The [Stream](xref:System.IO.Stream) class and its derived classes provide a common view of data sources and repositories, and isolate the programmer from the specific details of the operating system and underlying devices.

[MultiStream](xref:NetEx.IO.MultiStream) enables you to wrap multiple streams, and present them as a single, read-only [Stream](xref:System.IO.Stream).

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.IO
```

Use the [MultiStream](xref:NetEx.IO.MultiStream) constructor to create a [MultiStream](xref:NetEx.IO.MultiStream) instance that wraps other streams. You can then use the [MultiStream](xref:NetEx.IO.MultiStream) instance wherever you can use a [Stream](xref:System.IO.Stream).

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to create two [MemoryStream](xref:System.IO.MemoryStream) instances, each containing a string, and then creates a [MultiStream](xref:NetEx.IO.MultiStream) instance to wrap the streams. The [MultiStream](xref:NetEx.IO.MultiStream) instance is then passed to a [StreamReader](xref:System.IO.StreamReader), and the contents of the combined streams are displayed in a [MessageBox](xref:System.Windows.Forms.MessageBox).

```csharp
using NetEx.IO;
using System;
using System.Text;
using System.Windows.Forms;

public class MultiStreamForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new MultiStreamForm());
    }

    private Button selectButton;

    public MultiStreamForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Read MultiStream"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
    }

    private void SelectButton_Click(object sender, EventArgs e)
    {
        using var stream1 = new MemoryStream(Encoding.ASCII.GetBytes("Hello "));
        using var stream2 = new MemoryStream(Encoding.ASCII.GetBytes("World!"));
        using var multiStream = new MultiStream(stream1, stream2);
        using var streamReader = new StreamReader(multiStream);

        MessageBox.Show($"Stream 1 Length: {stream1.Length}\n"
                        + $"Stream 2 Length: {stream2.Length}\n"
                        + $"MultiStream Length: {multiStream.Length}\n\n"
                        + "Stream Text:\n"
                        + streamReader.ReadLine());
    }
}
```

## Applies to

Product            | Versions
-------------------|---------
**.NET**           | 5, 6, 7, 8, 9
**.NET Framework** | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
**.NET Standard**  | 2.0, 2.1

## See also

[Files and Stream I/O](https://learn.microsoft.com/en-us/dotnet/standard/io/#streams)