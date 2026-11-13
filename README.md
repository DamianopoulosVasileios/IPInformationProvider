#IPInformationProvider

Built an ASP.NET Core Minimal API that looks up the country associated with an IP address. It checks an in-memory cache and SQL Server before calling the external IP2C service, then stores new lookup results using EF Core. Added an hourly background service that rechecks stored IP addresses in batches and refreshes the cache.

