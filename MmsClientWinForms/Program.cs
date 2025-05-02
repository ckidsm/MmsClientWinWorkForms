using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MmsClientWinForms;
using MmsClientWinForms.Services;

namespace MmsClientWinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

         var host = Host.CreateDefaultBuilder()
      .ConfigureServices(services =>
      {
         services.AddHttpClient("ProductSerial", client =>
         {
            client.BaseAddress = new Uri("https://192.168.0.4:5001");
         })
       .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
          {
             ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
          });

         services.AddSingleton<ProductSerialService>();
         services.AddSingleton<Form1>(); 
      })
      .Build();

         var mainForm = host.Services.GetRequiredService<Form1>();
         Application.Run(mainForm);

         // Application.Run(new FormMain());
      }
   }
}