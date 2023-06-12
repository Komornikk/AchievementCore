using MSCLoader;

public class DebugController : ConsoleCommand
{
    public override string Name => "achievementcore_debug";
    public override string Help => "debug mode for AchievementCore";
    public override void Run(string[] args)
    {
        AchievementCore.DEBUG = !AchievementCore.DEBUG;
        ModConsole.Print($"<color=red> DEBUG MODE = </color><color=green>{AchievementCore.DEBUG}</color>");
    }
}