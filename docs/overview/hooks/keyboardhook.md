# KeyboardHook

A _hook_ is a mechanism by which an application can intercept events, such as messages, mouse actions, and keystrokes. A function that intercepts a particular type of event is known as a _hook procedure_. A hook procedure can act on each event it receives, and then modify or discard the event.

[KeyboardHook](xref:NetEx.Hooks.KeyboardHook) enables you to monitor keyboard input events about to be posted in a thread input queue.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.Hooks
```

Use the [Install](xref:NetEx.Hooks.KeyboardHook.Install) method to install the keyboard hook. Then subscribe to the events you wish to be notified of, such as [KeyDown](xref:NetEx.Hooks.KeyboardHook.KeyDown) or [KeyUp](xref:NetEx.Hooks.KeyboardHook.KeyUp).

> [!NOTE]
> The hook procedure should process a message as quickly as possible, and may be be skipped or removed if it is deemed to have timed out.
> 
> See the [Install](xref:NetEx.Hooks.KeyboardHook.Install) method for more information.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to install and uninstall the [KeyboardHook](xref:NetEx.Hooks.KeyboardHook). When the hook is installed, every press of the spacebar will increment the count [Label](xref:System.Windows.Forms.Label).

```csharp
using NetEx.Hooks;
using System;
using System.Windows.Forms;

public class KeyboardHookForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new KeyboardHookForm());
    }

    private Button selectButton;
    private Label spacebarCountLabel;
    private int spacebarCount;
    private bool hooked;

    public KeyboardHookForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Hook keyboard"
        };
        spacebarCountLabel = new Label
        {
            Size = new Size(150, 20),
            Location = new Point(15, 50),
            Text = "Spacebar count: 0"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
        Controls.Add(spacebarCountLabel);

        KeyboardHook.KeyDown += e =>
        {
            if (e.KeyCode == NetEx.Hooks.Keys.Space)
            {
                spacebarCount++;
                spacebarCountLabel.Text = $"Spacebar count: {spacebarCount}";
            }
        };
    }
    private void SelectButton_Click(object sender, EventArgs e)
    {
        if (hooked)
        {
            if (KeyboardHook.TryUninstall())
            {
                selectButton.Text = "Hook keyboard";
                hooked = false;
            }
        }
        else
        {
            if (KeyboardHook.TryInstall())
            {
                selectButton.Text = "Unhook keyboard";
                hooked = true;
            }
        }
    }
}
```

## Applies to

Product             | Versions
--------------------|---------
**.NET Framework**  | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
**Windows Desktop** | 5, 6, 7, 8, 9

## See Also

[Hooks Overview](https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks)

[LowLevelKeyboardProc function](https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelkeyboardproc)