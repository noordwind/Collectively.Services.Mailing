namespace Coolector.Services.Mailing.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => "Welcome to the Coolector.Services.Mailing API!");
        }
    }
}