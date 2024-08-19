using System.Windows;

namespace Bau.Libraries.BauOTP;

/// <summary>
///		Ventana principal de la aplicación de ejemplo BauOtp para generación de claves OTP
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	/// <summary>
	///		Inicializa la ventana
	/// </summary>
	private void InitWindow()
	{
		// Inicializa los objetos
		DataContext = MainViewModel = new ViewModels.BauOtpMainViewModel(new Controllers.OtpController(this));
		// Carga el archivo de propiedades
		MainViewModel.Load(GetDefaultFileName());
	}

	/// <summary>
	///		Obtiene el nombre de archivo predeterminado
	/// </summary>
	private string GetDefaultFileName() => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OtpProviders.xml");

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		InitWindow();
	}

	/// <summary>
	///		Manager de ViewModels de la aplicación de ejemplo
	/// </summary>
	public ViewModels.BauOtpMainViewModel MainViewModel { get; private set; } = default!;
}