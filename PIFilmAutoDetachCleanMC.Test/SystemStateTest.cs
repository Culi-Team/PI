using EQX.Core.Sequence;
using PIFilmAutoDetachCleanMC.Defines;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class SystemStateTest
    {
        [Fact]
        public void UserPhysicalAction_FuncTest()
        {
            UserPhysicalAction physicalAction = new UserPhysicalAction();

            physicalAction.StartPushed = () =>
            {
                return true;
            };

            Assert.Equal(EOperationCommand.Start, physicalAction.PhysicCommand);
        }
    }
}