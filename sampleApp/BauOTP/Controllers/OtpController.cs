using System.Windows;
using Bau.Libraries.BauOTP.ViewModels.Generators;

namespace Bau.Libraries.BauOTP.Controllers;

/// <summary>
///		Controlador principal de la aplicación de ejemplos del generador
/// </summary>
public class OtpController : ViewModels.Interfaces.IBauOtpController
{
	public OtpController(MainWindow mainWindow)
	{
		MainWindow = mainWindow;
	}

	/// <summary>
	///		Abre la ventana del generador OTP
	/// </summary>
	public bool OpenOtpView(GeneratorViewModel viewModel)
	{
		Views.OtpGeneratorView otpGeneratorView = new(viewModel);

			return ShowDialog(MainWindow, otpGeneratorView);
	}

	/// <summary>
	///		Muestra un cuadro de diálogo
	/// </summary>
	private bool ShowDialog(Window owner, Window view, WindowStyle style = WindowStyle.ToolWindow)
	{ 
		// Si no se le ha pasado una ventana propietario, le asigna una
		if (owner is null)
			owner = MainWindow;
		// Muestra el formulario activo
		view.Owner = owner;
		view.ShowActivated = true;
		view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		view.WindowStyle = style;
		view.ShowInTaskbar = false;
		if (style == WindowStyle.ToolWindow)
			view.ResizeMode = ResizeMode.NoResize;
		// Muestra el formulario y devuelve el resultado
		return view.ShowDialog() ?? false;
	}

	/// <summary>
	///		Muestra una ventana de mensaje
	/// </summary>
	public void ShowMessage(string message)
	{
		MainWindow.Dispatcher.Invoke(new Action(() => MessageBox.Show(MainWindow, message, MainWindow.Title)),
									 System.Windows.Threading.DispatcherPriority.Normal);
	}

	/// <summary>
	///		Muestra una ventana de interrogación sí / no
	/// </summary>
	public bool ShowQuestion(string message) => MessageBox.Show(MainWindow, message, MainWindow.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;

	/// <summary>
	///		Ventana principal
	/// </summary>
	public MainWindow MainWindow { get; }
}
