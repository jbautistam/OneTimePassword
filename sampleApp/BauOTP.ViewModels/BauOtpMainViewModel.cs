using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.ListView;

namespace Bau.Libraries.BauOTP.ViewModels;

/// <summary>
///		Manager de los ViewModels de la aplicación ejemplo de la librería OneTimePassword
/// </summary>
public class BauOtpMainViewModel : BauMvvm.ViewModels.BaseObservableObject
{
	// Variables privadas
	private string? _fileName;
	private ControlGenericListViewModel<Generators.GeneratorViewModel>? _otpListViewModel;

	public BauOtpMainViewModel(Interfaces.IBauOtpController mainController)
	{
		MainController = mainController;
		NewCommand = new BauMvvm.ViewModels.BaseCommand(_ => Open(null));
		OpenCommand = new BauMvvm.ViewModels.BaseCommand(_ => Open(OtpListViewModel?.SelectedItem?.Tag as Models.OtpPropertyModel));
		DeleteCommand = new BauMvvm.ViewModels.BaseCommand(_ => Delete());
	}

	/// <summary>
	///		Carga la lista de archivos
	/// </summary>
	public void Load(string fileName)
	{
		List<Models.OtpPropertyModel> otpProperties = new Repositories.OtpPropertiesRepository().Load(fileName);

			// Ordena la lista por nombre
			otpProperties.Sort((first, second) => (first.Name ?? string.Empty).CompareTo(second.Name));
			// Guarda el nombre del archivo
			_fileName = fileName;
			// Inicializa la lista
			OtpListViewModel = new ControlGenericListViewModel<Generators.GeneratorViewModel>();
			// Muestra los datos en la lista
			foreach (Models.OtpPropertyModel otpProperty in otpProperties)
				OtpListViewModel.Add(new BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel(otpProperty.Name ?? "Name", otpProperty));
	}

	/// <summary>
	///		Graba los datos del archivo
	/// </summary>
	public void Save()
	{
		if (OtpListViewModel is not null && !string.IsNullOrWhiteSpace(_fileName))
		{
			List<Models.OtpPropertyModel> otpProperties = new();

				// Guarda las propiedades
				foreach (BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel viewModel in OtpListViewModel.Items)
					if (viewModel.Tag is Models.OtpPropertyModel otpProperty)
						otpProperties.Add(otpProperty);
				// Graba el archivo de propiedades
				new Repositories.OtpPropertiesRepository().Save(otpProperties, _fileName);
		}
	}

	/// <summary>
	///		Abre los datos de un generador
	/// </summary>
	private void Open(Models.OtpPropertyModel? optProperties)
	{
		if (OtpListViewModel is not null)
		{
			bool isNew = optProperties is null;
			Generators.GeneratorViewModel viewModel = new(this, optProperties ?? new Models.OtpPropertyModel());

				// Abre la ventana
				if (MainController.OpenOtpView(viewModel))
				{
					// Añade el generador
					if (isNew)
						OtpListViewModel.Add(new BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel(viewModel.OtpProperties.Name!, viewModel.OtpProperties));
					// Graba los datos
					Save();
					// Actualiza la lista
					if (!string.IsNullOrWhiteSpace(_fileName))
						Load(_fileName);
				}
		}
	}

	/// <summary>
	///		Borra los datos de un proveedor
	/// </summary>
	private void Delete()
	{
		if (OtpListViewModel?.SelectedItem is not null && MainController.ShowQuestion($"Do you want to delete the application {OtpListViewModel.SelectedItem.Text}?"))
		{
			OtpListViewModel.Items.Remove(OtpListViewModel.SelectedItem);
			Save();
		}
	}

	/// <summary>
	///		Controlador principal
	/// </summary>
	public Interfaces.IBauOtpController MainController { get; }

	/// <summary>
	///		ViewModel de la lista de datos de servidores OTP definidos
	/// </summary>
	public ControlGenericListViewModel<Generators.GeneratorViewModel>? OtpListViewModel
	{
		get { return _otpListViewModel; }
		set { CheckObject(ref _otpListViewModel, value); }
	}

	/// <summary>
	///		Comando para añadir de un generador de claves
	/// </summary>
	public BauMvvm.ViewModels.BaseCommand NewCommand { get; }

	/// <summary>
	///		Comando para abrir las propiedades de un generador de claves
	/// </summary>
	public BauMvvm.ViewModels.BaseCommand OpenCommand { get; }

	/// <summary>
	///		Comando para borrar un generador
	/// </summary>
	public BauMvvm.ViewModels.BaseCommand DeleteCommand { get; }
}
