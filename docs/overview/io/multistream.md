# MultiStream
A _hook_ is a mechanism by which an application can intercept events, such as messages, mouse actions, and keystrokes. A function that intercepts a particular type of event is known as a _hook procedure_. A hook procedure can act on each event it receives, and then modify or discard the event.

[MouseHook](xref:NetEx.Hooks.MouseHook) enables you to monitor mouse input events about to be posted in a thread input queue.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.IO
```

Use the [Install](xref:NetEx.Hooks.MouseHook.Install) method to install the mouse hook. Then subscribe to the events you wish to be notified of, such as [MouseClick](xref:NetEx.Hooks.MouseHook.MouseClick) or [MouseMove](xref:NetEx.Hooks.MouseHook.MouseMove).

> [!NOTE]
> The hook procedure should process a message as quickly as possible, and may be be skipped or removed if it is deemed to have timed out.
> 
> See the [Install](xref:NetEx.Hooks.MouseHook.Install) method for more information.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to install and uninstall the [MouseHook](xref:NetEx.Hooks.MouseHook). When the hook is installed, every click of the mouse will increment the count [Label](xref:System.Windows.Forms.Label).

```csharp
using NetEx.Hooks;
using System;
using System.Windows.Forms;

public class MouseHookForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new MouseHookForm());
    }

    private Button selectButton;
    private Label clickCountLabel;
    private int clickCount;
    private bool hooked;

    public MouseHookForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Hook mouse"
        };
        clickCountLabel = new Label
        {
            Size = new Size(100, 20),
            Location = new Point(15, 50),
            Text = "Click count: 0"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
        Controls.Add(clickCountLabel);

        MouseHook.MouseDown += e =>
        {
            clickCount++;
            clickCountLabel.Text = $"Click count: {clickCount}";
        };
    }
    private void SelectButton_Click(object sender, EventArgs e)
    {
        if (hooked)
        {
            if (MouseHook.TryUninstall())
            {
                selectButton.Text = "Hook mouse";
                hooked = false;
            }
        }
        else
        {
            if (MouseHook.TryInstall())
            {
                selectButton.Text = "Unhook mouse";
                hooked = true;
            }
        }
    }
}
```

## Applies to

| Product            | Versions |
|--------------------|----------|
| **.Net**           | 5, 6, 7, 8, 9 |
| **.Net Framework** | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1 |
| **.Net Standard**  | 2.0, 2.1 |

## See also
[Hooks Overview](https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks)

[LowLevelMouseProc function](https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc)