> `IBreachedPasswordService` [calls the HIBP (haveibeenpwned.com) range search API](https://haveibeenpwned.com/API/v3#SearchingPwnedPasswordsByRange) and returns the number of times a password has appeared in a known data breach.

## Install 

```
Install-Package PasswordCheck
```

## Configure

The simplest configuratation is to use the `AddBreachedPasswordService` extension method:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddBreachedPasswordService();
}
```

Alternatively, if you want greater control over configuring the `HttpClient` used by `BreachedPasswordService`:

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

## Use

You can then use `IBreachedPasswordService` to check passwords.

```cs
var breachCount = await _breachedPasswordService.GetBreachCountAsync(password);
```
