:: --------------------------------------------
:: Start up the sass compiler
:: --------------------------------------------

cd C:\xampp\htdocs\files\deadit\src\dotnet\Deadit\Deadit.WebGui\wwwroot\css

::sass --watch custom/style.scss dist/custom/style.css
sass --watch custom/style.scss:dist/style.css 

