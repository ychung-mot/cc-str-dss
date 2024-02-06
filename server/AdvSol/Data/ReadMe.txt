dotnet tool install --global dotnet-ef
cd AdvSol
dotnet ef migrations add InitialCreate
dotnet ef database update