using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Furnace.UnitTests;

[TestClass]
public class VanillaRunTest
{
    /*[TestMethod]
    public void RunVanillaAndForget()
    {
        
    }*/

    [TestMethod]
    public async Task RunVanillaInProcess()
    {
        var runner = new Actions.Minecraft.MinecraftRunAction("1.19.2");
        await runner.RunAsync();
    }
}