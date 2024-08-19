# OneTimePassword

Implementaci�n de los algoritmos TOTP ([RFC 6238](http://tools.ietf.org/html/rfc6238)) y HOTP [RFC 4226](http://tools.ietf.org/html/rfc4226)
para generaci�n de contrase�as de un �nico uso con C#.

[![Publish package](https://github.com/jbautistam/OneTimePassword/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jbautistam/OneTimePassword/actions/workflows/dotnet.yml)

## Instalaci�n del paquete Nuget

```bash
dotnet add package Bau.Libraries.OneTimePassword
```

https://www.nuget.org/packages/Bau.Libraries.OneTimePassword

## Introducci�n

Un algoritmo de contrase�a de uso �nico (One Time Password u OTP) es un m�todo de autenticaci�n que genera una contrase�a �nica y 
temporal para una sesi�n. Este tipo de contrase�a va a utilizarse una sola vez y caduca despu�s de un breve per�odo de tiempo. 

Este tipo de algoritmos se utilizan para agregar una capa adicional de seguridad en diversas aplicaciones permitiendo la autentificaci�n 
en dos pasos (2FA o MFA).

Caracter�sticas clave de las OTP:

* **Temporalidad:** tienen un tiempo limitado de validez, se pueden utilizar dentro de un breve per�odo de tiempo despu�s de su generaci�n.
* **Exclusividad:** cada clave es �nica y se genera mediante algoritmos avanzados, garantizando que no se repita y que 
* solo sea accesible para su due�o.
* **Generaci�n segura:** creadas mediante procesos criptogr�ficos que imposibilita la predicci�n para los atacantes.
* **F�cil implementaci�n:** pueden enviarse a trav�s de varios medios, como mensajes de texto, correos electr�nicos o 
aplicaciones dedicadas, facilitando su integraci�n en diferentes sistemas.

Existen varios algoritmos para la generaci�n de claves OTP, esta librer�a genera claves utilizando HOTP o TOTP:

### Algoritmo HOTP (HMAC-Based One-Time Password) 

HOTP es un m�todo de autenticaci�n que genera contrase�as �nicas y temporales utilizando una clave secreta compartida y un contador. 
Este algoritmo es una parte fundamental de la iniciativa OATH (Initiative for Open Authentication) y publicado como 
[RFC 4226](http://tools.ietf.org/html/rfc4226) en 2005.

**Caracter�sticas clave del algoritmo HOTP:**

* **Clave secreta compartida:** Ambas partes (el servidor y el dispositivo de autenticaci�n) comparten una clave secreta que se utiliza para generar las contrase�as OTP.
* **Contador:** Un contador se incrementa cada vez que se genera una nueva contrase�a OTP. Este contador asegura que cada contrase�a OTP sea �nica.
* **Funci�n HMAC:** La clave secreta y el valor del contador se procesan utilizando una funci�n HMAC (Hash-based Message Authentication Code) para generar la contrase�a OTP.
* **Generaci�n de OTP:** La contrase�a OTP se genera combinando la clave secreta y el valor del contador, y luego truncando el resultado en un formato m�s amigable para el usuario, generalmente un n�mero de 6 a 8 d�gitos.

**Proceso de funcionamiento del algoritmo HOTP:**

* **Inicializaci�n:** El servidor y el dispositivo de autenticaci�n acuerdan una clave secreta y un contador inicial.
* **Generaci�n de OTP:** Cuando se necesita una contrase�a OTP, el dispositivo de autenticaci�n combina la clave secreta y el valor del contador actual, y aplica la funci�n HMAC para generar una contrase�a OTP.
* **Truncamiento:** La salida de la funci�n HMAC se trunca para obtener una contrase�a OTP de 6 a 8 d�gitos.
* **Incremento del contador:** Despu�s de usar la contrase�a OTP, el contador se incrementa en una unidad tanto en el servidor como en el dispositivo de autenticaci�n.
* **Autenticaci�n:** El servidor verifica la contrase�a OTP proporcionada por el usuario generando una contrase�a OTP localmente con la clave secreta y el contador actual, y compar�ndola con la proporcionada por el usuario.

**Ventajas del algoritmo HOTP:**
	* **Seguridad:** Las contrase�as OTP son �nicas y temporales, lo que las hace m�s seguras que las contrase�as est�ticas.
	* **Flexibilidad:** Puede ser utilizado en una variedad de aplicaciones, incluyendo autenticaci�n en l�nea y transacciones financieras.
**Desventajas del algoritmo HOTP:**
	* **Problema de sincronizaci�n:** Si los contadores del servidor y el dispositivo de autenticaci�n se desincronizan, puede requerir un protocolo de resincronizaci�n para resolver el problema.
	* **No tiene per�odo de expiraci�n:** Las contrase�as OTP generadas por HOTP permanecen v�lidas hasta que se utilice la siguiente, lo que puede ser un problema de seguridad si no se utiliza inmediatamente.

### Algoritmo TOTP (Time-Based One-Time Password) 

TOTP es un m�todo de autenticaci�n que genera contrase�as �nicas y temporales basadas en la clave secreta compartida y la 
hora actual. Este algoritmo es una variante del algoritmo HOTP (HMAC-Based One-Time Password) y se utiliza ampliamente 
en la autenticaci�n de dos factores (2FA).

**Caracter�sticas clave del algoritmo TOTP:**

* **Clave secreta compartida:** ambas partes (el servidor y el dispositivo de autenticaci�n) comparten una clave secreta que 
se utiliza para generar las contrase�as OTP.
* **Hora actual:** la hora actual se utiliza como variable para generar la contrase�a OTP, lo que asegura que cada contrase�a sea �nica y temporal.
* **Funci�n HMAC:** La clave secreta y la hora actual se procesan utilizando una funci�n HMAC (Hash-based Message Authentication Code) 
para generar la contrase�a OTP.
* **Intervalo de tiempo:** el algoritmo utiliza un intervalo de tiempo (generalmente 30 segundos) para determinar la validez de la contrase�a OTP.

**Proceso de funcionamiento del algoritmo TOTP:**

* **Inicializaci�n:** el servidor y el dispositivo de autenticaci�n acuerdan una clave secreta y un tiempo inicial (generalmente el tiempo Unix).
* **Generaci�n de OTP:** cuando se necesita una contrase�a OTP, el dispositivo de autenticaci�n combina la clave secreta y la hora actual
y aplica la funci�n HMAC para generar una contrase�a OTP.
* **Truncamiento:** La salida de la funci�n HMAC se trunca para obtener una contrase�a OTP de 6 a 8 d�gitos.
* **Verificaci�n:** El servidor verifica la contrase�a OTP proporcionada por el usuario generando una contrase�a OTP localmente 
con la clave secreta y la hora actual, y compar�ndola con la proporcionada por el usuario.

**Ventajas del algoritmo TOTP:**
* **Seguridad:** las contrase�as OTP son �nicas y temporales, lo que las hace m�s seguras que las contrase�as est�ticas.
* **Disponibilidad sin conexi�n:** las contrase�as OTP se pueden generar sin conexi�n a Internet, lo que las hace ideales 
para usuarios que necesitan acceder a sus cuentas en �reas con poca o ninguna conectividad.
* **Usabilidad:** el algoritmo TOTP es f�cil de implementar y utilizar, especialmente con aplicaciones de autenticaci�n como 
Authy y Google Authenticator.

**Desventajas del algoritmo TOTP:**
* **Problema de sincronizaci�n:** si los relojes del servidor y el dispositivo de autenticaci�n no est�n sincronizados, 
puede requerir un protocolo de resincronizaci�n para resolver el problema.
* **Limitaciones de seguridad:** aunque las contrase�as OTP son temporales, un atacante podr�a interceptar y utilizar una 
contrase�a OTP antes de que expire si tiene acceso a la clave secreta.

## Utilizaci�n de la librer�a

### Generaci�n de una clave HOTP

Para generar una clave HOTP utilizaremos la clase `HotpGenerator`. En el constructor debemos indicar:

* `Key`: clave devuelta por el servidor de claves.
* `Encoding`: modo de codificaci�n de la clave devuelta por el servidor de claves (texto plano, Base64 o Base32).
* `Algorithm`: algoritmo de hashing utilizado para obtener los c�digos resultantes (Sha1, Sah128, Sha256). El valor habitual es SHA1.
* `Digits`: n�mero de caracteres generados por el c�digo (de 6 a 8, lo habitual es 6).

Una vez inicializa la clase, simplemente llamando al m�todo `Compute` con el contador, obtendremos el c�digo de validaci�n:

```csharp
using Bau.Libraries.OneTimePassword;

HotpGenerator hotp = new("KEY", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 6);

string code = hotp.Compute(19238);
```

### Generaci�n de una clave TOTP

Para generar una clave tOTP utilizaremos la clase `TotpGenerator`. En el constructor debemos indicar:

* `Key`: clave devuelta por el servidor de claves.
* `Encoding`: modo de codificaci�n de la clave devuelta por el servidor de claves (texto plano, Base64 o Base32).
* `Algorithm`: algoritmo de hashing utilizado para obtener los c�digos resultantes (Sha1, Sah128, Sha256). El valor habitual es SHA1.
* `Digits`: n�mero de caracteres generados por el c�digo (de 6 a 8, lo habitual es 6).

Una vez inicializa la clase, simplemente llamando al m�todo `Compute` para obtener el c�digo de validaci�n:

```csharp
using Bau.Libraries.OneTimePassword;

TotpGenerator totp = new("KEY", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 6);

string code = totp.Compute();
```

En este caso, si no le pasamos ninguna fecha, se utiliza la fecha del sistema pero le podemos pasar tanto una fecha en
concreto como `DateTime`:

```csharp
string code = totp.Compute(new DateTime(2024, 8, 2, 17, 30, 5));
```

como un valor `long` especificando la fecha Unix (n�mero de ticks desde el 1-1-1970):

```csharp
string code = totp.Compute(1_991_289);
```

Inicialmente, el tiempo de validez de la clave, es de 30 segundos pero lo podemos modificar en cualquier momento:

```csharp
using Bau.Libraries.OneTimePassword;

TotpGenerator totp = new("KEY", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 6);

totp.TimeManager.IntervalSeconds = 60;

string code = totp.Compute();
```

#### Remaining Time

There is a method that will tell you how much time remains in the current time step window in seconds.

```c#
var remainingTime = totp.RemainingSeconds();
// there is also an overload that lets you specify the time
var remainingSeconds = totp.RemainingSeconds(DateTime.UtcNow);
```

## Cr�ditos

Este proyecto se basa en [Otp.Net](https://github.com/kspearrin/Otp.NET) desarrollado por [](https://github.com/kspearrin).