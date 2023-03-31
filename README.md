# ThreadingInC#

Binnen dit project is een simulatie ontwikkeld waarin de techniek threading is verwerkt. Daarbij zijn de volgende code conventies gebruikt: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions#commenting-conventions. Hieronder wordt het gehele installatieproces van dit project beschreven.



<h2>Configuratie</h2>

Om de applicatie te kunnen starten zijn er een aantal stappen die doorlopen moeten worden. Ten eerste zal Microsoft Visual Studio 2022 moeten worden geïnstalleerd. Hierbij is het van belang dat de volgende workloads aanwezig zijn:

- Universal Windows Platform development
- .NET desktop development

Wanneer de bovenstaande workloads aanwezig zijn, kan de database worden opgezet. In deze repository is een database aanwezig met de naam "lifethreadening.sql". Voordat deze kan worden gebruikt, zal eerst Micosoft SQL Server Management Studio moeten worden geïnstalleerd, waarna het script in het SQL-bestand (lifethreadening.sql) kan worden uitgevoerd in dit programma. Nu de database is geïmporteerd, zal er eerst nog een vervolgstap plaatsvinden voordat de applicatie kan verbinden met de betreffende database. 





Bovendien zullen de volgende NuGet packages binnen het project aanwezig moeten zijn:

- FontAwesome.WPF - charri, versie 4.7.0.9
- Microsoft.NETCore.UniversalWindowsPlatform - Microsoft, versie 6.2.14
- Microsoft.Toolkit.Uwp - Microsoft.Toolkit, versie 7.1.3
- Microsoft.Toolkit.Uwp.UI - Microsoft.Toolkit, versie 7.1.3
- Microsoft.Toolkit.Uwp.UI.Controls - Microsoft.Toolkit, versie 7.1.3
- System.Text.Json - Microsoft, versie 7.0.2
- WinRTXamlToolkit.Controls.DataVisualization.UWP - Filip Skakun & Mahmoud Moussa & Silverlight Toolkit Team, versie 2.3.0



Wanneer alle bovenstaande stappen zijn doorlopen kan de applicatie worden opgestart binnen Visual Studio.

