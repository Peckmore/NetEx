# KeyboardSimulator

[KeyboardSimulator](xref:NetEx.Hooks.KeyboardSimulator) uses [SendInput](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput) to synthesize keystrokes.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.Hooks
```

Use the methods on [KeyboardSimulator](xref:NetEx.Hooks.KeyboardSimulator), such as [KeyDown](xref:NetEx.Hooks.KeyboardSimulator.KeyDown*) or [KeyUp](xref:NetEx.Hooks.KeyboardSimulator.KeyUp*), to simulate keyboard events.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to open the Windows Start menu by generating a [KeyPress](xref:NetEx.Hooks.KeyboardSimulator.KeyPress*) event.

```csharp
using NetEx.Hooks;
using System;
using System.Windows.Forms;

public class KeyboardSimulatorForm : Form
{
    [STAThread]
    public static void Main()
    {
        Application.SetCompatibleTextRenderingDefault(false);
        Application.EnableVisualStyles();
        Application.Run(new KeyboardSimulatorForm());
    }

    private Button selectButton;

    public KeyboardSimulatorForm()
    {
        selectButton = new Button
        {
            Size = new Size(100, 20),
            Location = new Point(15, 15),
            Text = "Simulate keyboard"
        };
        selectButton.Click += new EventHandler(SelectButton_Click);
        ClientSize = new Size(330, 360);
        Controls.Add(selectButton);
    }
    private void SelectButton_Click(object sender, EventArgs e)
    {
        KeyboardSimulator.KeyPress(NetEx.Hooks.Keys.LWin);
    }
}
```

## Applies to

Product             | Versions
--------------------|---------
**.NET Framework**  | 2.0, 3.0, 3.5, 4.0, 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
**Windows Desktop** | 5, 6, 7, 8, 9

## See also

[SendInput function (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput)

[INPUT structure (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input)

[KEYBDINPUT structure (winuser.h)](https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput)