# ClipboardHook

There are three ways of monitoring changes to the clipboard. The oldest method is to create a _clipboard viewer window_. **Windows 2000** added the ability to query the clipboard sequence number, and **Windows Vista** added support for _clipboard format listeners_.

A _clipboard viewer window_ displays the current content of the clipboard, and receives messages when the clipboard content changes. A _clipboard format listener_ is a window which has registered to be notified when the contents of the clipboard has changed.

On **Windows Vista** and later, [ClipboardHook](xref:NetEx.Hooks.ClipboardHook) will create a _clipboard format listener_ to listen for clipboard updates. On earlier versions it will create a _clipboard viewer window_ instead.
    
## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.Hooks
```

Use the [Install](xref:NetEx.Hooks.ClipboardHook.Install) method to install the clipboard hook. Then subscribe to the [ClipboardUpdated](xref:NetEx.Hooks.ClipboardHook.ClipboardUpdated) event.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to install and uninstall the [ClipboardHook](xref:NetEx.Hooks.ClipboardHook). When the hook is installed, every change to the clipboard contents will increment the count [Label](xref:System.Windows.Forms.Label).

```csharp
using NetEx.Hooks;
using System;
using System.Windows.Forms;

public class ClipboardHookForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new ClipboardHookForm());
    }

    private Button selectButton;
    private Label changeCountLabel;
    private int changeCount;
    private bool hooked;

    public ClipboardHookForm()
    {
        selectButton = new Button
        {
            Size = new Size(150, 20),
            Location = new Point(15, 15),
            Text = "Hook clipboard"
        };
        changeCountLabel = new Label
        {
            Size = new Size(200, 20),
            Location = new Point(15, 50),
            Text = "Clipboard change count: 0"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
        Controls.Add(changeCountLabel);

        ClipboardHook.ClipboardUpdated += () =>
        {
            changeCount++;
            // Invoke back on the UI thread
            this.Invoke(() => changeCountLabel.Text = $"Clipboard change count: {changeCount}");
        };
    }
    private void SelectButton_Click(object sender, EventArgs e)
    {
        if (hooked)
        {
            if (ClipboardHook.TryUninstall())
            {
                selectButton.Text = "Hook clipboard";
                hooked = false;
            }
        }
        else
        {
            if (ClipboardHook.TryInstall())
            {
                selectButton.Text = "Unhook clipboard";
                hooked = true;
            }
        }
    }
}
```

## Applies to

Product             | Versions
--------------------|---------
**.Net Framework**  | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
**Windows Desktop** | 5, 6, 7, 8, 9

## See Also

[Monitoring Clipboard Contents](https://learn.microsoft.com/en-us/windows/win32/dataxchg/using-the-clipboard#monitoring-clipboard-contents)