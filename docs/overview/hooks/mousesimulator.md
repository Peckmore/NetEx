# MouseSimulator
[MouseSimulator](xref:NetEx.Hooks.MouseSimulator) uses [SendInput](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput) to synthesize mouse motions and button clicks.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.Hooks
```

Use the methods on [MouseSimulator](xref:NetEx.Hooks.MouseSimulator), such as [MouseClick](xref:NetEx.Hooks.MouseSimulator.MouseClick*) or [MouseMove](xref:NetEx.Hooks.MouseSimulator.MouseMove*), to simulate mouse events.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to move the mouse by generating [MouseMove](xref:NetEx.Hooks.MouseSimulator.MouseMove*) events.

```csharp
using NetEx.Hooks;
using System;
using System.Threading;
using System.Windows.Forms;

public class MouseSimulatorForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new MouseSimulatorForm());
    }

    private Button selectButton;

    public MouseSimulatorForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Simulate mouse"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
    }
    private void SelectButton_Click(object sender, EventArgs e)
    {
        var y = SystemInformation.PrimaryMonitorSize.Height / 2;
        for (var x = 0; x < 200;  x++)
        {
            MouseSimulator.MouseMove(new(x, y));
            Thread.Sleep(1);
        }
    }
}
```

## Applies to

| Product             | Versions |
|---------------------|----------|
| **.Net Framework**  | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1 |
| **Windows Desktop** | 5, 6, 7, 8, 9 |

## See also
[SendInput function (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput)

[INPUT structure (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input)

[MOUSEINPUT structure (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput)