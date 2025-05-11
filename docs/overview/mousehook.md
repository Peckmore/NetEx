# MouseHook
The [ProgressDialog](xref:NetEx.Dialogs.WinForms.ProgressDialog) component is a pre-configured dialog box. It is the same <b>Progress</b> dialog box exposed by the Windows operating system. It inherits from the [CommonDialog](xref:System.Windows.Forms.CommonDialog) class.

## Use this component

If you have not already added it to your project, install the package from NuGet:

```powershell
Install-Package NetEx.Hooks
```

Use this component within your Windows-based application as a simple solution for displaying progress in lieu of configuring your own dialog box. By relying on standard Windows dialog boxes, you create applications whose basic functionality is immediately familiar to users. Be aware, however, that when using the [ProgressDialog](xref:NetEx.Dialogs.WinForms.ProgressDialog) component, you must write your own progress updating logic.

Use the [ShowDialog](xref:System.Windows.Forms.CommonDialog.ShowDialog) method to display the dialog at run time in a modal fashion. Use the [Show](xref:NetEx.Dialogs.WinForms.ProgressDialog.Show) method to display the dialog at run time in a non-modal fashion.

When it is added to a form, the [ProgressDialog](xref:NetEx.Dialogs.WinForms.ProgressDialog) component appears in the tray at the bottom of the Windows Forms Designer in Visual Studio.

## Example

The following example uses the Windows Forms [Button](xref:System.Windows.Forms.Button) control's [Click](xref:System.Windows.Forms.Control.Click) event handler to install and uninstall the [MouseHook](xref:NetEx.Hooks.MouseHook). When the hook is installed, every click of the mouse will increment the click count [Label](xref:System.Windows.Forms.Label).

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

## API

API documentation is available [here](xref:NetEx.Dialogs.WinForms.ProgressDialog).

## Compatibility

| Product                   | Versions    |
|---------------------------|-------------|
| **.Net** *(Windows only)* | 5.0 - 9.0   |
| **.Net Framework**        | 2.0 - 4.8.1 |