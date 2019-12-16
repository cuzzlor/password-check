# password-check

`IBreachedPasswordService` [checks HIBP via the range search](https://haveibeenpwned.com/API/v3#SearchingPwnedPasswordsByRange) to find number of times a password has appeared in a known breach.

## Install 

```
Install-Package password-check
```

## Configure

`BreachedPasswordService` can be set up as a singleton. It requires an `IHttpClientFactory` that can create an `HttpClient` named `hibp-range`.

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IBreachedPasswordService, BreachedPasswordService>();
    services.AddHttpClient("hibp-range", client =>
    {
        client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
    });
}
```

## Use

You can then use `IBreachedPasswordService` to check passwords.

```cs
var breachCount = await _breachedPasswordService.GetBreachCountAsync(password);
```
