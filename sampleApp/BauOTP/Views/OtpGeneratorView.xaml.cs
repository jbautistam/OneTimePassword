using System.Windows;

using Bau.Libraries.BauOTP.ViewModels.Generators;

namespace Bau.Libraries.BauOTP.Views;

/// <summary>
///		Ventana de <see cref="GeneratorViewModel"/>
/// </summary>
public partial class OtpGeneratorView : Window
{
	public OtpGeneratorView(GeneratorViewModel viewModel)
	{
		InitializeComponent();
		DataContext = ViewModel = viewModel;
		viewModel.Close += (sender, result) =>
										{
											DialogResult = result.IsAccepted;
											Close();
										};
	}

	/// <summary>
	///		ViewModel
	/// </summary>
	public GeneratorViewModel ViewModel { get; }
}
