// See https://aka.ms/new-console-template for more information
using Minefield.Factories;
using Minefield.Rules;
using Minefield.Services;
using Unity;

UnityContainer container = new();
container.RegisterType<IGameLaunchService, GameLaunchService>();
container.RegisterType<IGameService, GameService>();
container.RegisterType<IRandomFactory, RandomFactory>();
container.RegisterType<IGameSetupService, GameSetupService>();
container.RegisterType<IConsoleService, ConsoleService>();
container.RegisterType<IMoveRule, LeftMoveRule>(nameof(LeftMoveRule));
container.RegisterType<IMoveRule, RightMoveRule>(nameof(RightMoveRule));
container.RegisterType<IMoveRule, UpMoveRule>(nameof(UpMoveRule));
container.RegisterType<IMoveRule, DownMoveRule>(nameof(DownMoveRule));

var gameLaunchService = container.Resolve<IGameLaunchService>();
gameLaunchService.LaunchGame();