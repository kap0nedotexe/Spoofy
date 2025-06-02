using Microsoft.Win32;
using Spectre.Console;
using System;
using System.Management;
using System.Security.Principal;
using System.Threading;

namespace Spoofy
{
    internal static class Program
    {
        private static readonly string creditsText = "https://cracked.sh/Kap0ne / https://voided.st/Kap0ne";
        private static readonly string fancyText = "\r\n\r\n██████████████████████████████████████████████████████████████████████████\r\n█▌                                                                      ▐█\r\n█▌   ██████     ██▓███      ▒█████      ▒█████       █████▒   ▓██   ██▓ ▐█\r\n█▌ ▒██    ▒    ▓██░  ██▒   ▒██▒  ██▒   ▒██▒  ██▒   ▓██   ▒     ▒██  ██▒ ▐█\r\n█▌ ░ ▓██▄      ▓██░ ██▓▒   ▒██░  ██▒   ▒██░  ██▒   ▒████ ░      ▒██ ██░ ▐█\r\n█▌   ▒   ██▒   ▒██▄█▓▒ ▒   ▒██   ██░   ▒██   ██░   ░▓█▒  ░      ░ ▐██▓░ ▐█\r\n█▌ ▒██████▒▒   ▒██▒ ░  ░   ░ ████▓▒░   ░ ████▓▒░   ░▒█░         ░ ██▒▓░ ▐█\r\n█▌ ▒ ▒▓▒ ▒ ░   ▒▓▒░ ░  ░   ░ ▒░▒░▒░    ░ ▒░▒░▒░     ▒ ░          ██▒▒▒  ▐█\r\n█▌ ░ ░▒  ░ ░   ░▒ ░          ░ ▒ ▒░      ░ ▒ ▒░     ░          ▓██ ░▒░  ▐█\r\n█▌ ░  ░  ░     ░░          ░ ░ ░ ▒     ░ ░ ░ ▒      ░ ░        ▒ ▒ ░░   ▐█\r\n█▌       ░                     ░ ░         ░ ░                 ░ ░      ▐█\r\n█▌                                                             ░ ░      ▐█\r\n█▌                                                                      ▐█\r\n██████████████████████████████████████████████████████████████████████████\r\n\r\n";

        [STAThread]
        private static void Main()
        {
            WindowUtility.SetConsoleWindowPosition(WindowUtility.AnchorWindow.Center);

            Console.Title = "[ Spoofy ] ~ AIO HWID Spoofer ~ Made By: Kap0ne";

            DisplayWelcome();

            if (!IsAdministrator())
            {
                AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Spoofy requires admin privileges to modify system identifiers! Please run as admin and try again.[/]\n");
                AnsiConsole.Markup("[#5a47c6][[->]] [/][white]Press any key to exit...[/]");
                Console.ReadKey();

                return;
            }

            while (true)
            {
                var selection = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("[#5a47c6][[!]] [/][white]Select an option:[/]")
                    .PageSize(10)
                    .AddChoices(new[] { "  [white]HWID[/]", "  [white]Machine GUID[/]", "  [white]Computer Name[/]", "  [white]Product ID[/]", "  [white]MAC Address[/]", "  [white]All[/]", "  [white]Exit[/]" })
                    .HighlightStyle(new Style(foreground: Color.FromHex("#5a47c6")))
                ).Replace("[white]", "").Replace("[/]", "");

                if (selection == "  Exit")
                {
                    AnsiConsole.Markup("[#5a47c6][[!]] [/][white]Exiting Spoofy... Goodbye![/]");
                    Thread.Sleep(2000);

                    break;
                }

                switch (selection)
                {
                    case "  HWID":

                        ChangeHWID();

                        break;

                    case "  Machine GUID":

                        ChangeMachineGUID();

                        break;

                    case "  Computer Name":

                        ChangeComputerName();

                        break;

                    case "  Product ID":

                        ChangeProductID();

                        break;

                    case "  MAC Address":

                        ChangeMACAddress();

                        break;

                    case "  All":

                        ChangeAll();

                        break;
                }

                AnsiConsole.MarkupLine($"\n[#5a47c6][[!]] [/][white]Changes have been applied! A system restart may be needed for certain changes to take effect (e.g., Computer Name, MAC Address, etc.)[/]\n");
                AnsiConsole.Markup("[#5a47c6][[->]] [/][white]Press any key to go back to the main menu...[/]");
                Console.ReadKey();
                AnsiConsole.Clear();
                DisplayWelcome();
            }
        }

        private static void DisplayWelcome()
        {
            string _fancyText = fancyText;
            string _creditsText = creditsText;
            string leftBracket = "[ ";
            string rightBracket = " ]";
            string[] lines = _fancyText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int consoleWidth = Console.WindowWidth;
            int startRow = 0;

            foreach (string line in lines)
            {
                int startColumn = Math.Max((consoleWidth - line.Length) / 2, 0);

                Console.SetCursorPosition(startColumn, startRow++);
                AnsiConsole.MarkupLine($"[#5a47c6]{line}[/]");
            }

            AnsiConsole.MarkupLine("");

            int totalLength = leftBracket.Length + _creditsText.Length + rightBracket.Length;
            int leftPadding = (consoleWidth - totalLength) / 2;
            int rightPadding = consoleWidth - totalLength - leftPadding;

            AnsiConsole.Markup(new string(' ', leftPadding));
            AnsiConsole.Markup("[#5a47c6][[ [/]");
            AnsiConsole.Markup($"[white]{_creditsText}[/]");
            AnsiConsole.Markup("[#5a47c6] ]][/]");
            AnsiConsole.Markup(new string(' ', rightPadding));
            AnsiConsole.MarkupLine("\n\n\n");
        }

        private static bool IsAdministrator()
        {
            using (WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);

                return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private static void ChangeHWID()
        {
            ChangeMachineGUID();
            ChangeComputerName();
        }

        private static void ChangeMachineGUID()
        {
            try
            {
                string newGuid = Guid.NewGuid().ToString();

                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid", newGuid);
                AnsiConsole.MarkupLine($"[lime][[SUCCESS]] [/][white]Machine GUID changed to: {newGuid}[/]");
            }

            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Failed to change Machine GUID: {ex.Message}[/]");
            }

            // AnsiConsole.WriteLine();
        }

        private static void ChangeComputerName()
        {
            try
            {
                string newName = Generate.RandomComputerName(7);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject mo in collection)
                {
                    ManagementBaseObject inParams = mo.GetMethodParameters("Rename");

                    inParams["Name"] = newName;

                    ManagementBaseObject outParams = mo.InvokeMethod("Rename", inParams, null);

                    uint returnValue = (uint)outParams["ReturnValue"];

                    if (returnValue == 0)
                    {
                        AnsiConsole.MarkupLine($"[lime][[SUCCESS]] [/][white]Computer Name changed to: {newName}[/]");
                    }

                    else
                    {
                        AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Failed to change Computer Name. Return code: {returnValue}[/]");
                    }
                }
            }

            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Failed to change Computer Name: {ex.Message}[/]");
            }

            // AnsiConsole.WriteLine();
        }

        private static void ChangeProductID()
        {
            try
            {
                string newProductId = Generate.RandomProductID();

                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductId", newProductId);
                AnsiConsole.MarkupLine($"[lime][[SUCCESS]] [/][white]Product ID changed to: {newProductId}[/]");
            }

            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Failed to change Product ID: {ex.Message}[/]");
            }

            // AnsiConsole.WriteLine();
        }

        private static void ChangeMACAddress()
        {
            try
            {
                string classKeyPath = @"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}";

                using (RegistryKey classKey = Registry.LocalMachine.OpenSubKey(classKeyPath, true))
                {
                    if (classKey == null)
                    {
                        AnsiConsole.MarkupLine("[red][[ERROR]] [/][white]Unable to access network adapter registry key![/]");

                        return;
                    }

                    foreach (string subKeyName in classKey.GetSubKeyNames())
                    {
                        if (subKeyName.Length == 4 && int.TryParse(subKeyName, out _))
                        {
                            using (RegistryKey adapterKey = classKey.OpenSubKey(subKeyName, true))
                            {
                                if (adapterKey != null && adapterKey.GetValue("DriverDesc") != null)
                                {
                                    string newMac = Generate.RandomMACAddress();

                                    adapterKey.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
                                    AnsiConsole.MarkupLine($"[lime][[SUCCESS]] [/][white]MAC Address for adapter {adapterKey.GetValue("DriverDesc")} changed to: {newMac}[/]");
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red][[ERROR]] [/][white]Failed to change MAC Address: {ex.Message}[/]");
            }

            // AnsiConsole.WriteLine();
        }

        private static void ChangeAll()
        {
            ChangeHWID();
            ChangeProductID();
            ChangeMACAddress();
        }
    }
}