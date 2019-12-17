# password-check

`IBreachedPasswordService` [checks HIBP via the range search](https://haveibeenpwned.com/API/v3#SearchingPwnedPasswordsByRange) to find number of times a password has appeared in a known breach.

## Install 

```
Install-Package password-check
```

## Configure

The simplest configuratation is to use the `AddBreachedPasswordService` extension method:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddBreachedPasswordService();
}
```

Or if you want greater control over the HttpClient used by `BreachedPasswordService`:

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
