using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.BauOTP.ViewModels.Models;
using Bau.Libraries.OneTimePassword;

namespace Bau.Libraries.BauOTP.ViewModels.Repositories;

/// <summary>
///		Repositorio de <see cref="OtpPropertyModel"/>
/// </summary>
internal class OtpPropertiesRepository
{
	// Constantes privadas
	private const string TagRoot = "OtpFiles";
	private const string TagOtp = "Otp";
	private const string TagName = "Name";
	private const string TagKey = "Key";
	private const string TagEncoding = "Encoding";
	private const string TagHashAlgorithm = "HashAlgorithm";
	private const string TagDigits = "Digits";
	private const string TagCounter = "Counter";
	private const string TagInterval = "Interval";

	/// <summary>
	///		Carga los proveedores de un archivo
	/// </summary>
	internal List<OtpPropertyModel> Load(string fileName)
	{
		List<OtpPropertyModel> otpProperties = new();
		MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

			// Carga el archivo
			if (fileML is not null)
				foreach (MLNode rootML in fileML.Nodes)
					if (rootML.Name == TagRoot)
						foreach (MLNode nodeML in rootML.Nodes)
							if (nodeML.Name == TagOtp)
								otpProperties.Add(LoadOtp(nodeML));
			// Devuelve las propiedades
			return otpProperties;
	}
	
	/// <summary>
	///		Carga los datos de un <see cref="OtpPropertyModel"/>
	/// </summary>
	private OtpPropertyModel LoadOtp(MLNode rootML)
	{
		OtpPropertyModel otpProperty = new();
								
			// Asigna los datos
			otpProperty.Name = rootML.Nodes[TagName].Value.TrimIgnoreNull();
			otpProperty.Key = rootML.Nodes[TagKey].Value.TrimIgnoreNull();
			otpProperty.Encoding = rootML.Attributes[TagEncoding].Value.GetEnum(Secret.Encoding.Plain);
			otpProperty.HashAlgorithm = rootML.Attributes[TagHashAlgorithm].Value.GetEnum(BaseTokenGenerator.HashAlgorithm.Sha1);
			otpProperty.Digits = rootML.Attributes[TagDigits].Value.GetInt(6);
			otpProperty.Counter = rootML.Attributes[TagCounter].Value.GetLong(0);
			otpProperty.Interval = rootML.Attributes[TagInterval].Value.GetInt(30);
			// Devuelve los datos del objeto
			return otpProperty;
	}

	/// <summary>
	///		Graba los proveedores en un archivo
	/// </summary>
	internal void Save(List<OtpPropertyModel> otpProperties, string fileName)
	{
		MLFile fileML = new();
		MLNode rootML = fileML.Nodes.Add(TagRoot);

			// Añade los datos de las propiedades
			foreach (OtpPropertyModel otpProperty in otpProperties)
				rootML.Nodes.Add(GetNode(otpProperty));
			// Graba los datos del archivo
			new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
	}

	/// <summary>
	///		Obtiene los datos de un nodo
	/// </summary>
	private MLNode GetNode(OtpPropertyModel otpProperty)
	{
		MLNode rootML = new(TagOtp);

			// Asigna los datos
			rootML.Nodes.Add(TagName, otpProperty.Name);
			rootML.Nodes.Add(TagKey, otpProperty.Key);
			rootML.Attributes.Add(TagEncoding, (int) otpProperty.Encoding);
			rootML.Attributes.Add(TagHashAlgorithm, (int) otpProperty.HashAlgorithm);
			rootML.Attributes.Add(TagDigits, otpProperty.Digits);
			rootML.Attributes.Add(TagCounter, otpProperty.Counter);
			rootML.Attributes.Add(TagInterval, otpProperty.Interval);
			// Devuelve el nodo
			return rootML;
	}
}
