- `IBreachedPasswordService` [calls the HIBP (haveibeenpwned.com) range search API](https://haveibeenpwned.com/API/v3#SearchingPwnedPasswordsByRange) and returns the number of times a password has appeared in a known data breach.
- `IForbiddenPasswordService` uses locally configured (via config or text file) regex patterns to check for forbidden passwords.

## Install 

```
Install-Package PasswordCheck
```

## IBreachedPasswordService

### DI Setup

The simplest way is to use the `AddBreachedPasswordService` extension method:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddBreachedPasswordService();
}
```

Alternatively, if you want greater control over configuring the `HttpClient` used by `BreachedPasswordService`, configure the services directly:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IBreachedPasswordService, BreachedPasswordService>();
    services.AddHttpClient<IBreachedPasswordService, BreachedPasswordService>(client =>
    {
        client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
    }).AddPolicyHandler(GetHibpHttpPolicy());
}
```

### Use

You can then use `IBreachedPasswordService` to find out how many times the password has appeared in known breaches.

```cs
var breachCount = await _breachedPasswordService.GetBreachCountAsync(password);
```

## IForbiddenPasswordService

### File Based Patterns

You can load up a text file with regex patterns:

```
p(a|@)(s|5)(s|5)w(o|0)rd
f(o|0)r(b|8)(i|1)dd(e|3)n
```

Provide the file name via the config key `ForbiddenPasswordOptions:ForbiddenPasswordsFile`.

>Make sure you set the `CopyToOutputDirectory` attribute for the text file.

### Patterns via Configuration

You can add regex patterns to config via the key `ForbiddenPasswordOptions:ForbiddenPasswords`, e.g.:

```json
{
    "ForbiddenPasswordOptions": {
        "ForbiddenPasswords": [
            "p(a|@)(s|5)(s|5)w(o|0)rd",
            "f(o|0)r(b|8)(i|1)dd(e|3)n"
        ]
    }
}
```

### DI Setup

The simplest way is to use the `AddForbiddenPasswordService` extension method. This will bind configuration from the `ForbiddenPasswordOptions` config key:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddForbiddenPasswordService();
}
```

Alternatively, if you want greater control over configuring the `ForbiddenPasswordOptions` used by `ForbiddenPasswordService`, configure the service and options directly:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IForbiddenPasswordService, ForbiddenPasswordService>();
    services.AddOptions<ForbiddenPasswordOptions>().Bind(configuration.GetSection("MyCustomForbiddenPasswordOptionsKey"));
}
```

### Use

```cs
var isPasswordForbidden = _forbiddenPasswordService.IsPasswordForbidden("password123");
```
