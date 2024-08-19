using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.ComboItems;
using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Libraries.OneTimePassword;

namespace Bau.Libraries.BauOTP.ViewModels.Generators;

/// <summary>
///		Base de los viewModel de generación de claves Otp
/// </summary>
public class GeneratorViewModel : BaseObservableObject
{
	// Eventos públicos
	public event EventHandler<BauMvvm.ViewModels.Forms.EventArguments.EventCloseArgs>? Close;
	// Variables privadas
	private string? _key, _name, _error;
	private string _hotpCode = default!, _totpCode = default!, _remainingTime = default!;
	private int _digits, _interval, _previousRemaining = -1;
	private long _counter;
	private ComboViewModel? _comboEncodings;
	private ComboViewModel? _comboShaAlgorithms;
	private System.Timers.Timer? _timer;
	private MvvmColor _remainingTimeColor = MvvmColor.Green;

	public GeneratorViewModel(BauOtpMainViewModel mainViewModel, Models.OtpPropertyModel otpProperties)
	{
		// Inicializa los objetos
		MainViewModel = mainViewModel;
		OtpProperties = otpProperties;
		// Inicializa el viewModel
		InitViewModel();
		// Asigna los comandos
		SaveCommand = new BaseCommand(_ => Save());
		CancelCommand = new BaseCommand(_ => Cancel());
	}

	/// <summary>
	///		Inicializa el viewModel
	/// </summary>
	private void InitViewModel()
	{
		// Carga los combos
		LoadComboEncodings();
		LoadComboShaAlgorithms();
		// Asigna los datos del modelo
		Name = OtpProperties.Name;
		Key = OtpProperties.Key;
		Digits = OtpProperties.Digits;
		Counter = OtpProperties.Counter;
		Interval = OtpProperties.Interval;
		if (ComboEncodings is not null)
		{
			ComboEncodings.SelectedId = (int) OtpProperties.Encoding;
			ComboEncodings.PropertyChanged += (sender, args) => {
																	if (!string.IsNullOrWhiteSpace(args.PropertyName) &&
																			args.PropertyName.Equals(nameof(ComboEncodings.SelectedItem), StringComparison.CurrentCultureIgnoreCase))
																		UpdateCode();
																};
		}
		if (ComboShaAlgorithms is not null)
		{
			ComboShaAlgorithms.SelectedId = (int) OtpProperties.HashAlgorithm;
			ComboShaAlgorithms.PropertyChanged += (sender, args) => {
																		if (!string.IsNullOrWhiteSpace(args.PropertyName) &&
																				args.PropertyName.Equals(nameof(ComboShaAlgorithms.SelectedItem), StringComparison.CurrentCultureIgnoreCase))
																			UpdateCode();
																	};
		}
		// Inicializa el temporizador
		_timer = new System.Timers.Timer();
		_timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
		_timer.Elapsed += (sender, args) => UpdateCodeTimer();
		_timer.Start();
	}

	/// <summary>
	///		Carga el combo de codificaciones
	/// </summary>
	private void LoadComboEncodings()
	{
		ComboEncodings = new ComboViewModel(this);
		ComboEncodings.AddItem((int) Secret.Encoding.Plain, "Plain");
		ComboEncodings.AddItem((int) Secret.Encoding.Base32, "Base 32");
		ComboEncodings.AddItem((int) Secret.Encoding.Base64, "Base 64");
		ComboEncodings.SelectedIndex = 0;
	}

	/// <summary>
	///		Obtiene la codificación seleccionada
	/// </summary>
	protected Secret.Encoding GetEncoding()
	{
		if (ComboEncodings is null)
			return Secret.Encoding.Plain;
		else
			return (Secret.Encoding) (ComboEncodings.SelectedId ?? (int) Secret.Encoding.Plain);
	}

	/// <summary>
	///		Carga el combo de algoritmos SHA
	/// </summary>
	private void LoadComboShaAlgorithms()
	{
		ComboShaAlgorithms = new ComboViewModel(this);
		ComboShaAlgorithms.AddItem((int) BaseTokenGenerator.HashAlgorithm.Sha1, "SHA 1");
		ComboShaAlgorithms.AddItem((int) BaseTokenGenerator.HashAlgorithm.Sha256, "SHA 256");
		ComboShaAlgorithms.AddItem((int) BaseTokenGenerator.HashAlgorithm.Sha512, "SHA 512");
		ComboShaAlgorithms.SelectedIndex = 0;
	}

	/// <summary>
	///		Obtiene el algoritmo SHA1 seleccionado
	/// </summary>
	protected BaseTokenGenerator.HashAlgorithm GetShaAlgorithm()
	{
		if (ComboShaAlgorithms is null)
			return BaseTokenGenerator.HashAlgorithm.Sha1;
		else
			return (BaseTokenGenerator.HashAlgorithm) (ComboShaAlgorithms.SelectedId ?? (int) BaseTokenGenerator.HashAlgorithm.Sha1);
	}

	/// <summary>
	///		Actualiza el código
	/// </summary>
	protected void UpdateCode()
	{
		Error = string.Empty;
		if (string.IsNullOrWhiteSpace(Key))
		{
			HotpCode = string.Empty;
			TotpCode = string.Empty;
		}
		else
		{
			UpdateHotpCode(Key);
			UpdateTotpCode(Key);
		}
	}

	/// <summary>
	///		Calcula el código Hotp
	/// </summary>
	private void UpdateHotpCode(string key)
	{
		try
		{
			HotpGenerator hotpGenerator = new(key, GetEncoding(), GetShaAlgorithm(), Digits);

				HotpCode = hotpGenerator.Compute(Counter);
		}
		catch (Exception exception)
		{
			HotpCode = string.Empty;
			Error = $"Error ({exception.Message})";
		}
	}

	/// <summary>
	///		Calcula el código Totp
	/// </summary>
	private void UpdateTotpCode(string key)
	{
		try
		{
			TotpGenerator totpGenerator = new(key, GetEncoding(), GetShaAlgorithm(), Digits);

				TotpCode = totpGenerator.Compute();
		}
		catch (Exception exception)
		{
			TotpCode = string.Empty;
			Error = $"Error ({exception.Message})";
		}
	}

	/// <summary>
	///		Modifica el temporizador
	/// </summary>
	private void UpdateCodeTimer()
	{
		// Vacía el contador de tiempo restante
		RemainingTime = string.Empty;
		RemainingTimeColor = MvvmColor.Black;
		// Si se puede calcular un código, se obtiene el tiempo pendiente
		if (!string.IsNullOrWhiteSpace(Key) && string.IsNullOrWhiteSpace(Error))
		{
			OneTimePassword.TimeTools.TotpTimeManager timeManager = new()
																		{
																			IntervalSeconds = Interval
																		};
			int remaining = timeManager.GetRemainingSeconds();

				// Muestra el tiempo
				RemainingTime = $"{remaining:#,##0}";
				// Actualiza el código si cambia la ventana de tiempo
				if (remaining > _previousRemaining)
				{
					UpdateCode();
					_previousRemaining = -1;
				}
				else
					_previousRemaining = remaining;
				// Actualiza el color dependiendo del tiempo restante
				UpdateColorTimer(remaining);
		}
	}

	/// <summary>
	///		Modifica el color en el que se muestra el tiempo restante
	/// </summary>
	private void UpdateColorTimer(int remaining)
	{
		if (Interval != 0)
		{
			double percent = remaining * 100 / Interval;

				if (percent < 30)
					RemainingTimeColor = MvvmColor.Red;
				else if (percent < 60)
					RemainingTimeColor = MvvmColor.Green;
				else
					RemainingTimeColor = MvvmColor.Navy;
		}
		else
			RemainingTimeColor = MvvmColor.Black;
	}

	/// <summary>
	///		Comprueba los datos
	/// </summary>
	private bool ValidateData()
	{
		bool validated = false;

			// Comprueba los datos
			if (!string.IsNullOrWhiteSpace(Error))
				MainViewModel.MainController.ShowMessage("Check the window errors");
			else if (string.IsNullOrWhiteSpace(Name))
				MainViewModel.MainController.ShowMessage("Enter the provider name");
			else if (string.IsNullOrWhiteSpace(Key))
				MainViewModel.MainController.ShowMessage("Enter the provider secret");
			else
				validated = true;
			// Devuelve el valor que indica si los datos son correctos
			return validated;
	}

	/// <summary>
	///		Graba los datos
	/// </summary>
	private void Save()
	{
		if (ValidateData())
		{
			// Asigna los datos
			OtpProperties.Name = Name;
			OtpProperties.Key = Key;
			OtpProperties.Encoding = GetEncoding();
			OtpProperties.HashAlgorithm = GetShaAlgorithm();
			OtpProperties.Digits = Digits;
			OtpProperties.Counter = Counter;
			OtpProperties.Interval = Interval;
			// Lanza el evento de cierre
			RaiseEventClose(true);
		}
	}

	/// <summary>
	///		Cancela las modificaciones
	/// </summary>
	private void Cancel()
	{
		RaiseEventClose(false);
	}

	/// <summary>
	///		Lanza el evento <see cref="Close"/>
	/// </summary>
	private void RaiseEventClose(bool isAccepted)
	{
		Close?.Invoke(this, new BauMvvm.ViewModels.Forms.EventArguments.EventCloseArgs(isAccepted));
	}

	/// <summary>
	///		ViewModel de la ventana principal
	/// </summary>
	public BauOtpMainViewModel MainViewModel { get; }

	/// <summary>
	///		Datos del generador
	/// </summary>
	public Models.OtpPropertyModel OtpProperties { get; }

	/// <summary>
	///		Nombre
	/// </summary>
	public string? Name
	{
		get { return _name; }
		set { CheckProperty(ref _name, value); }
	}

	/// <summary>
	///		Clave
	/// </summary>
	public string? Key
	{
		get { return _key; }
		set 
		{ 
			if (CheckProperty(ref _key, value))
				UpdateCode();
		}
	}

	/// <summary>
	///		Combo de codificación
	/// </summary>
	public ComboViewModel? ComboEncodings
	{
		get { return _comboEncodings; }
		set 
		{ 
			if (CheckObject(ref _comboEncodings, value))
				UpdateCode();
		}
	}

	/// <summary>
	///		Combo de algoritmos SHA
	/// </summary>
	public ComboViewModel? ComboShaAlgorithms
	{
		get { return _comboShaAlgorithms; }
		set 
		{ 
			if (CheckObject(ref _comboShaAlgorithms, value))
				UpdateCode(); 
		}
	}

	/// <summary>
	///		Dígitos de salida
	/// </summary>
	public int Digits
	{
		get { return _digits; }
		set 
		{ 
			if (CheckProperty(ref _digits, value))
				UpdateCode();
		}
	}

	/// <summary>
	///		Contador utilizado para generar la clave
	/// </summary>
	public long Counter
	{
		get { return _counter; }
		set 
		{ 
			if (CheckProperty(ref _counter, value))
				UpdateCode();
		}
	}

	/// <summary>
	///		Intervalo en segundos utilizado para generar la clave
	/// </summary>
	public int Interval
	{
		get { return _interval; }
		set 
		{ 
			if (CheckProperty(ref _interval, value))
				UpdateCode();
		}
	}

	/// <summary>
	///		Mensaje de error
	/// </summary>
	public string? Error
	{
		get { return _error; }
		set { CheckProperty(ref _error, value); }
	}

	/// <summary>
	///		Código generado por el algoritmo HOTP
	/// </summary>
	public string HotpCode
	{
		get { return _hotpCode; }
		set { CheckProperty(ref _hotpCode, value); }
	}

	/// <summary>
	///		Código generado por el algoritmo TOTP
	/// </summary>
	public string TotpCode
	{
		get { return _totpCode; }
		set { CheckProperty(ref _totpCode, value); }
	}

	/// <summary>
	///		Tiempo restante
	/// </summary>
	public string RemainingTime
	{
		get { return _remainingTime; }
		set { CheckProperty(ref _remainingTime, value); }
	}

	/// <summary>
	///		Color asignado al temporizador
	/// </summary>
	public MvvmColor RemainingTimeColor
	{
		get { return _remainingTimeColor; }
		set { CheckObject(ref _remainingTimeColor!, value); }
	}

	/// <summary>
	///		Comando de grabación
	/// </summary>
	public BaseCommand SaveCommand { get; }

	/// <summary>
	///		Comando de cancelación
	/// </summary>
	public BaseCommand CancelCommand { get; }
}