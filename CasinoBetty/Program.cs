using CasinoBetty.Commands;
using CasinoBetty.Services;
using CasinoBetty.Services.Interfaces;

ICasinoService casino = new CasinoService(
    new DepositCommand(),
    new BetCommand(new CasinoRNGCommand()),
    new WithdrawalCommand());

var input = "";

while (input != "exit")
{
    Console.WriteLine("Please, submit action:");
    input = Console.ReadLine();

    if (input == "exit")
    {
        Console.WriteLine("Thank you for playing! Hope to see you again soon");
        return;
    }

    var result = casino.Interact(input);

    Console.WriteLine(result);
}