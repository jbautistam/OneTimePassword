# OneTimePassword

Implementación de los algoritmos TOTP ([RFC 6238](http://tools.ietf.org/html/rfc6238)) y HOTP [RFC 4226](http://tools.ietf.org/html/rfc4226)
para generación de contraseñas de un único uso con C#.

[![Publish package](https://github.com/jbautistam/OneTimePassword/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jbautistam/OneTimePassword/actions/workflows/dotnet.yml)

[![NuGet OneTimePassword](https://buildstats.info/nuget/Bau.Libraries.OneTimePassword)](https://www.nuget.org/packages/Bau.Libraries.OneTimePassword)

## Instalación del paquete Nuget

```bash
dotnet add package Bau.Libraries.OneTimePassword
```

## Introducción

Un algoritmo de contraseña de uso único (One Time Password u OTP) es un método de autenticación que genera una contraseña única y 
temporal para una sesión. Este tipo de contraseña va a utilizarse sólo una vez y caduca después de un breve período de tiempo. 

Este tipo de algoritmos se utilizan para agregar una capa adicional de seguridad sobre aplicaciones permitiendo la autentificación 
en dos pasos (2FA o MFA).

Características clave de las OTP:

* **Temporalidad:** tienen un tiempo limitado de validez, se pueden utilizar dentro de un breve período de tiempo después de su generación.
* **Exclusividad:** cada clave es única y se genera mediante algoritmos avanzados, garantizando que no se repita y que 
solo su dueño pueda acceder a ella.
* **Generación segura:** creadas mediante procesos criptográficos que imposibilitan la adivinación por parte de los atacantes.
* **Fácil implementación:** pueden enviarse a través de varios medios, como mensajes de texto, correos electrónicos o 
aplicaciones dedicadas, facilitando su integración en diferentes sistemas.

Existen varios algoritmos para la generación de claves OTP, esta librería genera claves utilizando HOTP o TOTP:

### Algoritmo HOTP (HMAC-Based One-Time Password) 

HOTP es un método de autenticación que genera contraseñas únicas y temporales utilizando una clave secreta compartida y un contador. 
Este algoritmo es una parte fundamental de la iniciativa OATH (Initiative for Open Authentication) y publicado como 
[RFC 4226](http://tools.ietf.org/html/rfc4226) en 2005.

**Características clave del algoritmo HOTP:**

* **Clave secreta compartida:** ambas partes (el servidor y la aplicación de autenticación) comparten una clave secreta que se utiliza para generar las contraseñas OTP.
* **Contador:** se incrementa un contador cada vez que se genera una nueva contraseña OTP. Este contador asegura que cada contraseña sea única.
* **Función HMAC:** la clave secreta y el valor del contador se procesan utilizando una función HMAC (Hash-based Message Authentication Code) para generar la contraseña OTP.
* **Generación de OTP:** la contraseña OTP se genera combinando la clave secreta y el valor del contador, y luego se trunca el resultado para ofreces un 
formato más amigable para el usuario, generalmente un número de 6 a 8 dígitos.

**Proceso de funcionamiento del algoritmo HOTP:**

* **Inicialización:** el servidor y la aplicación de autenticación acuerdan una clave secreta y un contador inicial.
* **Generación de OTP:** cuando se necesita una contraseña OTP, la aplicación de autenticación combina la clave secreta 
y el valor actual del contador y aplica la función HMAC para generar una contraseña OTP.
* **Truncamiento:** la salida de la función HMAC se trunca para obtener una contraseña OTP de 6 a 8 dígitos.
* **Incremento del contador:** tras usar la contraseña OTP, el contador se incrementa en una unidad tanto en el servidor como en la aplicación de autenticación.
* **Autenticación:** el servidor verifica la contraseña OTP proporcionada por el usuario generando una contraseña OTP localmente con la clave secreta y el 
contador actual, y comparándola con la proporcionada por el usuario.

**Ventajas del algoritmo HOTP:**

* **Seguridad:** las contraseñas OTP son únicas y temporales, lo que las hace más seguras que las contraseñas estáticas.
* **Flexibilidad:** puede utilizarse en una variedad de aplicaciones, incluyendo autenticación en línea y transacciones financieras.
 
**Desventajas del algoritmo HOTP:**

* **Problema de sincronización:** Si los contadores del servidor y la aplicación de autenticación se desincronizan, puede requerir un protocolo
de resincronización para resolver el problema.
* **No tiene período de expiración:** Las contraseñas OTP generadas por HOTP son válidas hasta que se utilice la siguiente, lo que puede 
ser un problema de seguridad si no se utiliza inmediatamente.

### Algoritmo TOTP (Time-Based One-Time Password) 

TOTP es un método de autenticación que genera contraseñas únicas y temporales basadas en la clave secreta compartida y la 
hora actual. Este algoritmo es una variante del algoritmo HOTP (HMAC-Based One-Time Password) y se utiliza ampliamente 
en la autenticación de dos factores (2FA).

**Características clave del algoritmo TOTP:**

* **Clave secreta compartida:** ambas partes (el servidor y la aplicación de autenticación) comparten una clave secreta que 
se utiliza para generar las contraseñas OTP.
* **Hora actual:** la hora actual se utiliza como variable para generar la contraseña OTP, lo que asegura que cada contraseña sea única y temporal.
* **Función HMAC:** La clave secreta y la hora actual se procesan utilizando una función HMAC (Hash-based Message Authentication Code) 
para generar la contraseña OTP.
* **Intervalo de tiempo:** el algoritmo utiliza un intervalo de tiempo (generalmente 30 segundos) para determinar la validez de la contraseña OTP.

**Proceso de funcionamiento del algoritmo TOTP:**

* **Inicialización:** el servidor y la aplicación de autenticación acuerdan una clave secreta y un tiempo inicial. Lo normal es utilizar la constante EPOCH utilizada
para las fechas en Unix: número de ticks desde el 1-1-1970.
* **Generación de OTP:** cuando se necesita una contraseña OTP, la aplicación de autenticación combina la clave secreta y la hora actual
y aplica la función HMAC para generar una contraseña OTP.
* **Truncamiento:** la salida de la función HMAC se trunca para obtener una contraseña OTP de 6 a 8 dígitos.
* **Verificación:** el servidor verifica la contraseña OTP proporcionada por el usuario generando una contraseña OTP localmente 
con la clave secreta y la hora actual y comparándola con la proporcionada por el usuario.

**Ventajas del algoritmo TOTP:**

* **Seguridad:** las contraseñas OTP son únicas y temporales, lo que las hace más seguras que las contraseñas estáticas.
* **Disponibilidad sin conexión:** las contraseñas OTP se pueden generar sin conexión a Internet, lo que las hace ideales 
para usuarios que necesitan acceder a sus cuentas en áreas con poca o ninguna conectividad.
* **Usabilidad:** el algoritmo TOTP es fácil de implementar y utilizar.

**Desventajas del algoritmo TOTP:**

* **Problema de sincronización:** si los relojes del servidor y la aplicación de autenticación no están sincronizados, 
puede requerir un protocolo de resincronización para resolver el problema.
* **Limitaciones de seguridad:** aunque las contraseñas OTP son temporales, un atacante podría interceptar y utilizar una 
contraseña OTP antes de que expire si tiene acceso a la clave secreta.

## Utilización de la librería

### Generación de una clave HOTP

Para generar una clave HOTP utilizaremos la clase `HotpGenerator`. En el constructor debemos indicar:

* `Key`: clave devuelta por el servidor de claves.
* `Encoding`: modo de codificación de la clave devuelta por el servidor de claves (texto plano, Base64 o Base32).
* `Algorithm`: algoritmo de hashing utilizado para obtener los códigos resultantes (Sha1, Sah128, Sha256). El valor habitual es SHA1.
* `Digits`: número de caracteres generados por el código (de 6 a 8, lo habitual es 6).

Una vez inicializa la clase, simplemente llamando al método `Compute` con el contador adecuado para obtener el código de validación:

```csharp
using Bau.Libraries.OneTimePassword;

HotpGenerator hotp = new("KEY", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 6);

string code = hotp.Compute(19238);
```

### Generación de una clave TOTP

Para generar una clave tOTP utilizaremos la clase `TotpGenerator`. En el constructor debemos indicar:

* `Key`: clave devuelta por el servidor de claves.
* `Encoding`: modo de codificación de la clave devuelta por el servidor de claves (texto plano, Base64 o Base32).
* `Algorithm`: algoritmo de hashing utilizado para obtener los códigos resultantes (Sha1, Sah128, Sha256). El valor habitual es SHA1.
* `Digits`: número de caracteres generados por el código (de 6 a 8, lo habitual es 6).

Una vez inicializa la clase, simplemente llamando al método `Compute` para obtener el código de validación:

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

como un valor `long` especificando la fecha Unix (número de ticks desde el 1-1-1970):

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

### Tiempo de validez de la clave

Los códigos generados por `TotpGenerator` son válidos durante treinta segundos o el intervalo especificado, pero este
tiempo se mide no desde la generación si no desde el inicio del intervalo, es decir, si generamos el código a las 12:05, el
inicio de la ventana de generación será 12:00 y nos quedarán veinticinco segundos de validez.

Para comprobar el tiempo que nos queda de validez del código por ejemplo para mostrarlo en una aplicación, podemos utilizar
el método `GetRemainingSeconds` de clase `TopTimeManager` donde se agrupan los métodos relacionados con la fecha:

```c#
using Bau.Libraries.OneTimePassword;

TotpGenerator totp = new("KEY", Secret.Encoding.Plain, BaseTokenGenerator.HashAlgorithm.Sha1, 6);

totp.TimeManager.IntervalSeconds = 60;

string code = totp.Compute(DateTime.UtcNow);
int remainingSeconds = totp.TimeManager.GetRemainingSeconds(DateTime.UtcNow);
```

## Créditos

Este proyecto se basa en [Otp.Net](https://github.com/kspearrin/Otp.NET) desarrollado por [Kyle Spearrin](https://github.com/kspearrin).