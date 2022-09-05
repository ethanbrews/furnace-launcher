using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Furnace.UnitTests;

[TestClass]
public class GameInstallTest
{
    [TestMethod]
    public async Task InstallGame()
    {
        var gameInstaller = new Actions.Minecraft.MinecraftInstallAction("1.19.2");
        await gameInstaller.RunAsync();
    }

    [TestMethod]
    public void ValidateGame()
    {
        
    }

    [TestMethod]
    public void DeleteGame()
    {
        
    }
}