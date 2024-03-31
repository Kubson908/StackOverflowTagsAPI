<div align="center">
  <h1 align="center">StackOverflowTagsAPI</h1>

  <p align="center">
    REST API oparte o listę tagów pobranych z API Stack Overflow
  </p>
</div>
<br />

## O projekcie
<div align="justify"><p>API zostało napisane w .NET 8 przy wykorzystaniu kontenerów Docker w celu zapewnienia powtarzalnego budowania i uruchamiania projektu.
Pobrane tagi przechowywane są w bazie danych PostgreSQL. Po uruchomieniu, system automatycznie pobiera na nowo zestawienie 1000 tagów 
do bazy danych, o ile liczba istniejących tagów w bazie jest mniejsza od tego limitu. Po pobraniu dla każdego tagu obliczany jest jego
procentowy udział w całej populacji.</p></div>
REST API udostępnia następujące metody:
<ul>
<li>pobranie tagów z bazy poprzez stronicowanie z opcją sortowania po nazwie i udziale malejąco lub rosnąco</li>
<li>pobranie tagu z bazy na podstawie jego id</li>
<li>wymuszenie ponownego pobrania tagów z API Stack Overflow na podstawie podanych parametrów</li>
</ul>

## Instrukcja uruchomienia
Aby uruchomić API należy włączyć klienta docker i następnie za za pomocą wiersza poleceń w folderze StackOverflowTags.api wywołać polecenie
```docker compose up```

## Wykorzystane technologie

[![Dotnet][DotnetCore]][Dotnet-url]
[![Postgresql][Postgres]][Postgres-url]
[![Docker][Docker]][Docker-url]



[DotnetCore]: https://img.shields.io/badge/.NET_8-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[Dotnet-url]: https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0

[Postgres]: https://img.shields.io/badge/postgresql-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white
[Postgres-url]: https://www.postgresql.org/docs/

[Docker]: https://img.shields.io/badge/Docker-008BB9?style=for-the-badge&logo=docker&logoColor=white
[Docker-url]: https://docs.docker.com

